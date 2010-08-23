using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Common;
using System.Data;

namespace Westwind.BusinessFramework.LinqToSql
{
    /// <summary>
    /// Customization of the LINQ to SQL DataContext class that provides
    /// core ADO.NET Data Access methods to the data context.
    /// </summary>
    public class DataContextSql : System.Data.Linq.DataContext
    {
        // ProviderFactory in case we need to generate DB object objects
        public static DbProviderFactory dbProvider = DbProviderFactories.GetFactory("System.Data.SqlClient");

        #region Constructors
        static DataContextSql()
        {
        }

        public DataContextSql(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource)
        {
        }

        public DataContextSql(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource)
        {            
        }

        // Static so we can handle default constructors
        //private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();

        //public wwDataContext(string connection) : 
        //        base(connection, mappingSource)
        //{			
        //}

        //public wwDataContext(System.Data.IDbConnection connection) : 
        //        base(connection, mappingSource)
        //{
        //}
        #endregion

        #region Base Operations (open/close connections, Parameters, Commands)

        /// <summary>
        /// Opens a connection to the database
        /// </summary>
        /// <returns></returns>
        public bool OpenConnection()
        {
            if (this.Transaction != null && this.Connection.State != ConnectionState.Open)
                this.Transaction.Connection.Open();

            if (this.Connection.State != System.Data.ConnectionState.Open)
                this.Connection.Open();
           
            return true;
        }

        /// <summary>
        /// Opens the connection on this data context
        /// </summary>
        /// <returns></returns>
        public void CloseConnection()
        {
            if (this.Transaction != null)
                return;  // leave connection open

            this.Connection.Close();
        }

        /// <summary>
        /// Starts a new transaction on this connection/instance.
        /// 
        /// NOTE: provided only for ADO.NET style transactions
        /// LINQ to SQL will create its own connection instances
        /// and will close open transactions on its own.
        /// </summary>
        /// <returns></returns>
        public bool BeginTransaction()
        {

            this.OpenConnection();
            this.Transaction = this.Connection.BeginTransaction();
            if (this.Transaction == null)
                return false;
            return true;
        }

        /// <summary>
        /// Commits all changes to the database and ends the transaction
        /// </summary>
        /// <returns></returns>
        public bool CommitTransaction()
        {
            if (this.Transaction == null)
                return false;

            this.Transaction.Commit();
            this.Transaction = null;
            this.CloseConnection();

            return true;
        }

        /// <summary>
        /// Rolls back a transaction
        /// </summary>
        /// <returns></returns>
        public bool RollbackTransaction()
        {
            if (this.Transaction == null)
                return true;

            this.Transaction.Rollback();
            this.Transaction = null;            
            this.CloseConnection();

            return true;
        }

        /// <summary>
        /// Creates a new SQL Command
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public DbCommand CreateCommand(string sql, params DbParameter[] dbParameters)
        {
            DbCommand sqlCommand = dbProvider.CreateCommand();
            sqlCommand.CommandText = sql;

            if (this.Transaction != null)
            {
                sqlCommand.Transaction = this.Transaction;
                sqlCommand.Connection = this.Transaction.Connection;
            }
            else
            {                
                sqlCommand.Connection = this.Connection;
            }
            
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
            DbParameter parm = dbProvider.CreateParameter();
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
            DbParameter parm = dbProvider.CreateParameter();
            parm.ParameterName = name;
            parm.Value = value;
            parm.Direction = direction;
            return parm;
        }

        /// <summary>
        /// Creates a new SQL Parameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="direction"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string name, object value, ParameterDirection direction, DbType type)
        {
            DbParameter parm = dbProvider.CreateParameter();
            parm.ParameterName = name;
            parm.Value = value;
            parm.Direction = direction;
            parm.DbType = type;
            return parm;
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

            DbDataAdapter Adapter = dbProvider.CreateDataAdapter();
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

            DbDataAdapter Adapter = dbProvider.CreateDataAdapter();
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
