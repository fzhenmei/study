using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Data.Objects;
using System.Data.EntityClient;

namespace Westwind.BusinessFramework.EntityFramework
{
    /// <summary>
    /// Customization of the LINQ to SQL DataContext class that provides
    /// core ADO.NET Data Access methods to the data context.
    /// </summary>
    public class ObjectContextSql : ObjectContext
    {
        
        /// <summary>
        /// Internal Provider factory for native commands
        /// </summary>
        public  static DbProviderFactory DbProvider = DbProviderFactories.GetFactory("System.Data.SqlClient");

        /// <summary>
        /// Internally used raw SQL provider connection
        /// </summary>
        protected DbConnection DbConnection
        {
            get 
            {
                return ((EntityConnection) this.Connection).StoreConnection;                         
            }
        }

        /// <summary>
        /// Internal locking object
        /// </summary>
        private object _syncLock = new object();
            
        /// <summary>
        /// Initialize a new HelpBuilderEntities object.
        /// </summary>
        public ObjectContextSql(string connectionString) : base(connectionString)
        {
            this.ContextOptions.LazyLoadingEnabled = true;            
        }

        public ObjectContextSql(string connectionString,string defaultContainerName)
            : base(connectionString,defaultContainerName)
        {
            this.ContextOptions.LazyLoadingEnabled = true;
        }
   
        /// <summary>
        /// Initialize a new HelpBuilderEntities object.
        /// </summary>
        public ObjectContextSql(EntityConnection connection) : base(connection)
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            
        }
        /// <summary>
        /// Initialize a new HelpBuilderEntities object.
        /// </summary>
        public ObjectContextSql(EntityConnection connection, string defaultContainerName)
            : base(connection,defaultContainerName)
        {
            this.ContextOptions.LazyLoadingEnabled = true;

        }

        #region Base Operations (open/close connections, Parameters, Commands)

        /// <summary>
        /// Opens a connection to the database
        /// </summary>
        /// <returns></returns>
        public bool OpenConnection()
        {            

            if (this.DbConnection.State != ConnectionState.Open)
                this.DbConnection.Open();
           
            return true;
        }

        /// <summary>
        /// Opens the connection on this data context
        /// </summary>
        /// <returns></returns>
        public void CloseConnection()
        {
            this.DbConnection.Close();
        }


        /// <summary>
        /// Creates a new SQL Command
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public DbCommand CreateCommand(string sql, params DbParameter[] dbParameters)
        {
            DbCommand sqlCommand = DbProvider.CreateCommand();
            sqlCommand.CommandText = sql;

            sqlCommand.Connection = this.DbConnection;

            if (dbParameters != null)
            {
                foreach (DbParameter parm in dbParameters)
                    sqlCommand.Parameters.Add(parm);
            }

            return sqlCommand;
        }

        /// <summary>
        /// Creates a new SQL Parameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string name, object value)
        {
            DbParameter parm = DbProvider.CreateParameter();
            parm.ParameterName = name;
            parm.Value = value;
            return parm;
        }

        

        /// <summary>
        /// Creates a new SQL Parameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string name, object value, ParameterDirection direction)
        {
            DbParameter parm = DbProvider.CreateParameter();
            parm.ParameterName = name;
            parm.Value = value;
            parm.Direction = direction;
            return parm;
        }


        /// <summary>
        /// Retrieves a DbCommand from an IQueryable. 
        /// 
        /// Note: This routine may have parameter mapping imperfections 
        /// due to the limited parameter data available in the query's
        /// parameter collection.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbCommand GetCommand(IQueryable query)
        {
            ObjectQuery q = query as ObjectQuery;

            if ( q == null )
                throw new InvalidCastException("Query could not be converted to an ObjectQuery");

            DbCommand command = DbProvider.CreateCommand();
            command.CommandText = q.ToTraceString();

            foreach(var parm in q.Parameters)
            {
                DbParameter queryParm = this.CreateParameter( parm.Name, parm.Value);
                
                command.Parameters.Add(parm);
            }

            return command;
        }
        #endregion

        #region Query Operations

        /// <summary>
        /// Returns a DbDataReader from a SQL statement.
        /// 
        /// Note:
        /// Reader is created with CommandBehavior.CloseConnection
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="dbParameters"></param>
        /// <returns>DataReader or Null.</returns>
        public DbDataReader ExecuteReader(DbCommand sqlCommand, params DbParameter[] dbParameters)
        {
            foreach (DbParameter Parameter in dbParameters)
            {
                sqlCommand.Parameters.Add(Parameter);
            }
            
            this.OpenConnection();
            return  sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Returns a DbDataReader from a SQL statement.
        /// 
        /// Note:
        /// Reader is created with CommandBehavior.CloseConnection
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string sql, params DbParameter[] dbParameters)
        {
            return this.ExecuteReader(this.CreateCommand(sql), dbParameters);
        }

        /// <summary>
        /// Executes a LINQ to SQL query and returns the results as a DataReader.
        /// </summary>
        /// <param name="query">LINQ query object</param>        
        /// <returns></returns>
        public DbDataReader ExecuteReader(IQueryable query)
        {
            return this.ExecuteReader(this.GetCommand(query));
        }

