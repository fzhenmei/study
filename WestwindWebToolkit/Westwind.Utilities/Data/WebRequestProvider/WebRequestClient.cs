using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace Westwind.Utilities.Data
{    
 
    /// <summary>    
    /// DbProvider implementation that uses the WebRequestDataAccess class to make remote server
    /// calls to return data. Requires a SqlWebServer server backend to handle the incoming 
    /// Web requests...
    /// 
    /// Define factory as:
    /// Westwind.Data.WebRequest
    /// <system.data>
    ///    <DbProviderFactories>
    ///   <add name="West Wind Web Request Provider" invariant="Westwind.Data.WebRequest" description="Web Request Data Provider" type="Westwind.Utilities.WebRequestClientFactory,SqlWebService" ></add>
    /// </DbProviderFactories>
    /// </system.data>
    /// </summary>
    public class WebRequestClientFactory : DbProviderFactory
    {
        public static readonly WebRequestClientFactory Instance;
 
        private WebRequestClientFactory()
        {
            
        }

        static WebRequestClientFactory()
        {
            Instance = new WebRequestClientFactory();
        }

        public override DbCommand CreateCommand()
        {
            return new WebRequestCommand();
        }

        public override DbCommandBuilder CreateCommandBuilder()
        {
            return new WebRequestCommandBuilder();
        }


        public override DbConnection CreateConnection()
        {
            return new WebRequestConnection();
        }

        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            throw new NotImplementedException("ConnectionStringBuilder is not supported in WebRequestClient");
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            return new WebRequestDataAdapter();
        }

        public override DbDataSourceEnumerator CreateDataSourceEnumerator()
        {
            return SqlClientFactory.Instance.CreateDataSourceEnumerator();
        }

        public override DbParameter CreateParameter()
        {
            return SqlClientFactory.Instance.CreateParameter();
        }

        public override System.Security.CodeAccessPermission CreatePermission(System.Security.Permissions.PermissionState state)
        {
            return base.CreatePermission(state);
        }        
    }

    public class WebRequestConnection : DbConnection
    {
        public override void Open()
        {
            // do nothing
            _state = ConnectionState.Open;
        }
        public override void Close()
        {
            // do nothing
            _state = ConnectionState.Closed;
        }

        protected override DbTransaction BeginDbTransaction(System.Data.IsolationLevel isolationLevel)
        {

            return new WebRequestTransaction(this); 

            //throw new NotImplementedException("Transactions are not supported on WebRequestClient");
            //return BeginTransaction();
        }
        

        public override void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public override string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
            set
            {
                _ConnectionString = value;
            }
        }
        string _ConnectionString = string.Empty;

        protected override DbCommand CreateDbCommand()
        {
            return new WebRequestCommand(null,this);
        }

        public override string DataSource
        {
            get { throw new NotImplementedException(); }
        }

        public override string Database
        {
            get { throw new NotImplementedException(); }
        }

        public override string ServerVersion
        {
            get { return "1.0"; }
        }

        public override System.Data.ConnectionState State
        {
            get { return _state; }
        }
        ConnectionState _state = ConnectionState.Closed;
    }



    public class WebRequestCommand : DbCommand
    {
        /// <summary>
        /// Proxy through this sqlCommand
        /// </summary>
        //SqlCommand sqlCommand = new SqlCommand();


        public new WebRequestParameterCollection Parameters
        {
            get
            {
                if (_Parameters == null)
                    _Parameters = new WebRequestParameterCollection();

                return _Parameters;
            }
        }
        WebRequestParameterCollection _Parameters = null;

        public WebRequestCommand() : base()
        {
        }

        public WebRequestCommand(string commandText, DbConnection connection) : base()
        {
            DbConnection = connection;
            CommandText = commandText;            
        }

        public WebRequestCommand(string commandText, DbTransaction transaction) : base()
        {
            Transaction = transaction;
            CommandText = commandText;
        }


        private WebRequestDataAccess  CreateWebDataAccess()
        {
            WebRequestDataAccess access = new WebRequestDataAccess();
            access.Timeout = _CommandTimeout;

            if (Connection != null)
                access.ConnectionString = Connection.ConnectionString;

            return access;
        }


        public override int ExecuteNonQuery()
        {
            WebRequestDataAccess access = CreateWebDataAccess();
            int res = access.ExecuteNonQuery(this);

            if (res == -1)
               throw new InvalidOperationException(access.ErrorMessage);

            return res;
        }

        protected override DbDataReader ExecuteDbDataReader(System.Data.CommandBehavior behavior)
        {
            WebRequestDataAccess access = CreateWebDataAccess();
            DbDataReader reader = access.ExecuteReader(this);
            if (reader == null)
                throw new InvalidOperationException(access.ErrorMessage);
            return reader;
        }

        public override object ExecuteScalar()
        {
            WebRequestDataAccess access = CreateWebDataAccess();
            object res = access.ExecuteScalar(this);

            if (res == null && !string.IsNullOrEmpty(access.ErrorMessage))
                throw new InvalidOperationException(access.ErrorMessage);

            return res;
        }

        public override void Cancel()
        {                        
            Connection.Close();
        }

        
        public override string CommandText
        {
            get
            {
                return _CommandText;
            }
            set
            {
                _CommandText = value;
            }
        }
        string _CommandText = string.Empty;


        public override int CommandTimeout
        {
            get
            {
                return _CommandTimeout;
            }
            set
            {
                _CommandTimeout = value;
            }
        }
        int _CommandTimeout = 15;

        public override System.Data.CommandType CommandType
        {
            get
            {
                return _CommandType;
            }
            set
            {
                _CommandType = value;
            }
        }
        CommandType _CommandType = CommandType.Text;

        protected override DbParameter CreateDbParameter()
        {
            return SqlClientFactory.Instance.CreateParameter();
        }

        protected override DbConnection DbConnection
        {
            get
            {
                return _Connection;
            }
            set
            {
                _Connection = value;
            }
        }
        DbConnection _Connection = null;

        protected override DbParameterCollection DbParameterCollection
        {
            get { return Parameters; }
        }

        protected override DbTransaction DbTransaction
        {
            get
            {
                return  _DbTransaction;
            }
            set
            {
                _DbTransaction = value;
            }
        }
        DbTransaction _DbTransaction = null;

        public override bool DesignTimeVisible
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }

        public override void Prepare()
        {
            throw new NotImplementedException();
        }

        public override System.Data.UpdateRowSource UpdatedRowSource
        {
            get
            {               
                return _UpdatedRowSource;
            }
            set
            {
                _UpdatedRowSource = value;
            }
        }
        UpdateRowSource _UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;
    }

    public class WebRequestDataAdapter : DbDataAdapter
    {

        protected override int Update(DataRow[] dataRows, DataTableMapping tableMapping)
        {
            throw new NotSupportedException("WebRequestClient DataAdapter Updates are not supported");            
        }
        public override int Update(DataSet dataSet)
        {
            throw new NotSupportedException("WebRequestClient DataAdapter Updates are not supported");            
        }        
    }

    public class WebRequestCommandBuilder : DbCommandBuilder
    {

        protected override void ApplyParameterInfo(DbParameter parameter, DataRow datarow, StatementType statementType, bool whereClause)
        {
            SqlParameter parameter2 = parameter as SqlParameter;
            object obj3 = datarow[SchemaTableColumn.ProviderType];
            parameter2.SqlDbType = (SqlDbType)obj3;
            parameter2.Offset = 0;
            if ((parameter2.SqlDbType == SqlDbType.Udt) && !parameter2.SourceColumnNullMapping)
            {
                parameter2.UdtTypeName = datarow["DataTypeName"] as string;
            }
            else
            {
                parameter2.UdtTypeName = string.Empty;
            }
            object obj2 = datarow[SchemaTableColumn.NumericPrecision];
            if (DBNull.Value != obj2)
            {
                byte num2 = (byte)((short)obj2);
                parameter2.Precision = (0xff != num2) ? num2 : ((byte)0);
            }
            obj2 = datarow[SchemaTableColumn.NumericScale];
            if (DBNull.Value != obj2)
            {
                byte num = (byte)((short)obj2);
                parameter2.Scale = (0xff != num) ? num : ((byte)0);
            }

            
        }

        protected override string GetParameterName(string parameterName)
        {
            return "@" + parameterName;
        }

        protected override string GetParameterName(int parameterOrdinal)
        {

            return ("@p" + parameterOrdinal.ToString(CultureInfo.InvariantCulture));
        }

        protected override string GetParameterPlaceholder(int parameterOrdinal)
        {
         return ("@p" + parameterOrdinal.ToString(CultureInfo.InvariantCulture));
        }

        protected override void SetRowUpdatingHandler(DbDataAdapter adapter)
        {
        }
    }

    public class WebRequestParameterCollection : DbParameterCollection
    {
        List<DbParameter> items = new List<DbParameter>();

        public override int Add(object value)
        {
            items.Add(value as DbParameter);
            return 1;
        }

        public override void AddRange(Array values)
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            items.Clear();
            
        }

        public override bool Contains(string value)
        {
            DbParameter parm = items.Where(p => p.ParameterName == value).FirstOrDefault();
            if (parm == null)
                return false;

            return true;
        }

        public override bool Contains(object value)
        {
            return items.Contains(value as DbParameter);
        }

        public override void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override System.Collections.IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            DbParameter parm = items.Where(p => p.ParameterName == parameterName).FirstOrDefault();

            throw new NotImplementedException();
        }

        protected override DbParameter GetParameter(int index)
        {
            throw new NotImplementedException();
        }

        public override int IndexOf(string parameterName)
        {
            throw new NotImplementedException();
        }

        public override int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public override void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public override bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public override void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAt(string parameterName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            throw new NotImplementedException();
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            throw new NotImplementedException();
        }

        public override object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }
    }

    public class WebRequestTransaction : DbTransaction
    {
        public WebRequestTransaction() : base()
        {
        }

        public WebRequestTransaction(DbConnection connection) : base()
        {
            _DbConnection = connection;
        }

        public override void Commit()
        {
            // do nothing   
        }

        protected override DbConnection DbConnection
        {
            get { return _DbConnection; }
        }
        private DbConnection _DbConnection = null;

                                                  

        public override IsolationLevel IsolationLevel
        {
            get { return IsolationLevel; }
        }

        public override void Rollback()
        {            
            // do nothing
        }
    }
}
