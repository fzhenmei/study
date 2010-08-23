using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using Westwind.Utilities;
using System.Data.Common;
using Westwind.Utilities.Data;

namespace Westwind.Web.Banners
{
    /// <summary>
    /// This is the top level Banner management object that is called from
    /// a front end Web application.
    /// </summary>
    public class BannerManager
    {
        /// <summary>
        /// Global instance
        /// </summary>
        public static BannerManager Current = new BannerManager();

        /// <summary>
        /// Collection of Banners for this banner instance
        /// String value key is the banner id
        /// </summary>
        public Dictionary<string,BannerItem> Banners
        {
            get { return _Banners; }
            set { _Banners = value; }
        }
        private Dictionary<string,BannerItem> _Banners = new Dictionary<string,BannerItem>();
       
        /// <summary>
        /// The Connection string used to retrieve the banners
        /// </summary>                
        public string ConnectionString
        {
            get { return _ConnectionString; }
            set 
            { 
                _ConnectionString = value;
                Data = new SqlDataAccess(_ConnectionString);
            }
        }
        private string _ConnectionString = "";

        /// <summary>
        /// The name of the table that contains the banners
        /// </summary>
        public string BannerTable
        {
            get { return _BannerTable; }
            set { _BannerTable = value; }
        }
        private string _BannerTable = BannerConfiguration.Current.BannerTable;

        
        /// <summary>
        /// Name of the table that holds banner clicks
        /// </summary>
        public string BannerClicksTable
        {
            get { return _BannerClicksTable; }
            set { _BannerClicksTable = value; }
        }
        private string _BannerClicksTable = BannerConfiguration.Current.BannerClicksTable;



        /// <summary>
        /// Determines whether Hit counts and clicks are tracked. 
        /// Results in different links for the images
        /// </summary>
        public bool TrackBannerStatistics
        {
            get { return _TrackBannerStatistics; }
            set { _TrackBannerStatistics = value; }
        }
        private bool _TrackBannerStatistics = true;



        /// <summary>
        /// An error message if an error occurs
        /// </summary>
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
        private string _ErrorMessage = "";


        /// <summary>
        /// Internal Data Access Layer - loaded on startup
        /// </summary>
        protected SqlDataAccess Data
        {
            get { return _Data; }
            set { _Data = value; }
        }
        private SqlDataAccess _Data = null;


        public BannerManager()
        {
            ConnectionString = BannerConfiguration.Current.ConnectionString;
            Data = new SqlDataAccess(ConnectionString);

            TrackBannerStatistics = BannerConfiguration.Current.TrackStatistics;
        }

        /// <summary>
        /// Loads all banners into the Banners collection
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public bool LoadBanners(string BannerGroup)
        {
            SetError();

            DbDataReader Reader;
            if (string.IsNullOrEmpty(BannerGroup))
                Reader = Data.ExecuteReader("select * from " + BannerTable + " order by [bannergroup],sortorder DESC");
            else
                Reader = Data.ExecuteReader("select * from " + BannerTable + " where bannergroup=@Type order by bannergroup,sortorder DESC",
                                                new SqlParameter("@Type", BannerGroup));

            if (Reader == null)
            {
                SetError(Data.ErrorMessage);
                return false;
            }

            if (Reader.FieldCount < 1)
                return false;

            while (Reader.Read() )
            {
                BannerItem Item = ReadBannerItem(Reader);
                Banners.Add( Item.BannerId,Item);
            }

            Reader.Close();

            return true;
        }

