using System;
using System.Data;
using System.Data.SqlClient;

using System.Text;
using System.Web;


namespace Westwind.Utilities
{
	/// <summary>
	/// This class provides the ability to log requests into a SQL Server table.
	/// It's best called of the Application_BeginRequest/EndRequest event in global.asax.
	/// It can optionally log a request duration.
	/// </summary>
	public class WebRequestLog
	{

		public static string Tablename = "WebRequestLog";

		/// <summary>
		/// Parses the Exception into properties of this object. Called internally
		/// by LogError, but can also be called externally to get easier information
		/// about the error through the property interface.
		/// <seealso>Class WebRequestLog</seealso>
		/// </summary>
		/// <param name="ConnectionString"></param>
		/// <param name="CompactFormat"></param>
		/// <param name="StartTime">
		/// If you want to log a duration pass in a time the request was started or 
		/// pass DateTime.MinValue
		/// </param>
		/// <returns>bool</returns>
		public static bool Log(string ConnectionString,bool CompactFormat,DateTime StartTime) 
		{
			/// We'll build our SQL parameters dynamically in this code
			string InsertSql = "insert into " + Tablename +" (url,querystring,ip,post,browser,details,duration) values (@url,@querystring,@ip,@post,@browser,@details,@duration)";			
			SqlCommand Command = GetSqlCommand(ConnectionString,InsertSql,false);
			if (Command == null)
				return false;

			SqlParameterCollection parms = Command.Parameters;

			// Use String builder for Details
			StringBuilder sb =new StringBuilder(1024);

			// Simplify access to Request object
			HttpRequest Request = HttpContext.Current.Request;

			parms.AddWithValue("@url", Request.FilePath );
            parms.AddWithValue("@queryString",  Request.QueryString.ToString() );
            parms.AddWithValue("@ip", Request.UserHostAddress);
            parms.AddWithValue("@browser", Request.UserAgent );

			if (CompactFormat) 
			{
				parms.AddWithValue("@post","");
                parms.AddWithValue("@details", "");
			}
			else  
			{
				if (Request.TotalBytes > 0 && Request.TotalBytes < 2048)  
				{
                    parms.AddWithValue("@post", Encoding.GetEncoding(1252).GetString(Request.BinaryRead(Request.TotalBytes)));
					//ContentSize = Request.TotalBytes;
				}
				else if (Request.TotalBytes > 2048)  // strip the result
				{
                    parms.AddWithValue("@post", Encoding.GetEncoding(1252).GetString(Request.BinaryRead(2048)) + "...");
					//ContentSize = Request.TotalBytes;
				}
				else
                    parms.AddWithValue("@post", "");
                

				if (Request.UrlReferrer != null)
					sb.Append("Referrer: " + Request.UrlReferrer.ToString()+"\r\n");
				if (Request.IsAuthenticated)
					sb.Append("Login: " +HttpContext.Current.User.Identity.Name + "\r\n");

                parms.AddWithValue("@details", sb.ToString() );
			}
			
			SqlParameter parm  = parms.Add("@duration",SqlDbType.Float);

			if (StartTime == DateTime.MinValue)
				parm.Value = 0;
			else 
			{
				TimeSpan Span = DateTime.Now.Subtract( StartTime );
				parm.Value = Span.TotalMilliseconds;
			}

            try
            {
                Command.Connection.Open();
                int Result = Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string Message = ex.Message;
                return false;
            }
            finally
            {
               CloseConnection(Command);
            }

			return true;
		}

		/// <summary>
		/// Logs a custom message. Use for logging errors application messages and anything else you need to customize
		/// </summary>
		/// <param name="ConnectionString"></param>
		/// <param name="StartTime">If you want to log a duration pass in a time the request was started or pass DateTime.MinValue</param>
		public static bool LogCustomMessage(string ConnectionString,WebRequestLogMessageTypes Type,string CustomMessage) 
		{
			/// We'll build our SQL parameters dynamically in this code
			string InsertCommandText = "insert into " + Tablename +" (url,type,custommessage) values (@url,@type,@custommessage)";			
			SqlCommand Command = GetSqlCommand(ConnectionString,InsertCommandText,false);
			if (Command == null)
				return false;

			SqlParameterCollection parms = Command.Parameters;

			// Simplify access to Request object
            parms.AddWithValue("@type", Type);
            parms.AddWithValue("@custommessage", CustomMessage );
			
			string Url = "";
			if (Type == WebRequestLogMessageTypes.Normal)
				Url = "";
			else if (Type ==  WebRequestLogMessageTypes.Error) 
				Url  = "Error";
			else if (Type ==  WebRequestLogMessageTypes.ApplicationMessage) 
				Url = "Application Message";

            parms.AddWithValue("@url", Url);

            try
            {
                Command.Connection.Open();
                int Result = Command.ExecuteNonQuery();
            }
            catch(Exception ex)
            { string m = ex.Message; }
            finally
            {
                CloseConnection(Command);
            }

			return true;
		}
        
