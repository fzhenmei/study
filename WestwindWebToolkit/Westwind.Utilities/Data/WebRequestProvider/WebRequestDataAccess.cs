// #define subsonic
#if true

using System;
using System.Linq;
using System.Data;
using System.Data.Common;
using Westwind.InternetTools;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;

namespace Westwind.Utilities.Data
{
    public class WebRequestDataAccess : SqlDataAccessBase
    {
        /// <summary>
        /// The connection string to the remote data. 
        /// 
        /// Passed as a URL:
        /// "Data Source=https://rasnote/WestWindWebToolkitWeb/WebRequestDataHandler/DataServiceHandler.ashx;uid=johnd;pwd=supersecret;Timeout=20" 
        /// 
        /// Uid, Pwd = Http Access Password Security if required
        /// Timeout = Connection Timeout
        /// </summary>
        public override string ConnectionString
        {
            get { return base.ConnectionString;  }
            set 
            {
                base.ConnectionString = value;
                ParseConnectionString();
            }
        }
        
        /// <summary>
        /// The server url
        /// </summary>
        public string ServerUrl
        {
            get { return _ServerUrl; }
            set { _ServerUrl = value; }
        }
        private string _ServerUrl = string.Empty;

        /// <summary>
        /// The timeout of requests in seconds
        /// </summary>
        public int Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }
        private int _Timeout = 15;

                        
        /// <summary>
        /// Server access username or password. 
        /// This would be the HTTP password
        /// </summary>
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }
        private string _Username = string.Empty;

        /// <summary>
        /// Server access username or password. 
        /// This would be the HTTP password
        /// </summary>
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        private string _Password = string.Empty;


        /// <summary>
        /// An internal instance of a command object
        /// </summary>
        public DbCommand Command
        {
            get { return _Command; }
            set { _Command = value; }
        }
        private DbCommand _Command = null;


        /// <summary>
        /// The name of the cursor that gets generated in the dataset
        /// 
        /// List can be comma delimited if more than one cursor is created
        /// </summary>
        protected string CursorName
        {
            get { return _CursorName; }
            set { _CursorName = value; }
        }
        private string _CursorName = "Cursor";


        /// <summary>
        /// When calling ExecuteScalar returns a scalar result value
        /// </summary>
        public object ScalarResult
        {
            get { return _ScalarResult; }
            set { _ScalarResult = value; }
        }
        private object _ScalarResult = null;

        
        /// <summary>
        /// If calling ExecuteNonQuery returns a count of affected records
        /// </summary>
        public int AffectedRecords
        {
            get { return _AffectedRecords; }
            set { _AffectedRecords = value; }
        }
        private int _AffectedRecords = 0;


        public WebRequestDataAccess()
        {
            // We always want the server to include schema info
            // so we can get properly typed results
            ExecuteWithSchema = true;
        }

        #region Http And Parsing Code

        public void ParseConnectionString()
        {            
            if (string.IsNullOrEmpty(ConnectionString))
                return;
            
            ServerUrl = StringUtils.ExtractString(ConnectionString, "Data Source=", ";", false, true);
            if (string.IsNullOrEmpty(ServerUrl))
            {                
                ServerUrl = ConnectionString;
                if (!ServerUrl.ToLower().StartsWith("http://"))
                    throw new ArgumentException("Invalid or missing Data Source provided");                
            }

            Username = StringUtils.ExtractString(ConnectionString, "uid=", ";", true, true);
            Password = StringUtils.ExtractString(ConnectionString, "pwd=", ";", true, true);
            int val = StringUtils.ParseInt(StringUtils.ExtractString(ConnectionString, 
                                                                     "timeout=", ";",
                                                                     true,true),0);
            if (val > 0)
                Timeout = val;
        }


        public XElement ParseCommandToXml(DbCommand command, SqlExecutionModes executeMode)
        {
            Command = command;

            XElement doc = XElement.Parse("<Query/>");
            doc.Add(new XElement("Command", command.CommandText),
                    new XElement("CommandType", command.CommandType == CommandType.StoredProcedure ? 1 : 0),
                    new XElement("ExecuteMode", (int) executeMode),
                    new XElement("IncludeSchema", ExecuteWithSchema ? 1 : 0),
                    new XElement("Cursor",CursorName)
            );

            XElement parms = XElement.Parse("<Parameters />");
            doc.Add(parms);

            foreach (DbParameter parameter in command.Parameters)
            {
                if (parameter.DbType == DbType.Binary)
                    parameter.Value = Convert.ToBase64String(parameter.Value as byte[]);

                XElement parm = new XElement("Parameter",parameter.Value);
                
                parm.Add(new XAttribute("Name", parameter.ParameterName));                
                parm.Add( new XAttribute("DbType", parameter.DbType.ToString()));
                parm.Add( new XAttribute("Size", parameter.Size) );
                parm.Add(new XAttribute("Direction", (int) parameter.Direction));

                parms.Add(parm);
            }

            return doc;
        }

