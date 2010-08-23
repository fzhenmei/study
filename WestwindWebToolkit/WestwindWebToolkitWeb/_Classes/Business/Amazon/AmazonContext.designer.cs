﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Amazon
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Runtime.Serialization;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="WcfAjax")]
	public partial class AmazonContext : Westwind.BusinessFramework.LinqToSql.DataContextSql
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertAmazonBook(AmazonBook instance);
    partial void UpdateAmazonBook(AmazonBook instance);
    partial void DeleteAmazonBook(AmazonBook instance);
    partial void InsertAmazonLookupList(AmazonLookupList instance);
    partial void UpdateAmazonLookupList(AmazonLookupList instance);
    partial void DeleteAmazonLookupList(AmazonLookupList instance);
    #endregion
		
		public AmazonContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["DevSampleConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public AmazonContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AmazonContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AmazonContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AmazonContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<AmazonBook> AmazonBooks
		{
			get
			{
				return this.GetTable<AmazonBook>();
			}
		}
		
		public System.Data.Linq.Table<AmazonLookupList> AmazonLookupLists
		{
			get
			{
				return this.GetTable<AmazonLookupList>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.AmazonBooks")]
	[global::System.Runtime.Serialization.DataContractAttribute()]
	public partial class AmazonBook : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Pk;
		
		private string _Title;
		
		private string _ISBN;
		
		private string _AmazonUrl;
		
		private string _AmazonImage;
		
		private string _AmazonSmallImage;
		
		private string _Author;
		
		private string _Description;
		
		private string _Published;
		
		private string _Review;
		
		private int _Rating;
		
		private bool _Highlight;
		
		private string _Category;
		
		private System.DateTime _Entered;
		
		private int _SortOrder;
		
		private string _Type;
		
		private System.Data.Linq.Binary _tStamp;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnPkChanging(int value);
    partial void OnPkChanged();
    partial void OnTitleChanging(string value);
    partial void OnTitleChanged();
    partial void OnISBNChanging(string value);
    partial void OnISBNChanged();
    partial void OnAmazonUrlChanging(string value);
    partial void OnAmazonUrlChanged();
    partial void OnAmazonImageChanging(string value);
    partial void OnAmazonImageChanged();
    partial void OnAmazonSmallImageChanging(string value);
    partial void OnAmazonSmallImageChanged();
    partial void OnAuthorChanging(string value);
    partial void OnAuthorChanged();
    partial void OnDescriptionChanging(string value);
    partial void OnDescriptionChanged();
    partial void OnPublishedChanging(string value);
    partial void OnPublishedChanged();
    partial void OnReviewChanging(string value);
    partial void OnReviewChanged();
    partial void OnRatingChanging(int value);
    partial void OnRatingChanged();
    partial void OnHighlightChanging(bool value);
    partial void OnHighlightChanged();
    partial void OnCategoryChanging(string value);
    partial void OnCategoryChanged();
    partial void OnEnteredChanging(System.DateTime value);
    partial void OnEnteredChanged();
    partial void OnSortOrderChanging(int value);
    partial void OnSortOrderChanged();
    partial void OnTypeChanging(string value);
    partial void OnTypeChanged();
    partial void OntStampChanging(System.Data.Linq.Binary value);
    partial void OntStampChanged();
    #endregion
		
		public AmazonBook()
		{
			this.Initialize();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pk", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true, UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=1)]
		public int Pk
		{
			get
			{
				return this._Pk;
			}
			set
			{
				if ((this._Pk != value))
				{
					this.OnPkChanging(value);
					this.SendPropertyChanging();
					this._Pk = value;
					this.SendPropertyChanged("Pk");
					this.OnPkChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Title", DbType="NVarChar(255)", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=2)]
		public string Title
		{
			get
			{
				return this._Title;
			}
			set
			{
				if ((this._Title != value))
				{
					this.OnTitleChanging(value);
					this.SendPropertyChanging();
					this._Title = value;
					this.SendPropertyChanged("Title");
					this.OnTitleChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ISBN", DbType="NVarChar(50)", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=3)]
		public string ISBN
		{
			get
			{
				return this._ISBN;
			}
			set
			{
				if ((this._ISBN != value))
				{
					this.OnISBNChanging(value);
					this.SendPropertyChanging();
					this._ISBN = value;
					this.SendPropertyChanged("ISBN");
					this.OnISBNChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AmazonUrl", DbType="VarChar(512)", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=4)]
		public string AmazonUrl
		{
			get
			{
				return this._AmazonUrl;
			}
			set
			{
				if ((this._AmazonUrl != value))
				{
					this.OnAmazonUrlChanging(value);
					this.SendPropertyChanging();
					this._AmazonUrl = value;
					this.SendPropertyChanged("AmazonUrl");
					this.OnAmazonUrlChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AmazonImage", DbType="VarChar(255)", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=5)]
		public string AmazonImage
		{
			get
			{
				return this._AmazonImage;
			}
			set
			{
				if ((this._AmazonImage != value))
				{
					this.OnAmazonImageChanging(value);
					this.SendPropertyChanging();
					this._AmazonImage = value;
					this.SendPropertyChanged("AmazonImage");
					this.OnAmazonImageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AmazonSmallImage", DbType="VarChar(255)", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=6)]
		public string AmazonSmallImage
		{
			get
			{
				return this._AmazonSmallImage;
			}
			set
			{
				if ((this._AmazonSmallImage != value))
				{
					this.OnAmazonSmallImageChanging(value);
					this.SendPropertyChanging();
					this._AmazonSmallImage = value;
					this.SendPropertyChanged("AmazonSmallImage");
					this.OnAmazonSmallImageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Author", DbType="NVarChar(255)", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=7)]
		public string Author
		{
			get
			{
				return this._Author;
			}
			set
			{
				if ((this._Author != value))
				{
					this.OnAuthorChanging(value);
					this.SendPropertyChanging();
					this._Author = value;
					this.SendPropertyChanged("Author");
					this.OnAuthorChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Description", DbType="NText", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=8)]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Published", DbType="NVarChar(50)", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=9)]
		public string Published
		{
			get
			{
				return this._Published;
			}
			set
			{
				if ((this._Published != value))
				{
					this.OnPublishedChanging(value);
					this.SendPropertyChanging();
					this._Published = value;
					this.SendPropertyChanged("Published");
					this.OnPublishedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Review", DbType="NText", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=10)]
		public string Review
		{
			get
			{
				return this._Review;
			}
			set
			{
				if ((this._Review != value))
				{
					this.OnReviewChanging(value);
					this.SendPropertyChanging();
					this._Review = value;
					this.SendPropertyChanged("Review");
					this.OnReviewChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Rating", DbType="Int NOT NULL", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=11)]
		public int Rating
		{
			get
			{
				return this._Rating;
			}
			set
			{
				if ((this._Rating != value))
				{
					this.OnRatingChanging(value);
					this.SendPropertyChanging();
					this._Rating = value;
					this.SendPropertyChanged("Rating");
					this.OnRatingChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Highlight", DbType="Bit NOT NULL", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=12)]
		public bool Highlight
		{
			get
			{
				return this._Highlight;
			}
			set
			{
				if ((this._Highlight != value))
				{
					this.OnHighlightChanging(value);
					this.SendPropertyChanging();
					this._Highlight = value;
					this.SendPropertyChanged("Highlight");
					this.OnHighlightChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Category", DbType="NVarChar(50)", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=13)]
		public string Category
		{
			get
			{
				return this._Category;
			}
			set
			{
				if ((this._Category != value))
				{
					this.OnCategoryChanging(value);
					this.SendPropertyChanging();
					this._Category = value;
					this.SendPropertyChanged("Category");
					this.OnCategoryChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Entered", DbType="DateTime NOT NULL", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=14)]
		public System.DateTime Entered
		{
			get
			{
				return this._Entered;
			}
			set
			{
				if ((this._Entered != value))
				{
					this.OnEnteredChanging(value);
					this.SendPropertyChanging();
					this._Entered = value;
					this.SendPropertyChanged("Entered");
					this.OnEnteredChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SortOrder", DbType="Int NOT NULL", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=15)]
		public int SortOrder
		{
			get
			{
				return this._SortOrder;
			}
			set
			{
				if ((this._SortOrder != value))
				{
					this.OnSortOrderChanging(value);
					this.SendPropertyChanging();
					this._SortOrder = value;
					this.SendPropertyChanged("SortOrder");
					this.OnSortOrderChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Type", DbType="VarChar(20)", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=16)]
		public string Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				if ((this._Type != value))
				{
					this.OnTypeChanging(value);
					this.SendPropertyChanging();
					this._Type = value;
					this.SendPropertyChanged("Type");
					this.OnTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_tStamp", AutoSync=AutoSync.Always, DbType="rowversion NOT NULL", CanBeNull=false, IsDbGenerated=true, IsVersion=true, UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=17)]
		public System.Data.Linq.Binary tStamp
		{
			get
			{
				return this._tStamp;
			}
			set
			{
				if ((this._tStamp != value))
				{
					this.OntStampChanging(value);
					this.SendPropertyChanging();
					this._tStamp = value;
					this.SendPropertyChanged("tStamp");
					this.OntStampChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void Initialize()
		{
			OnCreated();
		}
		
		[global::System.Runtime.Serialization.OnDeserializingAttribute()]
		[global::System.ComponentModel.EditorBrowsableAttribute(EditorBrowsableState.Never)]
		public void OnDeserializing(StreamingContext context)
		{
			this.Initialize();
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.AmazonLookups")]
	[global::System.Runtime.Serialization.DataContractAttribute()]
	public partial class AmazonLookupList : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Pk;
		
		private string _Type;
		
		private string _cData;
		
		private string _cData1;
		
		private string _cData2;
		
		private int _iData;
		
		private System.Data.Linq.Binary _tStamp;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnPkChanging(int value);
    partial void OnPkChanged();
    partial void OnTypeChanging(string value);
    partial void OnTypeChanged();
    partial void OncDataChanging(string value);
    partial void OncDataChanged();
    partial void OncData1Changing(string value);
    partial void OncData1Changed();
    partial void OncData2Changing(string value);
    partial void OncData2Changed();
    partial void OniDataChanging(int value);
    partial void OniDataChanged();
    partial void OntStampChanging(System.Data.Linq.Binary value);
    partial void OntStampChanged();
    #endregion
		
		public AmazonLookupList()
		{
			this.Initialize();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pk", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true, UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=1)]
		public int Pk
		{
			get
			{
				return this._Pk;
			}
			set
			{
				if ((this._Pk != value))
				{
					this.OnPkChanging(value);
					this.SendPropertyChanging();
					this._Pk = value;
					this.SendPropertyChanged("Pk");
					this.OnPkChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Type", DbType="VarChar(128)", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=2)]
		public string Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				if ((this._Type != value))
				{
					this.OnTypeChanging(value);
					this.SendPropertyChanging();
					this._Type = value;
					this.SendPropertyChanged("Type");
					this.OnTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_cData", DbType="NVarChar(128)", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=3)]
		public string cData
		{
			get
			{
				return this._cData;
			}
			set
			{
				if ((this._cData != value))
				{
					this.OncDataChanging(value);
					this.SendPropertyChanging();
					this._cData = value;
					this.SendPropertyChanged("cData");
					this.OncDataChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_cData1", DbType="NVarChar(2048)", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=4)]
		public string cData1
		{
			get
			{
				return this._cData1;
			}
			set
			{
				if ((this._cData1 != value))
				{
					this.OncData1Changing(value);
					this.SendPropertyChanging();
					this._cData1 = value;
					this.SendPropertyChanged("cData1");
					this.OncData1Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_cData2", DbType="NVarChar(4000)", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=5)]
		public string cData2
		{
			get
			{
				return this._cData2;
			}
			set
			{
				if ((this._cData2 != value))
				{
					this.OncData2Changing(value);
					this.SendPropertyChanging();
					this._cData2 = value;
					this.SendPropertyChanged("cData2");
					this.OncData2Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_iData", DbType="Int NOT NULL", UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=6)]
		public int iData
		{
			get
			{
				return this._iData;
			}
			set
			{
				if ((this._iData != value))
				{
					this.OniDataChanging(value);
					this.SendPropertyChanging();
					this._iData = value;
					this.SendPropertyChanged("iData");
					this.OniDataChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_tStamp", AutoSync=AutoSync.Always, DbType="rowversion NOT NULL", CanBeNull=false, IsDbGenerated=true, IsVersion=true, UpdateCheck=UpdateCheck.Never)]
		[global::System.Runtime.Serialization.DataMemberAttribute(Order=7)]
		public System.Data.Linq.Binary tStamp
		{
			get
			{
				return this._tStamp;
			}
			set
			{
				if ((this._tStamp != value))
				{
					this.OntStampChanging(value);
					this.SendPropertyChanging();
					this._tStamp = value;
					this.SendPropertyChanged("tStamp");
					this.OntStampChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void Initialize()
		{
			OnCreated();
		}
		
		[global::System.Runtime.Serialization.OnDeserializingAttribute()]
		[global::System.ComponentModel.EditorBrowsableAttribute(EditorBrowsableState.Never)]
		public void OnDeserializing(StreamingContext context)
		{
			this.Initialize();
		}
	}
}
#pragma warning restore 1591