		/// <summary>
		/// Returns the entire log table. All Types, all dates...
		/// </summary>
		/// <param name="ConnectionString"></param>
		/// <returns></returns>
		public static DataTable RetrieveLogTable(string ConnectionString)
		{
			return RetrieveLogTable(ConnectionString,"select * from " + Tablename + " (NOLOCK) where type=0");
		}

		/// <summary>
		/// Returns the last number of requests from the log
		/// </summary>
		/// <param name="ConnectionString"></param>
		/// <param name="LastRecordCountToReturn"></param>
		/// <returns></returns>
		public static DataTable RetrieveLogTable(string ConnectionString,int MaxRecordsToReturn) 
		{
			return RetrieveLogTable(ConnectionString,"select TOP " + MaxRecordsToReturn.ToString() + " * from " + Tablename + " (NOLOCK) where type=0");
		}

		/// <summary>
		/// Returns the log since a given date.
		/// </summary>
		/// <param name="ConnectionString"></param>
		/// <param name="RetrievalStartDate"></param>
		/// <returns></returns>
		public static DataTable RetrieveLogTable(string ConnectionString, DateTime RetrievalStartDate) 
		{
			return RetrieveLogTable(ConnectionString,"select * from " + Tablename + " (NOLOCK) where time >  '" + RetrievalStartDate.ToString() + "' and type=0");
		}
	
		/// <summary>
		/// Internal worker method that retrieves the 
		/// </summary>
		/// <param name="StartDate"></param>
		/// <returns></returns>
		protected static DataTable RetrieveLogTable(string ConnectionString, string Sql) 
		{
			SqlCommand Command = null;
			DataTable Table = new DataTable();

			try 
			{
				Command = GetSqlCommand(ConnectionString,Sql + " Order by Time DESC");
				SqlDataAdapter Adapter = new SqlDataAdapter(Command);
				Adapter.Fill(Table);
			}
			catch
			{
				Table = null;
			}

			CloseConnection(Command);

			return Table;
		}


		/// <summary>
		/// Returns all Errors in the file
		/// </summary>
		/// <param name="ConnectionString"></param>
		/// <returns></returns>
		public static DataTable RetrieveErrors(string ConnectionString) 
		{
			return RetrieveLogTable(ConnectionString,"select * from " + Tablename + " (NOLOCK) where type=1");
		}

		/// <summary>
		/// Returns all Application Messages
		/// </summary>
		/// <param name="ConnectionString"></param>
		/// <returns></returns>
		public static DataTable RetrieveApplicationMessages(string ConnectionString) 
		{
			return RetrieveLogTable(ConnectionString,"select * from " + Tablename + " (NOLOCK) where type=2");
		}

		/// <summary>
		/// Clear the log completely.
		/// </summary>
		/// <param name="ConnectionString"></param>
		public static bool ClearLog(string ConnectionString) 
		{
			return ClearLog(ConnectionString,null);
		}

		/// <summary>
		/// Clear the log up until a specific date.
		/// </summary>
		/// <param name="ConnectionString"></param>
		/// <param name="ClearUntilTime"></param>
		public static bool ClearLog(string ConnectionString,DateTime ClearUntilTime) 
		{
			return ClearLog(ConnectionString," where time < '" + ClearUntilTime.ToString() + "'");
		}

		/// <summary>
		/// Clears the log with a filter condition.
		/// </summary>
		/// <returns></returns>
		protected static bool ClearLog(string ConnectionString, string Filter) 
		{
			SqlCommand Command = null;

			bool Failure = false;
			try 
			{
				Command	= GetSqlCommand(ConnectionString,"truncate table " + Tablename);
				Command.ExecuteNonQuery();
			}
			catch { Failure = true; }			

			CloseConnection(Command);

			return Failure;
		}

		/// <summary>
		/// Returns a count of how many entires there are in the log.
		/// </summary>
		/// <param name="ConnectionString"></param>
		/// <returns></returns>
		public static int GetLogCount(string ConnectionString) 
		{
			SqlCommand Command = null;
			int Result = 0;
			try
			{
			    Command = GetSqlCommand(ConnectionString,"select count(pk) from " + Tablename + " with (READUNCOMMITTED) where Type=0");
				 Result = (int) Command.ExecuteScalar();
			}
			catch { ; }

			CloseConnection(Command);

			return Result;
		}