#if subsonic
        public XElement ParseCommandToXml(QueryCommand command, SqlExecutionModes executeMode)
        {
            XElement doc = XElement.Parse("<Query/>");
            doc.Add(new XElement("Command", command.CommandSql),
                    new XElement("CommandType", command.CommandType == CommandType.StoredProcedure ? 1 : 0),
                    new XElement("ExecuteMode", (int)executeMode),
                    new XElement("IncludeSchema", IncludeSchema)
            );

            XElement parms = XElement.Parse("<Parameters />");
            doc.Add(parms);

            foreach (QueryParameter parameter in command.Parameters)
            {
                XElement parm = new XElement("Parameter",parameter.ParameterValue);
                parm.Add(new XAttribute("Name", parameter.ParameterName));
                parm.Add(new XAttribute("Direction", parameter.Mode.ToString()));
                parm.Add(new XAttribute("SqlType", parameter.DataType.ToString()));
                parm.Add(new XAttribute("Size", parameter.Size));
                parm.Add(new XAttribute("Scale", parameter.Scale));
                parm.Add(new XAttribute("Precision", parameter.Precision));
                
                parms.Add(parm);
            }

            return doc;
        }
#endif

        /// <summary>
        /// Calls the server with an XML request        
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public  XElement  CallServer(XElement doc)
        {
            HttpClient http = new HttpClient();
            http.CreateWebRequestObject(ServerUrl);
            http.ContentType = "text/xml";            
            http.PostMode = HttpPostMode.Xml;
            http.AddPostKey(doc.ToString());

            http.WebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");

            if (!string.IsNullOrEmpty(Username))
            {
                http.Username = Username;
                http.Password = Password;
            }

            string xmlResult = http.GetUrl(ServerUrl);
            

            if (string.IsNullOrEmpty(xmlResult) || http.Error)
                throw new InvalidOperationException(http.ErrorMessage);
            
            try
            {
                XElement response =  XElement.Parse(xmlResult);
                if (!ParseResponse(response))
                    return null;

                return response;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to parse Xml Response from server",ex);
            }
        }


        /// <summary>
        /// Processes the XML from the response and checks for errors
        /// If an error is found error properties are set and method
        /// returns false.
        /// 
        /// Otherwise properties of hte class are set (AffectedRecords, ScalarResult etc.)
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public bool ParseResponse(XElement doc)
        {
            XElement message = doc.Element("Message");
            if (message != null)
            {
                XElement errorNode = message.Element("ErrorCode");
                if (errorNode != null && (int)errorNode > 0)
                {
                    SetError((string)message.Element("ErrorMessage"));
                    return false;                    
                }
                
                XElement affectedRecords = message.Element("AffectedRecords");
                if (affectedRecords != null)
                    AffectedRecords = (int)affectedRecords;
            }

            XElement work = doc.Element("ScalarResult");
            if (work != null)
            {
                string strType = work.Attribute("Type").Value;
                Type type = ReflectionUtils.GetTypeFromName(strType);
                if (type == null)
                {
                    SetError("Couldn't parse scalar result returned from server.");
                    return false;
                }

                ScalarResult = ReflectionUtils.StringToTypedValue(work.Value, type);
            }

            work = doc.Element("AffectedRecords");
            if (work != null)
                AffectedRecords = (int)work;

            work = doc.Element("Parameters");
            if (work != null )
            {
                foreach(DbParameter parameter in Command.Parameters)
                {
                    if (parameter.Direction != ParameterDirection.Input)
                    {
                        XElement parmNode = work.Elements("Parameter").Where(el => el.Attribute("Name").Value == parameter.ParameterName).FirstOrDefault();
                        if (parmNode == null)
                            continue;

                        Type parmType = DataUtils.DbTypeToDotNetType(parameter.DbType);
                        parameter.Value = ReflectionUtils.StringToTypedValue(parmNode.Value, parmType);
                    }
                }
            }
            
            return true;
        }


        /// <summary>
        /// Processes a DbCommand, creates XML and sends it to the server.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="sqlExecutionMode"></param>
        /// <returns></returns>
        public XElement ProcessCommand(DbCommand command,SqlExecutionModes sqlExecutionMode)
        {
            XElement xml = ParseCommandToXml(command, sqlExecutionMode);
            if (xml == null)
            {
                ErrorMessage="Invalid XML passed";
                return null;
            }

            XElement response = CallServer(xml);            
            if (response == null)
                return null;

            return response;
        }

        #endregion

        #region SqlDataAccessBase Implementation



        /// <summary>
        /// Executes a SQL command against the server and returns a DbDataReader
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override DbDataReader ExecuteReader(DbCommand command,params DbParameter[] parameters) 
        {
            SetError();

            DataSet ds = ExecuteDataSet("Cursor",command, parameters);
            if (ds == null || ds.Tables.Count < 0)
                return null;
            
            DataTableReader reader = null;
            if (ds.Tables.Count > 1)
            {
                List<DataTable> tables = new List<DataTable>();
                foreach (DataTable table in ds.Tables)
                {
                    tables.Add(table);
                }
                reader = new DataTableReader(tables.ToArray());
            }
            else
            {
                reader = new DataTableReader(ds.Tables[0]);
            }
            return reader;
        }

        /// <summary>
        /// Executes a SQL command against the server and returns a DataSet of the result
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override DataSet ExecuteDataSet(string tablename, DbCommand command, params DbParameter[] parameters)
        {
            SetError();

            if (parameters != null)
            {
                foreach (DbParameter parm in parameters)
                {
                    command.Parameters.Add(parm);
                }
            }

            CursorName = tablename;
            XElement response = ProcessCommand(command, SqlExecutionModes.Execute);
            if (response == null)
                return null;

            DataSet ds = new DataSet();

            if (ds == null || ds.Tables.Count < 0)
            {
                ErrorMessage = "No data returned.";
                return null;
            }
            ds.ReadXml(response.CreateReader());

            return ds;
        }


        /// <summary>
        /// Executes a Sql command and retrieves the result as a table in an existing dataset
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="tablename"></param>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override DataSet ExecuteDataSet(DataSet dataSet, string tablename, DbCommand command, params DbParameter[] parameters)
        {
            SetError();
            DataSet newDs = ExecuteDataSet(tablename, command, parameters);
            if (newDs == null || newDs.Tables.Count < 0)
                return null;


            DataTable table = newDs.Tables["Table"];
            newDs.Tables.Remove(table);

            if (dataSet.Tables.Contains(tablename))
                dataSet.Tables.Remove(tablename);

            dataSet.Tables.Add(table);

            return dataSet;
        }

        /// <summary>
        /// Returns a DataTable from a Sql Command string passed in.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override DataTable ExecuteTable(string tableName, DbCommand command, params DbParameter[] parameters)
        {
            SetError();

            DataSet ds = ExecuteDataSet(tableName,command,parameters);
            if (ds == null)
                return null;

            return ds.Tables[0];
        }

        /// <summary>
        /// Executes a command and returns a scalar value from it
        /// </summary>
        /// <param name="command">A SQL Command object</param>        
        /// <returns>value or null on failure</returns>        
        public override object ExecuteScalar(DbCommand command, params DbParameter[] parameters)
        {
            SetError();

            if (parameters != null)
            {
                foreach (DbParameter parm in parameters)
                {
                    command.Parameters.Add(parm);
                }
            }

            XElement response = ProcessCommand(command, SqlExecutionModes.ExecuteScalar);
            if (response == null)
                return null;

            XElement scalar = response.Descendants("ScalarResult").FirstOrDefault() ;
            if (scalar == null)
            {
                SetError("No scalar result returned.");
                return scalar;
            }

            string strType = scalar.Attribute("Type").Value;
            Type type = ReflectionUtils.GetTypeFromName(strType);
            if (type == null)
            {
                SetError("Couldn't parse scalar result");
                return null;
            }

            return ReflectionUtils.StringToTypedValue(scalar.Value, type);
        }


        /// <summary>
        /// Executes a command that doesn't return a data result. You can return
        /// output parameters and you do receive an AffectedRecords counter.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override int ExecuteNonQuery(DbCommand command, params DbParameter[] parameters)
        {
            SetError();

            if (parameters != null)
            {
                foreach (DbParameter parm in parameters)
                    command.Parameters.Add(parm);            
            }

            XElement response = ProcessCommand(command, SqlExecutionModes.ExecuteScalar);
            if (response == null)
                return -1;

            return AffectedRecords;
        }



        /// <summary>
        /// Used to create named parameters to pass to commands or the various
        /// methods of this class.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public override DbParameter CreateParameter(string parameterName, object value)
        {
            DbParameter parm = new SqlParameter();
            parm.ParameterName = parameterName;
            parm.Value = value;
            return parm;
        }


        /// <summary>
        /// Creates a Command object and opens a connection
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="sql"></param>
        /// <param name="commandType">type of command to createCommand</param>
        /// <returns></returns>
        public override DbCommand CreateCommand(string sql, CommandType commandType, params DbParameter[] parameters)
        {
            SetError();

            DbCommand command = new SqlCommand(sql);
            command.CommandType = commandType;
            if (parameters != null)
            {
                foreach (DbParameter Parm in parameters)
                {
                    command.Parameters.Add(Parm);
                }
            }
            return command;
        }
        /// <summary>
        /// Creates a Command object and opens a connection
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public override DbCommand CreateCommand(string sql, params DbParameter[] parameters)
        {
            return CreateCommand(sql, CommandType.Text, parameters);
        }

        public override bool BeginTransaction()
        {
            throw new NotSupportedException("Transactions are not supported in WebRequestDataAccess");
        }
        public override bool RollbackTransaction()
        {
            throw new NotSupportedException("Transactions are not supported in WebRequestDataAccess");
        }
        public override bool CommitTransaction()
        {
            throw new NotSupportedException("Transactions are not supported in WebRequestDataAccess");
        }
        public override void CloseConnection(DbCommand command)
        {
            // do nothing            
        }
        public override void CloseConnection()
        {
            // do nothing            
        }
        public override bool OpenConnection()
        {
            // do nothing
            return true;
        }

        #endregion

    }

    public enum SqlExecutionModes
    {
        Execute=0,
        ExecuteNonQuery=1,
        ExecuteScalar=2
    }
}
#endif