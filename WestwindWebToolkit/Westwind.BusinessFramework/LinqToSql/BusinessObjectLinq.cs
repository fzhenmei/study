using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data;
using System.Data.Common;
using System.Xml.Serialization;
using Westwind.Utilities;
using System.Reflection;
using System.Linq.Expressions;
using Westwind.BusinessFramework;

namespace Westwind.BusinessFramework.LinqToSql
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
    public abstract class BusinessObjectLinq<TEntity,TContext> : IDisposable
           where TEntity: class, new()
           where TContext: DataContextSql, new()
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

        public TableInfo TableInfo
        {
          get 
          {     
              return _TableInfo; 
          }
          set { _TableInfo = value; }
        }
        private TableInfo _TableInfo = null;


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
        public QueryConverter<TEntity,TContext> Converter
        {
            get
            {
                if (_QueryConverter == null)
                    _QueryConverter = new QueryConverter<TEntity, TContext>(this,Context as DataContextSql);
                return _QueryConverter ; 
            }            
        }
        private QueryConverter<TEntity,TContext> _QueryConverter = null;

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
        public BusinessObjectLinq()
        {
            IntializeInternal();
            Initialize();
        }

        /// <summary>
        /// Constructore that allows passing in an existing DataContext
        /// so several business objects can share Context scope.
        /// </summary>
        /// <param name="context"></param>
        public BusinessObjectLinq(TContext context)
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

            // Initialize Table Info 
            TableInfo = CreateTableInfo();
            if (TableInfo == null)
                TableInfo = new TableInfo(Context, typeof(TEntity));            
        }

        /// <summary>
        /// Creates an instance of the context object.
        /// </summary>
        /// <returns></returns>
        protected virtual TContext CreateContext()
        {
            return new TContext() as TContext; 
            // Activator.CreateInstance<TContext>() as TContext;
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
            return Activator.CreateInstance( typeof(TContext), ConnectionString) as TContext;
        }

        /// <summary>
        /// Instantiates a DataContext by passing in a provider specific connection
        /// object
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        protected virtual TContext CreateContext(IDbConnection connection)
        {            
            return Activator.CreateInstance(typeof(TContext), connection) as TContext;
        }


        /// <summary>
        /// Overridable Factory for TableInfo structure. Allows code to override
        /// loading of this structure. Return non-null to override.
        /// </summary>
        /// <returns></returns>
        protected virtual TableInfo CreateTableInfo()
        {
            return null;
        }

        #endregion

        #region CRUD Methods

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
        public virtual TEntity Load(object pk)
        {            
            // Since we have dynamic expressions and a simple query let's just run the 
            // query as a string query with LinqToSql's ExecuteCommand<>. More efficient than using DynamicQuery
            string sql = "select * from " + TableInfo.Tablename + " where " + TableInfo.PkField + "={0}";
            return LoadBase(sql, pk);
        }

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
        /// <param name="pk">integer primary key</param>
        /// <returns>entity or null. Also sets Entity property</returns>
        public virtual TEntity Load(int pk)
        {
            // Since we have dynamic expressions and a simple query let's just run the 
            // query as a string query with LinqToSql's ExecuteCommand<>. More efficient than using DynamicQuery
            string sql = "select * from " + TableInfo.Tablename + " where " + TableInfo.PkField + "={0}";
            return LoadBase(sql, pk);
        }

        /// <summary>
        /// Loads an individual instance of an entity and returns the instance.
        ///  Entity is also set with the entity if loaded.
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public virtual TEntity Load(string pk)
        {
            // Since we have dynamic expressions and a simple query let's just run the 
            // query as a string query with LinqToSql's ExecuteCommand<>. More efficient than using DynamicQuery
            string sql = "select * from " + TableInfo.Tablename + " where " + TableInfo.PkField + "={0}";
            return LoadBase(sql, pk);
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
                if (Options.TrackingMode == TrackingModes.Disconnected)
                    context = CreateContext();
                
                IEnumerable<TEntity> entityList = context.ExecuteQuery<TEntity>(sqlLoadCommand,args);

                TEntity entity = null;
                entity = entityList.Single();

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
                if (Options.TrackingMode == TrackingModes.Disconnected)
                    context = CreateContext();

                var res = Context.GetTable<TEntity>().Where(whereClauseLambda);

                Entity = Context.GetTable<TEntity>().Where(whereClauseLambda).SingleOrDefault();

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

                if (Options.TrackingMode == TrackingModes.Disconnected)
                    return entity;

                if (!noContextInsert)
                {
                    Table<TEntity> table = Context.GetTable(typeof(TEntity)) as Table<TEntity>;
                    table.InsertOnSubmit(entity);
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

            if (!OnBeforeSave(entity))
                return false;

            // If available this will get set - not guaranteed! Check for null.
            EntityBase baseEntity = Entity as EntityBase;


            // In connected mode any Save operation causes
            // all changes to be submitted.
            if (Options.TrackingMode == TrackingModes.Connected && entity == null)
            {                
                try
                {
                    bool result = SubmitChanges();
                    if (result)
                    {
                        // clear new flag
                        if (baseEntity != null)
                            baseEntity.EntityState.IsNew = false;
                    }

                    if (!OnAfterSave(entity))
                        return false;

                    return result;
                }                
                catch (Exception ex)
                {
                    SetError(ex);                   
                }
                return false;
            }

            if (entity == null)
                entity = Entity;

            using (TContext context = CreateContext())
            {
                try
                {                    
                    // Generically get the table
                    Table<TEntity> table = context.GetTable( typeof(TEntity) ) as Table<TEntity>;

                    bool IsNew = false;

                    // Determine New status based on whether Timestamp (if available) or Pk 
                    // is set to null or empty to signify new object

                    // Try to use Timestamp field if available because it's easiest
                    if (baseEntity != null)
                    {
                        IsNew = baseEntity.EntityState.IsNew;
                    }
                    else if (TableInfo.VersionField != null)
                    {
                        object tstamp = entity.GetType()
                                              .GetProperty(TableInfo.VersionField, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                              .GetValue(entity, null);
                        IsNew = (tstamp == null);
                    }
                    else   // try to do it off the PK field
                    {
                        object pkVal = entity.GetType().GetProperty(TableInfo.PkField).GetValue(entity, null);
                        IsNew = IsPkEmpty(pkVal);
                    }

                    
                    // If there's no timestamp on the entity it's a new record
                    if (IsNew)
                        table.InsertOnSubmit(entity);
                    else
                        table.Attach(entity, true);

                    context.SubmitChanges();

                    if (baseEntity != null)
                        baseEntity.EntityState.IsNew = false;


                }
                catch (Exception ex)
                {
                    SetError(ex);
                    return false;
                }
            }

            if (!OnAfterSave(entity))
                return false;

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
                Context = CreateContext(Context.Connection.ConnectionString);
            else
                Context = CreateContext();
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
                Context.SubmitChanges(ConflictMode.ContinueOnConflict);
            }
            catch (ChangeConflictException ex)
            {
                switch (ConflictResolutionMode)
                { 
                    // Pass the error out of here as is
                    // Let the client deal with it
                    case ConflictResolutionModes.None:
                        SetError(ex);
                        return false;

                    // Last one wins
                    case ConflictResolutionModes.ForceChanges:
                        Context.ChangeConflicts.ResolveAll(RefreshMode.KeepChanges);
                        break;

                    // All changes are aborted and entities update from database
                    case ConflictResolutionModes.AbortChanges:
                        Context.ChangeConflicts.ResolveAll(RefreshMode.OverwriteCurrentValues);
                        break;
                    //case ConflictResolutionModes.WriteNonConflictChanges
                    //    // TODO: Check this ConflictResoltuionmode behavior
                    //    context2.ChangeConflicts.ResolveAll(RefreshMode.KeepCurrentValues);
                    //    break;
                }
                try
                {
                    Context.SubmitChanges(ConflictMode.ContinueOnConflict);                    
                }
                catch(Exception ex2)
                {
                    SetError(ex2);
                    return false;
                }
            }
            catch (Exception ex)
            {
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
                                    "delete from " + TableInfo.Tablename + " where " + TableInfo.PkField + "=@Pk",
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

            if (Options.TrackingMode == TrackingModes.Connected)
            {
                try
                {
                    Table<TEntity> table = Context.GetTable(typeof(TEntity)) as Table<TEntity>;
                    table.DeleteOnSubmit(entity);                    
                    return SubmitChanges();
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
                    Table<TEntity> table = context.GetTable(typeof(TEntity)) as Table<TEntity>;
                    table.DeleteOnSubmit(entity);                    
                    context.SubmitChanges();
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
        /// Called before Save() makes the actual database call
        /// to save the object.
        /// 
        /// Return false to signify that you don't want to
        /// save.
        /// </summary>
        /// <param name="entity"></param>
        protected virtual bool OnBeforeSave(TEntity entity)
        {
            return true;
        }

        /// <summary>
        /// Called after the entity has been saved to disk
        /// successfully.
        /// 
        /// Return false to indicate that Save() should return
        /// false even though the entity was successfully saved.        
        /// </summary>
        /// <param name="entity"></param>        
        /// <returns></returns>
        protected virtual bool OnAfterSave(TEntity entity)
        {
            return true;
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


}