        /// <summary>
        /// Deletes an individual banner
        /// </summary>
        /// <param name="BannerId"></param>
        /// <returns></returns>
        public bool DeleteBanner(string BannerId)
        {
            SetError();

            int Result = Data.ExecuteNonQuery("delete from " + BannerTable + " where BannerId=@BannerId", new SqlParameter("@BannerId", BannerId));
            if (Result < 0)
            {
                SetError(Data.ErrorMessage);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Updates the banner click count in the database
        /// </summary>
        /// <param name="BannerId"></param>
        /// <returns></returns>
        public bool ClickBanner(string BannerId)
        {
            SqlDataAccess Access = new SqlDataAccess();
            Data.ConnectionString = ConnectionString;
            int Result = Data.ExecuteNonQuery("update " + BannerTable + " set clicks=clicks+1, resetclicks=resetclicks+1 where BannerId=@BannerId", new SqlParameter("@BannerId", BannerId) );
            if (Result == -1)
            {
                SetError(Data.ErrorMessage);
                return false;
            }

            HttpRequest Request = HttpContext.Current.Request;

            string Sql = "insert " + BannerClicksTable + " (BannerId,Referrer,UserAgent,IpAddress) values (@BannerId,@Referrer,@UserAgent,@IpAddress)";
            Result = Data.ExecuteNonQuery(Sql,
                new SqlParameter("@BannerId",BannerId),
                new SqlParameter("@Referrer",Request.QueryString["u"]),
                new SqlParameter("@UserAgent",Request.UserAgent),
                new SqlParameter("@IpAddress", Request.UserHostAddress) );
            if (Result == -1)
            {
                SetError(Data.ErrorMessage);
                return false;
            }

            return true;                
        }

        /// <summary>
        /// Increments the hit counter on a banner
        /// </summary>
        /// <param name="BannerId"></param>
        /// <returns></returns>
        public bool BannerHit(string BannerId)
        {
            int Result = Data.ExecuteNonQuery("update " + BannerTable + " set hits=hits+1, resethits=resethits+1 where BannerId=@BannerId", new SqlParameter("@BannerId", BannerId));
            if (Result == -1)
            {
                SetError(Data.ErrorMessage);
                return false;
            }
            return true;
        }


        /// <summary>
        /// Returns a specific banner item by its id
        /// </summary>
        /// <param name="BannerId"></param>
        /// <returns></returns>
        public BannerItem GetBanner(string BannerId)
        {
            SetError();

            string Sql = "select * from " + BannerTable + " where BannerId=@BannerId";

            using (DbDataReader Reader = Data.ExecuteReader(Sql,
                                 new SqlParameter("@BannerId", BannerId)))
            {
                if (Reader == null)
                {
                    SetError(Data.ErrorMessage);
                    return null;
                }

                if (!Reader.Read())
                    return null;

                BannerItem Item = ReadBannerItem(Reader);

                Reader.Close();

                return Item;
            }
        }

        /// <summary>
        /// Returns a random banner
        /// </summary>
        /// <param name="Group"></param>
        /// <returns></returns>
        public BannerItem GetNextBanner(string Group)
        {
            SetError();

            if (Group == null)
                Group = "";

            DbDataReader Reader = null;
            DbCommand Command = Data.CreateCommand(BannerStoredProcedures.GetNextBanner, new SqlParameter("@Group", Group));
            if (Command == null)
            {
                SetError(Data.ErrorMessage);
                return null;
            }
            Command.CommandType = CommandType.StoredProcedure;

            Reader = Data.ExecuteReader(Command);

            if (Reader == null)
            {
                SetError(Data.ErrorMessage);
                return null;
            }

            if (!Reader.Read())
                return null;

            BannerItem Item = ReadBannerItem(Reader);
            Reader.Close();

            Data.CloseConnection(Command);

            return Item;
        }


        /// <summary>
        /// Renders a given banner as a HTML
        /// </summary>
        /// <param name="banner"></param>
        /// <returns></returns>
        public string RenderBanner(BannerItem banner)
        {
            if (banner == null)
                return "";

            return banner.RenderScriptInclude();
        }

        /// <summary>
        /// Renders a given banner as a HTML
        /// </summary>
        /// <param name="banner"></param>
        /// <returns></returns>
        public string RenderBanner(BannerItem banner, bool NoTracking)
        {
            if (!NoTracking)
                return RenderBanner(banner);
            else
                return banner.RenderScriptInclude();
        }

        /// <summary>
        /// Renders a given banner as a script include
        /// </summary>
        /// <param name="BannerId"></param>
        /// <returns></returns>
        public string RenderBanner(string BannerId)
        {
            BannerItem banner = GetBanner(BannerId);
            if (banner == null)
                return "";

            return banner.RenderScriptInclude();
        }


        /// <summary>
        /// Renders a given banner as a HTML
        /// </summary>
        /// <param name="banner"></param>
        /// <returns></returns>
        public string RenderBannerLink(BannerItem banner)
        {
            if (banner == null)
                return "";

            return banner.RenderLink();
        }


        /// <summary>
        /// Renders a given banner as a script include
        /// </summary>
        /// <param name="BannerId"></param>
        /// <returns></returns>
        public string RenderBannerLink(string BannerId)
        {
            BannerItem banner = GetBanner(BannerId);
            if (banner == null)
                return "";

            return banner.RenderLink();
        }

        /// <summary>
        /// Retrieves the next banner for the group and renders the banner as a 
        /// script include into the page.
        /// 
        /// This link is created as a script link so that robots will not Click
        /// or follow this banner's link.
        /// </summary>
        /// <returns></returns>
        public string RenderNextBanner(string bannerGroup)
        {
            string Url = BannerItem.RenderGroupScriptInclude(bannerGroup);
            return "<script src='" + Url + "' type='text/javascript'></script>";
        }

        /// <summary>
        /// Retrieves the next banner for the group and renders the banner as a 
        /// script include into the page.
        /// 
        /// This link is created as a script link so that robots will not Click
        /// or follow this banner's link.
        /// </summary>
        /// <returns></returns>
        public string RenderNextBanner()
        {
            return RenderNextBanner(null);
        }


        /// <summary>
        /// This is the primary rendering routine that retrieves the next Banner
        /// and renders it as a script include into the page.
        /// </summary>
        /// <param name="BannerGroup"></param>
        /// <returns></returns>
        public string RenderNextBannerLink(string BannerGroup)
        {            
            BannerItem banner = GetNextBanner(BannerGroup);
            if (banner == null)
                return "";

            return banner.RenderLink();
        }

        /// <summary>
        /// This is the primary rendering routine that retrieves the next Banner
        /// and renders it as a script include into the page.
        /// </summary>
        /// <returns></returns>
        public string RenderNextBannerLink()
        {
            return RenderNextBannerLink(null);
        }

        /// <summary>
        /// Loads an individual Banner Entity from a DataReader
        /// </summary>
        /// <param name="Reader"></param>
        /// <returns></returns>
        private BannerItem ReadBannerItem(DbDataReader Reader)
        {
            BannerItem Item = new BannerItem();
            Item.BannerId = Reader["BannerId"] as string;
            Item.BannerGroup = Reader["BannerGroup"] as string;
            Item.Hits = (int)Reader["Hits"];
            Item.ResetHits = (int)Reader["ResetHits"];
            Item.ImageUrl = Reader["ImageUrl"] as string;
            Item.NavigateUrl = Reader["NavigateUrl"] as string;
            Item.Clicks = (int) Reader["Clicks"];
            Item.ResetClicks = (int) Reader["ResetClicks"];
            Item.SortOrder = (int)Reader["SortOrder"];
            Item.MaxHits = (int)Reader["MaxHits"];
            Item.Active = (bool)Reader["Active"];
            Item.Updated = (DateTime)Reader["Updated"];
            Item.Entered = (DateTime) Reader["Entered"];
            Item.Width = (int)Reader["Width"];
            Item.Height = (int)Reader["Height"];
            Item.BannerType = (BannerTypes)Reader["BannerType"];

            return Item;
        }

        /// <summary>
        /// Updates an existing banner item.
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public bool UpdateBannerItem(BannerItem Item)
        {            

            string Sql = 
"update " + BannerTable + @" 
 set BannerGroup=@BannerGroup, Hits=@Hits, ResetHits=@ResetHits,ImageUrl=@ImageUrl,NavigateUrl=@NavigateUrl,
     Clicks=@Clicks,ResetClicks=@ResetClicks,Active=@Active,Updated=@Updated,Width=@Width,Height=@Height where BannerId=@BannerId";

            DbCommand Command = Data.CreateCommand(Sql,
                        new SqlParameter("@BannerId",Item.BannerId),
                        new SqlParameter("@BannerGroup",Item.BannerGroup),
                        new SqlParameter("@Hits",Item.Hits),                        
                        new SqlParameter("@ImageUrl",Item.ImageUrl),
                        new SqlParameter("@NavigateUrl",Item.NavigateUrl),                        
                        new SqlParameter("@Clicks",Item.Clicks),
                        new SqlParameter("@ResetHits", Item.ResetHits),
                        new SqlParameter("@ResetClicks",Item.ResetClicks),
                        new SqlParameter("@MaxHits", Item.MaxHits),
                        new SqlParameter("@SortOrder", Item.SortOrder),
                        new SqlParameter("@Active",Item.Active),
                        new SqlParameter("@Updated",DateTime.Now),
                        new SqlParameter("@Width", Item.Width),
                        new SqlParameter("@Height", Item.Height)

                        );

            if (Data.ExecuteNonQuery(Command) < 0)
            {
                SetError(Data.ErrorMessage);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Creates a new Banner Id. Banner Ids are GUID Hashcodes
        /// </summary>
        /// <returns></returns>
        private string GetNewBannerId()
        {
            return Guid.NewGuid().GetHashCode().ToString("x");
        }


        /// <summary>
        /// Inserts a new banner
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public string InsertBannerItem(BannerItem Item)
        {
            if (string.IsNullOrEmpty(Item.BannerId))
                Item.BannerId = GetNewBannerId();
            
            string Sql =
"insert into " + BannerTable + @" 
    (BannerId,BannerGroup,Hits,ResetHits,Clicks,ResetClicks,MaxHits,ImageUrl,NavigateUrl,SortOrder,Active,Width,Height) VALUES
    (@BannerId,@BannerGroup,@Hits,@ResetHits,@Clicks,@ResetClicks,@MaxHits,@ImageUrl,@NavigateUrl,@SortOrder,@Active,@Width,@Height)";

            DbCommand Command = Data.CreateCommand(Sql,
                        new SqlParameter("@BannerId", Item.BannerId),
                        new SqlParameter("@BannerGroup", Item.BannerGroup),
                        new SqlParameter("@Hits", Item.Hits),
                        new SqlParameter("@ImageUrl", Item.ImageUrl),
                        new SqlParameter("@NavigateUrl", Item.NavigateUrl),                        
                        new SqlParameter("@Clicks", Item.Clicks),
                        new SqlParameter("@ResetHits", Item.ResetHits),
                        new SqlParameter("@ResetClicks", Item.ResetClicks),
                        new SqlParameter("@MaxHits", Item.MaxHits),
                        new SqlParameter("@SortOrder", Item.SortOrder),                        
                        new SqlParameter("@Active", Item.Active),
                        new SqlParameter("@Width", Item.Width),
                        new SqlParameter("@Height", Item.Height)
                            );
                        

            if (Data.ExecuteNonQuery(Command) < 0)
            {
                SetError(Data.ErrorMessage);
                return null;
            }

            return Item.BannerId;
        }

        /// <summary>
        /// Creates the Sql Server Tables
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool CreateTables(string ConnectionString)
        {
            SqlDataAccess data = Data;
            if (!string.IsNullOrEmpty(ConnectionString))
                data = new SqlDataAccess(ConnectionString);

            string Sql = @"
CREATE TABLE [dbo].[wwBanners](
	[BannerId] [varchar](20) NOT NULL DEFAULT (''),
	[BannerGroup] [varchar](50) NOT NULL DEFAULT (''),
	[NavigateUrl] [varchar](255) NOT NULL  DEFAULT (''),
	[ImageUrl] [varchar](255) NOT NULL  DEFAULT (''),
	[Entered] [datetime] NOT NULL   DEFAULT (getdate()),
	[Updated] [datetime] NOT NULL   DEFAULT (getdate()),
	[Hits] [int] NOT NULL   DEFAULT ((0)),
	[MaxHits] [int] NOT NULL  DEFAULT ((0)),
	[ResetHits] [int] NOT NULL   DEFAULT ((0)),
	[Clicks] [int] NOT NULL  DEFAULT ((0)),
	[ResetClicks] [int] NOT NULL   DEFAULT ((0)),
	[Active] [bit] NOT NULL   DEFAULT ((1)),
	[SortOrder] [int] NOT NULL  DEFAULT ((0)),
	[Type] [varchar](20) NOT NULL   DEFAULT (''),
	[Width] [int] NOT NULL  DEFAULT ((480)),
	[Height] [int] NOT NULL   DEFAULT ((60)),
    [BannerType] [int] NOT NULL Default((0)),
	CONSTRAINT [PK_wwBanners] PRIMARY KEY CLUSTERED 
(
	[BannerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[wwBannerClicks](
	[Pk] [int] IDENTITY(1,1) NOT NULL,
	[Timestamp] [datetime] NOT NULL   DEFAULT (getdate()),
	[BannerId] [varchar](20) NOT NULL  DEFAULT (''),
	[Referrer] [varchar](512) NOT NULL DEFAULT (''),
	[UserAgent] [varchar](512) NOT NULL  DEFAULT (''),
	[IpAddress] [varchar](50) NOT NULL DEFAULT (''),
	CONSTRAINT [PK_wwBannerClicks] PRIMARY KEY CLUSTERED 
(
	[Pk] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE PROCEDURE [dbo].[ww_GetNextBanner] 
    @Group as varchar(100)
AS
BEGIN
	SET NOCOUNT ON;
    
	declare @Rand as Int

	CREATE TABLE #PageIndex
	( 
		RowId INT IDENTITY NOT NULL, 
		BannerId varchar(50)
    )

    if ( @Group is NULL or @Group = '')
       Begin	   
		insert into #PageIndex (BannerId) 
				select BannerId 
					   from wwBanners 					   
					   where MaxHits = 0 OR MaxHits > 0 and ResetHits < MaxHits
					   order by BannerId  
        

		select  @Rand = CAST( RAND() * @@ROWCOUNT as Int ) + 1
		
		select RowId,wwBanners.* from wwBanners,#PageIndex 
               where RowId = @Rand AND wwBanners.BannerId = #PageINdex.BannerId
	   End
    else
       Begin
		insert into #PageIndex (BannerId) 
               select BannerId from wwBanners 
                      where (BannerGroup = @Group or BannerGroup = '') and MaxHits = 0 OR MaxHits > 0 and ResetHits < MaxHits
                      order by BannerId
		select  @Rand = CAST( RAND() * @@ROWCOUNT as Int ) + 1

		select RowId,wwBanners.* from wwBanners,#PageIndex 
               where  RowId = @Rand AND wwBanners.BannerId = #PageINdex.BannerId
       End
	
END
GO
";

            Sql = Sql.Replace("wwBanners", BannerTable);
            Sql = Sql.Replace("wwBannerClicks", BannerClicksTable);

            string[] cmds = Sql.Split(new string[1] { "GO" }, StringSplitOptions.RemoveEmptyEntries);            
            foreach(string cmd in cmds)
            {
                int Result = data.ExecuteNonQuery(cmd);
                if (Result == -1)
                {
                    SetError(data.ErrorMessage);
                    return false;
                }
            }

            return true;
        }

        


        public void SetError(string Message)
        {
            ErrorMessage = Message;
        }
        public void SetError()
        {
            ErrorMessage = "";
        }
    }

    class BannerStoredProcedures
    {
        public static string GetNextBanner = "ww_getnextbanner";
    }
}