		/// <summary>
		/// Returns a two column summary table that shows Hour of day and hits for the last 24 hour period.
		/// Field names are  Hits (int), Day (int), Hour (int), Day (int) and AvgTime (float) sorted by day, hour
		/// </summary>
		/// <param name="ConnectionString"></param>
		/// <returns></returns>
		public static DataTable Retrieve24HourSummary(string ConnectionString) 
		{
			SqlCommand Command = null;
			DataTable Table = new DataTable();

            try
            {
                DateTime StartFrom = DateTime.Now.AddHours(-23);
                StartFrom = StartFrom.AddMinutes(-1 * StartFrom.Minute);
                StartFrom = StartFrom.AddSeconds(-1 * StartFrom.Second);

                string Sql = String.Format(
                    @"SELECT datepart(hour,time) as hour, count(*) as hits, DatePart(day,time) as Day, DatePart(month,time) as Month, avg(duration) as AvgTime
FROM WebRequestLog (NOLOCK)
         WHERE time > '{0}'
         GROUP BY DatePart(month,time),DatePart(day,time),datepart(HOUR,TIME)
         order by Day,hour", StartFrom);
                //AddMinutes(-1 * DateTime.Now.Minute))

                Command = GetSqlCommand(ConnectionString, Sql, true);
                SqlDataAdapter Adapter = new SqlDataAdapter(Command);
                Adapter.Fill(Table);
            }
            catch
            {
                Table = null;
            }
            finally
            {
                CloseConnection(Command);
            }

			return Table;
		}


		/// <summary>
		/// Creates the Table that receives log requests. Must pass a valid connection string
		/// to the database that will be receive these log requests.
		/// </summary>
		/// <param name="ConnectionString"></param>
		/// <returns></returns>
		public static bool CreateLogTable(string ConnectionString) 
		{
			string CreateTableString = @"
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WebRequestLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[WebRequestLog]

CREATE TABLE [dbo].[WebRequestLog] (
	[pk] [int] IDENTITY (0, 1) NOT NULL ,
	[time] [datetime] NOT NULL ,
	[Url] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[querystring] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ip] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Post] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[browser] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Details] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Duration] [float] NOT NULL ,
	[Type] [int] NOT NULL ,
	[CustomMessage] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[WebRequestLog] ADD 
	CONSTRAINT [DF__DbaMgr_Tmp__time__5F9E293D] DEFAULT (getdate()) FOR [time],
	CONSTRAINT [DF_WebRequestLog_Url] DEFAULT ('') FOR [Url],
	CONSTRAINT [DF_WebRequestLog_querystring] DEFAULT ('') FOR [querystring],
	CONSTRAINT [DF_WebRequestLog_ip] DEFAULT ('') FOR [ip],
	CONSTRAINT [DF_WebRequestLog_Post] DEFAULT ('') FOR [Post],
	CONSTRAINT [DF_WebRequestLog_browser] DEFAULT ('') FOR [browser],
	CONSTRAINT [DF_WebRequestLog_Details] DEFAULT ('') FOR [Details],
	CONSTRAINT [DF_WebRequestLog_Duration] DEFAULT (0.0) FOR [Duration],
	CONSTRAINT [DF_WebRequestLog_Type] DEFAULT (0) FOR [Type],
	CONSTRAINT [DF_WebRequestLog_CustomMessage] DEFAULT ('') FOR [CustomMessage],
	CONSTRAINT [PK_WebRequestLog] PRIMARY KEY  NONCLUSTERED 
	(
		[pk]
	)  ON [PRIMARY] 

 CREATE  INDEX [Type] ON [dbo].[WebRequestLog]([Type]) ON [PRIMARY]

 CREATE  INDEX [Time] ON [dbo].[WebRequestLog]([time]) ON [PRIMARY]
";
			SqlCommand Command = null;
			bool Failure = false;
			try 
			{
				Command= GetSqlCommand(ConnectionString,CreateTableString);
				Command.ExecuteNonQuery();
			}
			catch {Failure =  true;}

			CloseConnection(Command);
		
			return Failure;
		}
		
		/// <summary>
		/// Helper function to open the connection to the database.
		/// </summary>
		/// <param name="ConnectionString"></param>
		/// <param name="Sql"></param>
		/// <returns></returns>
		protected static SqlCommand GetSqlCommand(string ConnectionString, string Sql) 
		{
			return GetSqlCommand(ConnectionString,Sql,true);
		}

		/// <summary>
		/// Helper function to open connection and retrieve Sql Command
		/// </summary>
		/// <param name="ConnectionString"></param>
		/// <param name="Sql"></param>
		/// <param name="OpenConnection"></param>
		/// <returns></returns>
		protected static SqlCommand GetSqlCommand(string ConnectionString, string Sql,bool OpenConnection) 
		{
			SqlCommand Command = null;
			Command= new SqlCommand();
			Command.CommandText = Sql;
			Command.Connection = new SqlConnection(ConnectionString);
			if (OpenConnection)
				Command.Connection.Open();

            
			return Command;
		}

		/// <summary>
		/// Closes the Command/Connection.
		/// </summary>
		/// <param name="Command"></param>
		protected static void CloseConnection(SqlCommand Command) 
		{
			if (Command != null && Command.Connection != null && 
				Command.Connection.State != ConnectionState.Closed)
				Command.Connection.Close();
		}

	}

	public enum WebRequestLogMessageTypes 
	{
		Normal,
		Error,
		ApplicationMessage
	}

}
