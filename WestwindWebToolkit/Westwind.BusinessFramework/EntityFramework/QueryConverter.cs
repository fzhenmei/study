using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace Westwind.BusinessFramework.EntityFramework
{
    /// <summary>
    /// This class acts like a wrapped LINQ to SQL Query converter
    /// that provides error handling. Each of the calls is wrapped
    /// and sets error messages on the hosting business object.
    /// 
    /// Check busObject.ErrorMessage or busObject.ErrorException
    /// for errors if the result from the conversion method call
    /// is null.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public class QueryConverter<TEntity, TContext>
        where TEntity : class, new()
        where TContext : ObjectContextSql, new()
    {
        protected BusinessObjectEF<TEntity, TContext> Business = null;

        protected ObjectContextSql Context = null;


        /// <summary>
        /// Constructor initializes the converter with an instance
        /// of the full business object so that error messages/exceptions
        /// can be fired back into the business object.
        /// </summary>
        /// <param name="business">instance of a BusinessObjectLinq</param>
        public QueryConverter(BusinessObjectEF<TEntity, TContext> business, ObjectContextSql context)
        {
            this.Business = business;
            this.Context = context;                
        }

        /// <summary>
        /// Returns a DataReader from a Linq to Sql query
        /// 
        /// DataReader is created with CommandBehavior.CloseConnection
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbDataReader ToDataReader(object query)
        {
            if (query == null)
            {
                this.Business.SetError("No query data to create list.");
                return null;
            }
            
            try
            {
                IDbCommand command = this.Context.GetCommand(query as IQueryable);
                command.Connection = this.Context.Connection;
                command.Connection.Open();
                DbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection) as DbDataReader;
                return reader;
            }
            catch (Exception ex)
            {
                this.Business.SetError(ex);
            }

            return null;
        }

        /// <summary>
        /// Creates a DataTable from a query.
        /// </summary>
        /// <param name="query">The Linq to Sql query</param>
        /// <param name="tableName">The name of the table to create</param>
        /// <param name="ExecuteWithSchema">determines whether extra schema information is retrieved with the query</param>
        /// <returns></returns>
        public DataTable ToDataTable(object query, string tableName, bool ExecuteWithSchema)
        {
            if (query == null)
            {
                this.Business.SetError("No query data to create list.");
                return null;
            }

            DbCommand command = this.Context.GetCommand(query as IQueryable);

            DbDataAdapter Adapter = BusinessObjectEF<TEntity,TContext>.ProviderFactory.CreateDataAdapter();
            Adapter.SelectCommand = command;
            
            DataTable dt = new DataTable(tableName);

            try
            {
                command.Connection.Open();
                Adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                this.Business.SetError(ex);
                return null;
            }
            finally
            {
                command.Connection.Close();
            }

            return dt;
        }

        /// <summary>
        /// Creates a DataTable from a query.
        /// </summary>
        /// <param name="query">The Linq to Sql query</param>
        /// <param name="tableName">The name of the table to create</param>
        /// <param name="ExecuteWithSchema">determines whether extra schema information is retrieved with the query</param>
        /// <returns></returns>
        public DataTable ToDataTable(object query, string tableName)
        {
            return this.ToDataTable(query, tableName, false);
        }

        /// <summary>
        /// Converts a query to a Table in a DataSet. You can pass in the
        /// dataset if you choose.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataSet ToDataSet(object query, DataSet dataSet, string tableName)
        {
            if (dataSet == null)
                dataSet = new DataSet();

            DataTable table =  this.ToDataTable(query, tableName);
            if (table == null)
                return null;

            if (dataSet.Tables.Contains(tableName))
                dataSet.Tables.Remove(tableName);

            dataSet.Tables.Add(table);

            return dataSet;
        }

        /// <summary>
        /// Converts a query to a Table in a DataSet. You can pass in the
        /// dataset if you choose.
        /// </summary>        
        public DataSet ToDataSet(object query, string tableName)
        {
            return this.ToDataSet(query, null, tableName);
        }

        /// <summary>
        /// Returns a generic List&gt;&lt; of entities
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<TEntity> ToList(object query)
        {
            IEnumerable<TEntity> res = query as IEnumerable<TEntity>;
            if (res == null)
            {
                this.Business.SetError("No query data to create list.");
                return null;
            }

            try
            {
                return res.ToList();
            }
            catch (Exception ex)
            {
                this.Business.SetError(ex);
            }

            return null;
        }

        /// <summary>
        /// Returns a generic array of entities
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public TEntity[] ToArray(object query)
        {
            IEnumerable<TEntity> res = query as IEnumerable<TEntity>;

            try
            {
                return res.ToArray();
            }
            catch (Exception ex)
            {
                this.Business.SetError(ex);
            }

            return null;
        }

    }

}