        /// <summary>
        /// Executes a SQL command and retrieves a DataTable of the result
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="tableName"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(DbCommand sqlCommand, string tableName, params DbParameter[] dbParameters)
        {
            foreach (DbParameter Parameter in dbParameters)
            {
                sqlCommand.Parameters.Add(Parameter);
            }

            DbDataAdapter Adapter = DbProvider.CreateDataAdapter();
            Adapter.SelectCommand = sqlCommand;

            DataTable dt = new DataTable(tableName);
            
            try
            {
                this.OpenConnection();
                Adapter.Fill(dt);
            }
            finally
            {
                this.CloseConnection();
            }

            return dt;
        }
        /// <summary>
        /// Executes a SQL command from a string and retrieves a DataTable of the result
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="tableName"></param>
        /// <param name="dbParameters"></param>
        public DataTable ExecuteDataTable(string sql, string tableName, params DbParameter[] dbParameters)
        {
            return this.ExecuteDataTable(this.CreateCommand(sql),tableName,dbParameters);
        }

        /// <summary>
        /// Creates a DataTable from a Linq Query expression
        /// </summary>
        /// <param name="query">A LINQ to SQL query object</param>
        /// <param name="tableName">The resulting table name.</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(IQueryable query, string tableName)
        {
            return this.ExecuteDataTable(this.GetCommand(query),tableName);
        }

        /// <summary>
        /// Runs a query and returns a table in a DataSet either passed in or
        /// by creating a new one.
        /// </summary>
        /// <param name="sqlCommand">SQL Command object</param>
        /// <param name="dataSet">Dataset to add table to</param>
        /// <param name="tableName">Name of the result table</param>
        /// <param name="dbParameters">Optional SQL statement parameters</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(DbCommand sqlCommand, DataSet dataSet, string tableName, params DbParameter[] dbParameters)
        {
            if (dataSet == null)
                dataSet = new DataSet();

            DbDataAdapter Adapter = DbProvider.CreateDataAdapter();
            Adapter.SelectCommand = sqlCommand;

            foreach (DbParameter Parameter in dbParameters)
            {
                sqlCommand.Parameters.Add(Parameter);
            }

            DataTable dt = new DataTable(tableName);

            if (dataSet.Tables.Contains(tableName))
                dataSet.Tables.Remove(tableName);

            try
            {
                this.OpenConnection();
                Adapter.Fill(dataSet, tableName);
            }
            finally
            {
                this.CloseConnection();
            }

            return dataSet;
        }


        /// <summary>
        /// Runs a query and returns a table in a DataSet either passed in or
        /// by creating a new one.
        /// </summary>
        /// <param name="sqlCommand">SQL string to execute</param>
        /// <param name="dataSet">Dataset to add table to</param>
        /// <param name="tableName">Name of the result table</param>
        /// <param name="dbParameters">Optional SQL statement parameters</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql, DataSet dataSet, string tableName, params DbParameter[] dbParameters)
        {
            return this.ExecuteDataSet(this.CreateCommand(sql), dataSet, tableName, dbParameters);
        }
    
        /// <summary>
        /// Runs a query and returns a table in a DataSet either passed in or
        /// by creating a new one.
        /// </summary>
        /// <param name="sqlCommand">SQL string to execute</param>
        /// <param name="dataSet">Dataset to add table to</param>
        /// <param name="tableName">Name of the result table</param>
        /// <param name="dbParameters">Optional SQL statement parameters</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(IQueryable query, DataSet dataSet, string tableName)
        {
            return this.ExecuteDataSet(this.GetCommand(query), dataSet, tableName);
        }


        /// <summary>
        /// Executes a raw Sql Command against the server that doesn't return
        /// a result set.
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="dbParameters"></param>
        /// <returns>-1 on error, records affected otherwise</returns>
        public int ExecuteNonQuery(DbCommand sqlCommand, params DbParameter[] dbParameters)
        {
            int RecordCount = 0;

            foreach (DbParameter parameter in dbParameters)
            {
                sqlCommand.Parameters.Add(parameter);
            }
            
            try
            {
                this.OpenConnection();
                
                RecordCount = sqlCommand.ExecuteNonQuery();
                if (RecordCount == -1)
                    RecordCount = 0;
            }
            finally
            {
                this.CloseConnection();
            }

            return RecordCount;
        }

        /// <summary>
        /// Executes a raw Sql Command against the server that doesn't return
        /// a result set.
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="dbParameters"></param>
        /// <returns>-1 on error. Records affected otherwise</returns>
        public int ExecuteNonQuery(string sql, params DbParameter[] dbParameters)
        {            
            return this.ExecuteNonQuery(this.CreateCommand(sql), dbParameters);
        }

        /// <summary>
        /// Executes a SQL Query and returns a single result value that isn't
        /// part of a result cursor
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public virtual object ExecuteScalar(DbCommand command, params DbParameter[] dbParameters)
        {         
            object result = null;            
            result = command.ExecuteScalar();
            return result;
        }

        /// <summary>
        /// Executes a SQL QUery from a string and returns a single value
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, params DbParameter[] dbParameters)
        {
            return this.ExecuteScalar(this.CreateCommand(sql), dbParameters);
        }

#endregion        
    }

}
