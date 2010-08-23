using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Xml.Serialization;
using Westwind.Utilities;
using System.Reflection;
using System.Linq.Expressions;
using System.Data.Objects;
using System.Data.Linq;
using System.Data.Objects.DataClasses;

namespace Westwind.BusinessFramework.EntityFramework
{
         
    /// <summary>
    /// Base Business Object class that wrappers LINQ as a data access layer.
    /// 
    /// Assume:
    /// Operations work best and efficiently when tables have a TimeStamp field
    /// 
    /// Each business object maps to a primary Entity/Table, but can of course
    /// access other tables.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TContext"></typeparam>    
    public abstract class BusinessObjectEF<TEntity,TContext> : IDisposable
           where TEntity: class, new()
           where TContext: ObjectContextSql, new()
    {

        #region Properties
        /// <summary>
        /// The provider factory used for direct ADO.NET operations
        /// </summary>
        public static DbProviderFactory ProviderFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");

        /// <summary>
        /// Instance of the Data Context that is used for this class.
        /// Note that this is a primary instance only - other instances
        /// can be used in other situations.
        /// </summary>
        public TContext Context { get; set; }


        /// <summary>
        /// Contains information about the primary table that is mapped
        /// to this business object. Contains table name, Pk and version
        /// field info. 
        /// 
        /// Values are automatically set by the constructor so ensure
        /// that the base constructor is always called.
        /// </summary>        

        public TableInfo<TEntity> TableInfo
        {
            get
            {
                if (_TableInfo == null)
                    _TableInfo = new TableInfo<TEntity>();

                return _TableInfo;
            }
            set { _TableInfo = value; }
        }
        private TableInfo<TEntity> _TableInfo = null;


        /// <summary>
        /// Contains options for the business object's operation
        /// </summary>
        public BusinessObjectOptions Options { get; set; }


        /// <summary>
        /// Object that handles conversion of queries into concreate
        /// types and data structures. Allows conversion of queries
        /// to a data reader, data table as well as the standard 
        /// behaviors  ToList(), ToArray() using the entity class
        /// as its input.
        /// 
        /// This routine is useful in that it provides object parameters
        /// the abillity to return output in a variety of ways which
        /// makes anonymous type result more usable outside of local scope.
        /// </summary>
        //public QueryConverter<TEntity,TContext> Converter
        //{
        //    get
        //    {
        //        if (_QueryConverter == null)
        //            _QueryConverter = new QueryConverter<TEntity, TContext>(this,Context as DataContextEF);
        //        return _QueryConverter ; 
        //    }            
        //}
        //private QueryConverter<TEntity,TContext> _QueryConverter = null;

        /// <summary>
        /// Instance of a locally managed entity object. Set with Load and New
        /// methods.
        /// </summary>
        public TEntity Entity {get; set; }


        /// <summary>
        /// Determines whether or not the Save operation causes automatic
        /// validation
        /// </summary>        
        protected bool AutoValidate { get; set; }

        /// <summary>
        /// Instance of an exception object that caused the last error
        /// </summary>                
        [XmlIgnore]        
        public Exception ErrorException
        {
            get { return _ErrorException; }
            set { _ErrorException = value; }
        }
        [NonSerialized]
        private Exception _ErrorException = null;


        /// <summary>
        /// A collection that can be used to hold errors. This collection
        /// is set by the AddValidationError method.
        /// </summary>
        [XmlIgnore]
        public ValidationErrorCollection ValidationErrors
        {
            get
            {
                if (_ValidationErrors == null)
                    _ValidationErrors = new ValidationErrorCollection();
                return _ValidationErrors;
            }            
        }
        [NonSerialized]
        ValidationErrorCollection _ValidationErrors;

        
        /// <summary>
        /// Error Message of the last exception
        /// </summary>
        public string ErrorMessage
        {
            get 
            { 
                if (ErrorException == null)
                    return "";
                return ErrorException.Message;
            }
            set 
            { 
                if ( string.IsNullOrEmpty(value) )
                    ErrorException = null;
                else
                    // Assign a new exception
                    ErrorException = new ApplicationException(value);                
            }
        }



#endregion




        #region Object Initialization
        /// <summary>
        /// Base constructor - initializes the business object's
        /// context and table mapping info
        /// </summary>
        public BusinessObjectEF()
        {
            IntializeInternal();
            Initialize();
        }

        /// <summary>
        /// Constructore that allows passing in an existing DataContext
        /// so several business objects can share Context scope.
        /// </summary>
        /// <param name="context"></param>
        public BusinessObjectEF(TContext context)
        {
            IntializeInternal();
            Context = context;
            Initialize();            
        }

        /// <summary>
        /// Internal method called to initialize various sub objects
        /// and default settings.
        /// </summary>
        private void IntializeInternal()
        {
            // Create the options for this business object
            Options = new BusinessObjectOptions();
        }

        /// <summary>
        /// Initializes the business object explicitly.
        /// 
        /// This method can be overridden by any subclasses that want to customize
        /// the instantiation behavior and should call back to the base method
        /// 
        /// The core features this method performs are:
        /// - Create a new context     
        /// - Creates the TableInfo
        /// 
        /// Overrides should create the Context FIRST 
        /// then call back into the base Initialize
        /// </summary>
        protected virtual void Initialize()
        {
            // Create a default context
            if (Context == null)
            {
                if (!string.IsNullOrEmpty(Options.ConnectionString))
                    Context = CreateContext(Options.ConnectionString);
                else
                    Context = CreateContext();
            }

            TableInfo = CreateTableInfo() ?? new TableInfo<TEntity>(Context);
        }

        /// <summary>
        /// Create an instance of the TableInfo structure. Can be overridden
        /// to allow customization.
        /// 
        /// If null is returned 
        /// </summary>
        /// <returns></returns>
        protected virtual TableInfo<TEntity> CreateTableInfo()
        {
            return null;
        }


        /// <summary>
        /// Creates an instance of the context object.
        /// </summary>
        /// <returns></returns>
        protected virtual TContext CreateContext()
        {
            var context = new TContext() as TContext;
            EnsureMetaDataIsLoaded(context);
            return context;            
        }

        /// <summary>
        /// Allows creating a new context with a specific connection string.
        /// 
        /// The connection string can either be a full connection string or
        /// a connection string .Config entry.
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        protected virtual TContext CreateContext(string ConnectionString)
        {
            var context = Activator.CreateInstance( typeof(TContext), ConnectionString) as TContext;
            EnsureMetaDataIsLoaded(context);
            return context;
        }

        /// <summary>
        /// Ensures that the MetaData
        /// </summary>
        /// <param name="context"></param>
        protected void EnsureMetaDataIsLoaded(TContext context)
        {
            if (context == null)
                return;

            // Hack: Create an object query.
            var temp = new ObjectQuery<TEntity>(context.DefaultContainerName + "." + typeof(TEntity).Name,
                                    context,
                                    MergeOption.NoTracking);
            temp.ToTraceString();
        }

        #endregion

        #region CRUD Methods


        /// <summary>
        /// Loads an individual instance of an entity and returns the instance.
        ///  Entity is also set with the entity if loaded.
        /// 
        /// This method works with single primary keys
        /// of:
        /// int
        /// string
        /// Guid
        /// </summary>
        /// <param name="id">integer primary key</param>
        /// <returns>entity or null. Also sets Entity property</returns>
        public virtual TEntity Load(int id)
        {
            Entity = Context.GetObjectByKey(new EntityKey(TableInfo.EntityTablename, TableInfo.PkField, id)) as TEntity;

            if (Entity != null)
                OnLoaded(Entity);

            // and return instance
            return Entity;
        }

        public virtual TEntity Load(Guid id)
        {
            Entity = Context.GetObjectByKey(new EntityKey(TableInfo.EntityTablename, TableInfo.PkField, id)) as TEntity;            
                
            if (Entity != null)
                OnLoaded(Entity);

            // and return instance
            return Entity;
        }

        /// <summary>
        /// Loads an individual instance of an entity and returns the instance.
        ///  Entity is also set with the entity if loaded.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity Load(string id)
        {
            Entity = Context.GetObjectByKey(new EntityKey(TableInfo.EntityTablename, TableInfo.PkField, id)) as TEntity;

            if (Entity != null)
                OnLoaded(Entity);

            // and return instance
            return Entity;
        }


        /// <summary>
        /// Loads an individual instance of an object and returns the instance.
        ///  Entity is also set with the entity if loaded.
        /// 
        /// This method works with single primary keys
        /// of:
        /// int
        /// string
        /// Guid
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public virtual TEntity Load(object id)
        {
            Entity = Context.GetObjectByKey(new EntityKey(TableInfo.EntityTablename, TableInfo.PkField, id)) as TEntity;

            if (Entity != null)
                OnLoaded(Entity);

            // and return instance
            return Entity;
        }

        /// <summary>
        /// Loads an entity based on a lambda expression.
        /// This allows a bit more control over which record
        /// to retrieve.
        /// 
        /// Note: The behavior of this method may result in
        /// multiple matches and only the first match is retrieved
        /// </summary>
        /// <param name="predicate">a where clause Lambda expression</param>
        /// <returns>Entity object or null. Entity is also set</returns>
        public virtual TEntity Load(Expression<Func<TEntity,bool>> whereClauseLambda)
        {
            return LoadBase(whereClauseLambda);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual TEntity LoadBase(ObjectQuery<TEntity> query)
        {
            var list = query.ToList();
            
            Entity = query.SingleOrDefault();

            if (Entity != null)
                OnLoaded(Entity);

            // and return instance
            return Entity;
        }

        /// <summary>
        /// Loads a single record based on a generic SQL command. Can be used
        /// for customized Load behaviors where entities are loaded up.
        /// </summary>
        /// <param name="sqlLoadCommand"></param>
        /// <returns></returns>
        protected virtual TEntity LoadBase(string sqlLoadCommand, params object[] args)
        {
            SetError();

            try
            {
                TContext context = Context;

                // If disconnected we'll create a new context
                if (Options.TrackingMode == TrackingMode.Disconnected)
                    context = CreateContext();
                
                IEnumerable<TEntity> entityList = context.ExecuteStoreQuery<TEntity>(sqlLoadCommand,args);

                TEntity entity = null;
                entity = entityList.SingleOrDefault();

                // Assign to local entity
                Entity = entity;

                if (Entity != null)
                    OnLoaded(entity);

                // and return instance
                return entity;
            }
            catch (InvalidOperationException)
            {
                // Handles errors where an invalid Id was passed, but SQL is valid
                SetError("Couldn't load entity - invalid key provided.");
                Entity = null;
                return null;
            }   
            catch (Exception ex)
            {                
                // handles Sql errors                
                Entity = null;
                SetError(ex);
            }

            return null;
        }

        /// <summary>
        /// Loads an entity based on a predicate expression that 
        /// can be applied against the bus object's TEntity type.
        /// </summary>
        /// <param name="predicate">Lambda where expression  that returns bool</param>
        /// <returns></returns>
        protected virtual TEntity LoadBase(Expression<Func<TEntity, bool>> whereClauseLambda)
        {
            SetError();

            try
            {
                TContext context = Context;

                // If disconnected we'll create a new context
                if (Options.TrackingMode == TrackingMode.Disconnected)
                    context = CreateContext();

                //var res = Context.CreateObjectSet<TEntity>().Where(whereClauseLambda);

                Entity = Context.CreateObjectSet<TEntity>().Where(whereClauseLambda).SingleOrDefault();

                if (Entity != null)
                    OnLoaded(Entity);                

                return Entity;
            }
            catch (InvalidOperationException)
            {
                // Handles errors where an invalid Id was passed, but SQL is valid
                SetError("Couldn't load entity - invalid key provided.");
                Entity = null;
                return null;
            }
            catch (Exception ex)
            {
                // handles Sql errors                
                Entity = null;
                SetError(ex);
            }

            return null;
        }

        /// <summary>
        /// Create a new entity instance. When in connected mode the instance
        /// is automatically added to the context unless you pass false to the
        /// constructor.
        /// </summary>
        /// <returns></returns>
        public virtual TEntity NewEntity(bool noContextInsert)
        {
            SetError();

            try
            {
                TEntity entity = new TEntity(); // Activator.CreateInstance<TEntity>();

                Entity = entity;

                if (Entity is EntityBase)
                {
                    EntityBase baseInstance = Entity as EntityBase;
                    baseInstance.EntityState.IsNew = true;
                }

                if (Options.TrackingMode == TrackingMode.Disconnected)
                    return entity;

                if (!noContextInsert)
                {
                    ObjectSet<TEntity> table = Context.CreateObjectSet<TEntity>();
                    table.AddObject(entity);
                }

                OnNewEntityLoaded(entity);
                
                return entity;
            }
            catch (Exception ex)
            {
                SetError(ex);
                return null;
            }
        }

        /// <summary>
        /// Create a new entity instance on the Entity property and return
        /// the instance. The instance created is automatically added to the context
        /// in connected mode.
        /// </summary>
        /// <returns></returns>
        public virtual TEntity NewEntity()
        {
            return NewEntity(false);
        }

        /// <summary>
        /// Saves a disconnected entity object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Save(TEntity entity)        
        {
            if (AutoValidate && !Validate())
                return false;

            OnBeforeSave(entity);

            // If available this will get set - not guaranteed! Check for null.
            EntityObject baseEntity = Entity as EntityObject;

            // In connected mode any Save operation causes
            // all changes to be submitted.
            if (Options.TrackingMode == TrackingMode.Connected && entity == null)
            {                
                    return SubmitChanges();
            }

            if (entity == null)
                entity = Entity;

            using (TContext context = CreateContext())
            {
                try
                {                    
                    // Generically get the table
                    ObjectSet<TEntity> table = context.CreateObjectSet<TEntity>();


                    // Try to use Timestamp field if available because it's easiest
                    bool IsNew = false;
                    if (baseEntity != null)
                        IsNew = baseEntity.EntityState == System.Data.EntityState.Added;                                
                    
                    // If there's no timestamp on the entity it's a new record
                    if (IsNew)
                        table.AddObject(entity);
                    else                    
                        table.Attach(entity);
                    
                    int result = context.SaveChanges();
                }
                catch (Exception ex)
                {
                    SetError(ex);
                    return false;
                }
            }

            return true;            
        }

        /// <summary>
        /// Saves the internally stored Entity object
        /// by submitting all changes. Note this is the
        /// 'connected' version that submits all pending
        /// changes on the current data context.
        /// 
        /// For subclassing you should override the alternate
        /// entity signature.
        /// </summary>
        /// <returns></returns>
        public virtual bool Save()
        {
            return Save(null);
        }

        /// <summary>
        /// Aborts all changes made to tracked entities.
        /// 
        /// Effectively creates a new DataContext
        /// </summary>
        public void CancelChanges()
        {
            // Recreate the context cancels all changes
            if (Context != null && !string.IsNullOrEmpty(Context.Connection.ConnectionString) )
                CreateContext(Context.Connection.ConnectionString);
            else
                CreateContext();
        }

        /// <summary>
        /// Check to see if a PK value is empty which means
        /// we're dealing with a new record.
        /// 
        /// Supports string, int/int?, Guid
        /// </summary>
        /// <param name="pkVal"></param>
        /// <returns></returns>
        private bool IsPkEmpty(object pkVal)
        {
            bool IsNew = false;

            if (pkVal == null)
                IsNew = true;
            else if (pkVal is int)
            {
                if ((int)pkVal < 1)
                    IsNew = true;
            }
            else if (pkVal is Guid)
            {
                if ((Guid)pkVal == Guid.Empty)
                    IsNew = true;
            }
            return IsNew;
        }


        /// <summary>
        /// Cancel Changes on the current connected context
        /// </summary>
        public virtual void AbortChanges()
        {
            // Create a new context instance from scratch
            Context = CreateContext();
        }

        /// <summary>
        /// Saves changes on the current connected context.
        /// Preferrably you should use Save() rather than
        /// this method, but this provides a more low level
        /// direct context saving approach if you are
        /// working with connected data.
        /// 
        /// This method is also called from the Save() method.
        /// </summary>
        /// <param name="ConflictResolutionMode">Determines how change conflicts are applied</param>
        public bool SubmitChanges(ConflictResolutionModes ConflictResolutionMode)
        {
            try
            {
                // Always continue so we can get all the change conflicts
                Context.SaveChanges(SaveOptions.AcceptAllChangesAfterSave);
            }
            catch (UpdateException ex)
            {
                Exception ex2 = ex;
                if (ex.InnerException != null)
                    ex2 = ex.InnerException;
                SetError(ex2);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ex = ex.InnerException;
                SetError(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Saves changes on the current connected context.
        /// Preferrably you should use Save() rather than
        /// this method, but this provides a more low level
        /// direct context saving approach if you are
        /// working with connected data.
        /// </summary>
        public bool SubmitChanges()
        {
            return SubmitChanges(Options.ConflictResolutionMode);
        }


        /// <summary>
        /// Deletes an entity specific by its Pk
        /// </summary>
        /// <param name="Pk"></param>
        /// <returns></returns>
        public virtual bool Delete(object Pk)
        {            
            int result = -1;
            try
            {
                result = Context.ExecuteNonQuery(
                                    "delete from " + TableInfo.DbTableName + " where " + TableInfo.PkField + "=@Pk",
                                    Context.CreateParameter("@Pk", Pk));
            }
            catch (Exception ex)
            {
                SetError(ex);
                return false;
            }

            if (result < 1)
            {
                SetError("Nothing to delete");
                return false;
            }

            return true;                                
        }

        /// <summary>
        /// Deletes the active Entity object
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {            
            return Delete(null);
        }

        /// <summary>
        /// Deletes a specific entity object.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Delete(TEntity entity)
        {
            if (entity == null)
            {   
                entity = Entity;
                if (entity == null)
                    return false;
            }            

            if (Options.TrackingMode == TrackingMode.Connected)
            {
                try
                {
                    ObjectSet<TEntity> table = Context.CreateObjectSet<TEntity>();
                    table.DeleteObject(entity);                    
                    return  Save();
                }
                catch (Exception ex)
                {
                    SetError(ex);
                }
                return false;
            }

            using (TContext context = CreateContext())
            {
                try
                {
                    ObjectSet<TEntity> table = context.CreateObjectSet<TEntity>();
                    table.DeleteObject(entity);                    
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    SetError(ex);
                    return false;
                }
            }

            return true;            
        }


        /// <summary>
        /// Executes a query command against server using a DbCommand object.        
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="parameters">Named sql parameters</param>
        /// <returns></returns>
        protected int ExecuteNonQuery(DbCommand sqlCommand, params DbParameter[] parameters)
        {
            int result = -1;
            try
            {
                result = Context.ExecuteNonQuery(sqlCommand, parameters);
            }
            catch (Exception ex)
            {
                SetError(ex);
                return -1;
            }

            return result;
        }

        /// <summary>
        /// Executes a string based SQL Command
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters">Named sql parameters</param>
        /// <returns>-1 on failure, otherwise count of records affected</returns>
        protected int ExecuteNonQuery(string sql, params DbParameter[] parameters)
        {
            DbCommand command = Context.CreateCommand(sql);
            return ExecuteNonQuery(command, parameters);
        }

        /// <summary>
        /// Executes a SQL statement and returns a single value
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected object ExecuteScalar(DbCommand command, params DbParameter[] parameters)
        {
            object result;
            try
            {
                result = Context.ExecuteScalar(command, parameters);
            }
            catch (Exception ex)
            {
                SetError(ex);
                return null;
            }
            
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected object ExecuteScalar(string command, params DbParameter[] parameters)
        {
            return ExecuteScalar(Context.CreateCommand(command), parameters);
        }

        /// <summary>
        /// Executes a raw DbCommand and returns a DataReader
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters">Named sql parameters</param>
        /// <returns>null on failure, DataReader otherwise</returns>
        protected DbDataReader ExecuteReader(DbCommand command, params DbParameter[] parameters)        
        {
            try
            {
                return Context.ExecuteReader(command, parameters);
            }
            catch (Exception ex)
            {
                SetError(ex);
                return null;
            }
        }

        /// <summary>
        /// Executes a sql string and returns a data reader
        /// </summary>
        /// <param name="sql">Sql string to execute</param>
        /// <param name="parameters">Named sql parameters</param>
        /// <returns>null on failure. DataReader otherwise</returns>
        protected DbDataReader ExecuteReader(string sql, params DbParameter[] parameters)
        {
            DbCommand command = Context.CreateCommand(sql);
            return ExecuteReader(command, parameters);
        }

        /// <summary>
        /// Creates a DataReader from a query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected DbDataReader ExecuteReader(IQueryable query)
        {
            try
            {                
                return Context.ExecuteReader(query);
            }
            catch (Exception ex)
            {
                SetError(ex);
                return null;
            }
        }

        /// <summary>
        /// Returns an instance of a DataTable from a dbCommand object.
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected DataTable ExecuteDataTable(DbCommand sqlCommand, string tableName, params DbParameter[] parameters)
        {
            try
            {
                return Context.ExecuteDataTable(sqlCommand, tableName, parameters);
            }
            catch (Exception ex)
            {
                SetError(ex);
                return null;
            }
        }

        /// <summary>
        /// Returns an instance of a DataTable from a dbCommand object.
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected DataTable ExecuteDataTable(string sql, string tableName, params DbParameter[] parameters)
        {
            DbCommand command = Context.CreateCommand(sql);
            return ExecuteDataTable(command,tableName, parameters);
        }


        /// <summary>
        /// Returns an instance of a DataTable from a dbCommand object.
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected DataSet ExecuteDataSet(DbCommand sqlCommand, DataSet dataSet, string tableName, params DbParameter[] parameters)
        {
            try
            {
                return Context.ExecuteDataSet(sqlCommand, dataSet, tableName, parameters);
            }
            catch (Exception ex)
            {
                SetError(ex);
                return null;
            }
        }

        /// <summary>
        /// Returns an instance of a DataTable from a dbCommand object.
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected DataSet ExecuteDataSet(string sql, DataSet dataSet, string tableName, params DbParameter[] parameters)
        {
            DbCommand command = Context.CreateCommand(sql);
            return ExecuteDataSet(command, dataSet, tableName, parameters);
        }

        #endregion

        /// <summary>
        /// Validate() is used to validate business rules on the business object. 
        /// Generally this method consists of a bunch of if statements that validate 
        /// the data of the business object and adds any errors to the 
        /// <see>wwBusiness.ValidationErrors</see> collection.
        /// 
        /// If the <see>wwBusiness.AutoValidate</see> flag is set to true causes Save()
        ///  to automatically call this method. Must be overridden to perform any 
        /// validation.
        /// <seealso>Class wwBusiness Class ValidationErrorCollection</seealso>
        /// </summary>
        /// <returns>True or False.</returns>
        public bool Validate(TEntity entity)
        {
            ValidationErrors.Clear();
            
            OnValidate(entity);

            if (ValidationErrors.Count > 0)
            {
                SetError(ValidationErrors.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the current entity object. Overloads of this method
        /// should set the validation error collection and return false from
        /// this method.
        /// </summary>
        /// <returns></returns>
        public virtual bool Validate()
        {            
            return Validate(Entity);
        }

        /// <summary>
        /// Called after load has completed. Ideal point to hook post Load 
        /// logic that is fired from all Load() based overrides.        
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void OnLoaded(TEntity entity)
        {

        }

        /// <summary>
        /// Called after a new instance has been created. Ideal for creating
        /// and default values after new instance has been created.
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void OnNewEntityLoaded(TEntity entity)
        {

        }


        /// <summary>
        /// Called before Save() makes the actual database call.
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void OnBeforeSave(TEntity entity)
        {
        }

        /// <summary>
        /// Method that should be overridden in a business object to handle actual validation. 
        /// This method is called from the Validate method.
        /// 
        /// This method should add any errors to the <see cref="ValidationErrors"/> collection.
        /// </summary>
        /// <param name="entity">The entity to be validated</param>
        protected virtual void OnValidate(TEntity entity)
        {
        }

        #region GenericPropertyStorage

        /// <summary>
        // Dictionary of arbitrary property values that can be attached
        // to the current object. You can GetProperties, SetProperties
        // to load the properties to and from a text field.
        /// </summary>
        public Dictionary<string, object> Properties
        {
            get
            {
                if (_Properties == null)
                    _Properties = new Dictionary<string, object>();

                return _Properties;
            }
            set { _Properties = value; }
        }
        private Dictionary<string, object> _Properties = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringFieldNameToLoadFrom"></param>
        public bool GetProperties(string stringFieldNameToLoadFrom)
        {
            Properties = null;

            string fieldValue = ReflectionUtils.GetProperty(Entity, stringFieldNameToLoadFrom) as string;
            if (string.IsNullOrEmpty(fieldValue))
                return false;
           
            Properties = DataContractSerializationUtils.DeserializeXmlString(fieldValue,typeof(Dictionary<string,object>),true) as Dictionary<string,object>;
            return true;
        }

        /// <summary>
        /// Saves the Properties Dictionary to a specified field value
        /// </summary>
        /// <param name="stringFieldToSaveTo"></param>
        public void SetProperties(string stringFieldToSaveTo)
        {            
            string xml = DataContractSerializationUtils.SerializeToXmlString(Properties,true);
            ReflectionUtils.SetProperty(Entity, stringFieldToSaveTo, xml);
        }

        #endregion

        /// <summary>
        /// Sets an internal error message.
        /// </summary>
        /// <param name="Message"></param>
        public void SetError(string Message)
        {
            if (string.IsNullOrEmpty(Message))
            {
                ErrorException = null;
                return;
            }
            
            ErrorException = new ApplicationException(Message);

            if (Options.ThrowExceptions)
                throw ErrorException;

        }

        /// <summary>
        /// Sets an internal error exception
        /// </summary>
        /// <param name="ex"></param>
        public void SetError(Exception ex)
        {
            ErrorException = ex;
            ErrorMessage = ex.Message;
            if (ex != null && Options.ThrowExceptions)
                throw ex;
        }
        /// <summary>
        /// Clear out errors
        /// </summary>
        public void SetError()
        {
            ErrorException = null;
        }

        private bool disposed = false;
        protected void Dispose(bool dispoing)
        {
            if (!disposed)
            {
                if (Context != null)
                {
                    Context.Dispose();
                    Context = null;        
                    TableInfo = null;                    
                }            
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }


#if false   // Expression based Load Method - this works but Sql String is more efficient and easier understand

        //protected Expression<Func<TEntity, object>> pkFieldExpression;
        protected Expression<Func<TEntity, object>> pkFieldExpression;

        protected Expression<Func<TEntity, bool>> FilterByPk(object Pk)
        {
            // ent => PkFieldExpression(ent) == Pk          
            ParameterExpression entity = Expression.Parameter(typeof(TEntity), "ent");
            Expression keyValue = Expression.Invoke(pkFieldExpression, entity);
            Expression pkValue = Expression.Constant(Pk, keyValue.Type);
            Expression body = Expression.Equal(keyValue, pkValue);
            return Expression.Lambda<Func<TEntity, bool>>(body, entity);
        }


        public virtual TEntity LoadX(object Pk)
        {
            Table<TEntity> table = Context.GetTable<TEntity>();
            TEntity ent =  table.SingleOrDefault( FilterByPk(Pk) );
            Entity = ent;
            return ent;
        }
#endif

#if false // ReadOnlyContext option
        /// <summary>
        /// Contains a readonly context
        /// </summary>
        public TContext ReadOnlyContext
        {
            get 
            {
                if (_ReadOnlyContext == null)
                {
                    _ReadOnlyContext = CreateContext();
                    _ReadOnlyContext.ObjectTrackingEnabled = false;
                }
                
                return _ReadOnlyContext; 
            }
            set { _ReadOnlyContext = value; }
        }
        private TContext _ReadOnlyContext = null;
#endif
    }

    /// <summary>
    /// Determines how LINQ Change Tracking is applied
    /// </summary>
    public enum TrackingMode 
    {
        /// <summary>
        /// Uses a LINQ connected data context for change management
        /// whenever possible. Save and SubmitChanges operation is used
        /// to persist changes. In general this provides better performance
        /// for change tracking.
        /// </summary>
        Connected,

        /// <summary>
        /// Creates a new DataContext for each operation and performs .Save 
        /// operations by reconnecting to the DataContext.
        /// </summary>
        Disconnected
    }

    public enum ResultListTypes
    {
        DataReader,
        DataTable,
        DataSet
    }

    /// <summary>
    /// Determines how conflicts on SubmitChanges are handled.
    /// </summary>
    public enum ConflictResolutionModes
    {
        /// <summary>
        /// No Conflict resolution - nothing is done when conflicts
        /// occur. You can check Context.ChangeConflicts manually
        /// </summary>
        None,
        /// <summary>
        /// Forces all changes to get written. Last one wins strategy
        /// </summary>
        ForceChanges,
        /// <summary>
        /// Aborts all changes and updates the entities with the values
        /// from the database.
        /// </summary>
        AbortChanges,
        /// <summary>
        /// Writes all changes that are not in conflict. Updates entities
        /// with values from the data.
        /// </summary>
        //WriteNonConflictChanges
    }



}
