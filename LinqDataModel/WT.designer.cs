﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FW.WT.LinqDataModel
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="FwWorkTools")]
	public partial class WTDataContext<T> : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertAddressBook(AddressBook instance);
    partial void UpdateAddressBook(AddressBook instance);
    partial void DeleteAddressBook(AddressBook instance);
    partial void InsertMenu(Menu instance);
    partial void UpdateMenu(Menu instance);
    partial void DeleteMenu(Menu instance);
    partial void InsertSrcCodeManage(SrcCodeManage instance);
    partial void UpdateSrcCodeManage(SrcCodeManage instance);
    partial void DeleteSrcCodeManage(SrcCodeManage instance);
    partial void InsertFeedBackLog(FeedBackLog instance);
    partial void UpdateFeedBackLog(FeedBackLog instance);
    partial void DeleteFeedBackLog(FeedBackLog instance);
    #endregion
		
		public WTDataContext() : 
				base(global::FW.WT.LinqDataModel.Properties.Settings.Default.FwWorkToolsConnectionString11, mappingSource)
		{
			OnCreated();
		}
		
		public WTDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public WTDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public WTDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public WTDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<AddressBook> AddressBooks
		{
			get
			{
				return this.GetTable<AddressBook>();
			}
		}
		
		public System.Data.Linq.Table<Menu> Menus
		{
			get
			{
				return this.GetTable<Menu>();
			}
		}
		
		public System.Data.Linq.Table<SrcCodeManage> SrcCodeManages
		{
			get
			{
				return this.GetTable<SrcCodeManage>();
			}
		}
		
		public System.Data.Linq.Table<FeedBackLog> FeedBackLogs
		{
			get
			{
				return this.GetTable<FeedBackLog>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.AddressBooks")]
	public partial class AddressBook : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _AddressBookID;
		
		private string _Name;
		
		private string _Phone;
		
		private string _QQ;
		
		private string _Area;
		
		private string _Position;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnAddressBookIDChanging(int value);
    partial void OnAddressBookIDChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnPhoneChanging(string value);
    partial void OnPhoneChanged();
    partial void OnQQChanging(string value);
    partial void OnQQChanged();
    partial void OnAreaChanging(string value);
    partial void OnAreaChanged();
    partial void OnPositionChanging(string value);
    partial void OnPositionChanged();
    #endregion
		
		public AddressBook()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AddressBookID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int AddressBookID
		{
			get
			{
				return this._AddressBookID;
			}
			set
			{
				if ((this._AddressBookID != value))
				{
					this.OnAddressBookIDChanging(value);
					this.SendPropertyChanging();
					this._AddressBookID = value;
					this.SendPropertyChanged("AddressBookID");
					this.OnAddressBookIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(20)")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Phone", DbType="NVarChar(15)")]
		public string Phone
		{
			get
			{
				return this._Phone;
			}
			set
			{
				if ((this._Phone != value))
				{
					this.OnPhoneChanging(value);
					this.SendPropertyChanging();
					this._Phone = value;
					this.SendPropertyChanged("Phone");
					this.OnPhoneChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_QQ", DbType="NVarChar(15)")]
		public string QQ
		{
			get
			{
				return this._QQ;
			}
			set
			{
				if ((this._QQ != value))
				{
					this.OnQQChanging(value);
					this.SendPropertyChanging();
					this._QQ = value;
					this.SendPropertyChanged("QQ");
					this.OnQQChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Area", DbType="NVarChar(50)")]
		public string Area
		{
			get
			{
				return this._Area;
			}
			set
			{
				if ((this._Area != value))
				{
					this.OnAreaChanging(value);
					this.SendPropertyChanging();
					this._Area = value;
					this.SendPropertyChanged("Area");
					this.OnAreaChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Position", DbType="NVarChar(50)")]
		public string Position
		{
			get
			{
				return this._Position;
			}
			set
			{
				if ((this._Position != value))
				{
					this.OnPositionChanging(value);
					this.SendPropertyChanging();
					this._Position = value;
					this.SendPropertyChanged("Position");
					this.OnPositionChanged();
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
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Menu")]
	public partial class Menu : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _MenuID;
		
		private System.Nullable<int> _GroupID;
		
		private string _MenuName;
		
		private string _MenuType;
		
		private System.Nullable<int> _ParentID;
		
		private System.Nullable<bool> _IsShow;
		
		private System.Nullable<int> _OrderBy;
		
		private string _Url;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnMenuIDChanging(int value);
    partial void OnMenuIDChanged();
    partial void OnGroupIDChanging(System.Nullable<int> value);
    partial void OnGroupIDChanged();
    partial void OnMenuNameChanging(string value);
    partial void OnMenuNameChanged();
    partial void OnMenuTypeChanging(string value);
    partial void OnMenuTypeChanged();
    partial void OnParentIDChanging(System.Nullable<int> value);
    partial void OnParentIDChanged();
    partial void OnIsShowChanging(System.Nullable<bool> value);
    partial void OnIsShowChanged();
    partial void OnOrderByChanging(System.Nullable<int> value);
    partial void OnOrderByChanged();
    partial void OnUrlChanging(string value);
    partial void OnUrlChanged();
    #endregion
		
		public Menu()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MenuID", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int MenuID
		{
			get
			{
				return this._MenuID;
			}
			set
			{
				if ((this._MenuID != value))
				{
					this.OnMenuIDChanging(value);
					this.SendPropertyChanging();
					this._MenuID = value;
					this.SendPropertyChanged("MenuID");
					this.OnMenuIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GroupID", DbType="Int")]
		public System.Nullable<int> GroupID
		{
			get
			{
				return this._GroupID;
			}
			set
			{
				if ((this._GroupID != value))
				{
					this.OnGroupIDChanging(value);
					this.SendPropertyChanging();
					this._GroupID = value;
					this.SendPropertyChanged("GroupID");
					this.OnGroupIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MenuName", DbType="NVarChar(20)")]
		public string MenuName
		{
			get
			{
				return this._MenuName;
			}
			set
			{
				if ((this._MenuName != value))
				{
					this.OnMenuNameChanging(value);
					this.SendPropertyChanging();
					this._MenuName = value;
					this.SendPropertyChanged("MenuName");
					this.OnMenuNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MenuType", DbType="NVarChar(10)")]
		public string MenuType
		{
			get
			{
				return this._MenuType;
			}
			set
			{
				if ((this._MenuType != value))
				{
					this.OnMenuTypeChanging(value);
					this.SendPropertyChanging();
					this._MenuType = value;
					this.SendPropertyChanged("MenuType");
					this.OnMenuTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ParentID", DbType="Int")]
		public System.Nullable<int> ParentID
		{
			get
			{
				return this._ParentID;
			}
			set
			{
				if ((this._ParentID != value))
				{
					this.OnParentIDChanging(value);
					this.SendPropertyChanging();
					this._ParentID = value;
					this.SendPropertyChanged("ParentID");
					this.OnParentIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsShow", DbType="Bit")]
		public System.Nullable<bool> IsShow
		{
			get
			{
				return this._IsShow;
			}
			set
			{
				if ((this._IsShow != value))
				{
					this.OnIsShowChanging(value);
					this.SendPropertyChanging();
					this._IsShow = value;
					this.SendPropertyChanged("IsShow");
					this.OnIsShowChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OrderBy", DbType="Int")]
		public System.Nullable<int> OrderBy
		{
			get
			{
				return this._OrderBy;
			}
			set
			{
				if ((this._OrderBy != value))
				{
					this.OnOrderByChanging(value);
					this.SendPropertyChanging();
					this._OrderBy = value;
					this.SendPropertyChanged("OrderBy");
					this.OnOrderByChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Url", DbType="NVarChar(200)")]
		public string Url
		{
			get
			{
				return this._Url;
			}
			set
			{
				if ((this._Url != value))
				{
					this.OnUrlChanging(value);
					this.SendPropertyChanging();
					this._Url = value;
					this.SendPropertyChanged("Url");
					this.OnUrlChanged();
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
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.SrcCodeManage")]
	public partial class SrcCodeManage : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _SrcCodeManageID;
		
		private string _CustomerName;
		
		private string _CustomerArea;
		
		private string _VSSAddress;
		
		private string _ServerName;
		
		private string _ServerPort;
		
		private string _DBName;
		
		private string _ApplicationName;
		
		private string _UserName;
		
		private string _UserPwd;
		
		private string _Remark;
		
		private string _ProjVersion;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnSrcCodeManageIDChanging(int value);
    partial void OnSrcCodeManageIDChanged();
    partial void OnCustomerNameChanging(string value);
    partial void OnCustomerNameChanged();
    partial void OnCustomerAreaChanging(string value);
    partial void OnCustomerAreaChanged();
    partial void OnVSSAddressChanging(string value);
    partial void OnVSSAddressChanged();
    partial void OnServerNameChanging(string value);
    partial void OnServerNameChanged();
    partial void OnServerPortChanging(string value);
    partial void OnServerPortChanged();
    partial void OnDBNameChanging(string value);
    partial void OnDBNameChanged();
    partial void OnApplicationNameChanging(string value);
    partial void OnApplicationNameChanged();
    partial void OnUserNameChanging(string value);
    partial void OnUserNameChanged();
    partial void OnUserPwdChanging(string value);
    partial void OnUserPwdChanged();
    partial void OnRemarkChanging(string value);
    partial void OnRemarkChanged();
    partial void OnProjVersionChanging(string value);
    partial void OnProjVersionChanged();
    #endregion
		
		public SrcCodeManage()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SrcCodeManageID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int SrcCodeManageID
		{
			get
			{
				return this._SrcCodeManageID;
			}
			set
			{
				if ((this._SrcCodeManageID != value))
				{
					this.OnSrcCodeManageIDChanging(value);
					this.SendPropertyChanging();
					this._SrcCodeManageID = value;
					this.SendPropertyChanged("SrcCodeManageID");
					this.OnSrcCodeManageIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CustomerName", DbType="NVarChar(20)")]
		public string CustomerName
		{
			get
			{
				return this._CustomerName;
			}
			set
			{
				if ((this._CustomerName != value))
				{
					this.OnCustomerNameChanging(value);
					this.SendPropertyChanging();
					this._CustomerName = value;
					this.SendPropertyChanged("CustomerName");
					this.OnCustomerNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CustomerArea", DbType="NVarChar(20)")]
		public string CustomerArea
		{
			get
			{
				return this._CustomerArea;
			}
			set
			{
				if ((this._CustomerArea != value))
				{
					this.OnCustomerAreaChanging(value);
					this.SendPropertyChanging();
					this._CustomerArea = value;
					this.SendPropertyChanged("CustomerArea");
					this.OnCustomerAreaChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_VSSAddress", DbType="NVarChar(500)")]
		public string VSSAddress
		{
			get
			{
				return this._VSSAddress;
			}
			set
			{
				if ((this._VSSAddress != value))
				{
					this.OnVSSAddressChanging(value);
					this.SendPropertyChanging();
					this._VSSAddress = value;
					this.SendPropertyChanged("VSSAddress");
					this.OnVSSAddressChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ServerName", DbType="NVarChar(50)")]
		public string ServerName
		{
			get
			{
				return this._ServerName;
			}
			set
			{
				if ((this._ServerName != value))
				{
					this.OnServerNameChanging(value);
					this.SendPropertyChanging();
					this._ServerName = value;
					this.SendPropertyChanged("ServerName");
					this.OnServerNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ServerPort", DbType="NVarChar(10)")]
		public string ServerPort
		{
			get
			{
				return this._ServerPort;
			}
			set
			{
				if ((this._ServerPort != value))
				{
					this.OnServerPortChanging(value);
					this.SendPropertyChanging();
					this._ServerPort = value;
					this.SendPropertyChanged("ServerPort");
					this.OnServerPortChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DBName", DbType="NVarChar(100)")]
		public string DBName
		{
			get
			{
				return this._DBName;
			}
			set
			{
				if ((this._DBName != value))
				{
					this.OnDBNameChanging(value);
					this.SendPropertyChanging();
					this._DBName = value;
					this.SendPropertyChanged("DBName");
					this.OnDBNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ApplicationName", DbType="NVarChar(100)")]
		public string ApplicationName
		{
			get
			{
				return this._ApplicationName;
			}
			set
			{
				if ((this._ApplicationName != value))
				{
					this.OnApplicationNameChanging(value);
					this.SendPropertyChanging();
					this._ApplicationName = value;
					this.SendPropertyChanged("ApplicationName");
					this.OnApplicationNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserName", DbType="NVarChar(50)")]
		public string UserName
		{
			get
			{
				return this._UserName;
			}
			set
			{
				if ((this._UserName != value))
				{
					this.OnUserNameChanging(value);
					this.SendPropertyChanging();
					this._UserName = value;
					this.SendPropertyChanged("UserName");
					this.OnUserNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserPwd", DbType="NVarChar(50)")]
		public string UserPwd
		{
			get
			{
				return this._UserPwd;
			}
			set
			{
				if ((this._UserPwd != value))
				{
					this.OnUserPwdChanging(value);
					this.SendPropertyChanging();
					this._UserPwd = value;
					this.SendPropertyChanged("UserPwd");
					this.OnUserPwdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Remark", DbType="NVarChar(MAX)")]
		public string Remark
		{
			get
			{
				return this._Remark;
			}
			set
			{
				if ((this._Remark != value))
				{
					this.OnRemarkChanging(value);
					this.SendPropertyChanging();
					this._Remark = value;
					this.SendPropertyChanged("Remark");
					this.OnRemarkChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProjVersion", DbType="NVarChar(10)")]
		public string ProjVersion
		{
			get
			{
				return this._ProjVersion;
			}
			set
			{
				if ((this._ProjVersion != value))
				{
					this.OnProjVersionChanging(value);
					this.SendPropertyChanging();
					this._ProjVersion = value;
					this.SendPropertyChanged("ProjVersion");
					this.OnProjVersionChanged();
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
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.FeedBackLog")]
	public partial class FeedBackLog : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _FeedBackLogID;
		
		private System.Nullable<System.DateTime> _FeedBackDate;
		
		private string _CstName;
		
		private string _TaskNo;
		
		private string _FeedBackContent;
		
		private string _CsZrr;
		
		private string _CsYzResult;
		
		private string _IsKfCl;
		
		private string _KfZrr;
		
		private System.Nullable<System.DateTime> _EndDate;
		
		private string _KfClDate;
		
		private string _Wtyy;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnFeedBackLogIDChanging(int value);
    partial void OnFeedBackLogIDChanged();
    partial void OnFeedBackDateChanging(System.Nullable<System.DateTime> value);
    partial void OnFeedBackDateChanged();
    partial void OnCstNameChanging(string value);
    partial void OnCstNameChanged();
    partial void OnTaskNoChanging(string value);
    partial void OnTaskNoChanged();
    partial void OnFeedBackContentChanging(string value);
    partial void OnFeedBackContentChanged();
    partial void OnCsZrrChanging(string value);
    partial void OnCsZrrChanged();
    partial void OnCsYzResultChanging(string value);
    partial void OnCsYzResultChanged();
    partial void OnIsKfClChanging(string value);
    partial void OnIsKfClChanged();
    partial void OnKfZrrChanging(string value);
    partial void OnKfZrrChanged();
    partial void OnEndDateChanging(System.Nullable<System.DateTime> value);
    partial void OnEndDateChanged();
    partial void OnKfClDateChanging(string value);
    partial void OnKfClDateChanged();
    partial void OnWtyyChanging(string value);
    partial void OnWtyyChanged();
    #endregion
		
		public FeedBackLog()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FeedBackLogID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int FeedBackLogID
		{
			get
			{
				return this._FeedBackLogID;
			}
			set
			{
				if ((this._FeedBackLogID != value))
				{
					this.OnFeedBackLogIDChanging(value);
					this.SendPropertyChanging();
					this._FeedBackLogID = value;
					this.SendPropertyChanged("FeedBackLogID");
					this.OnFeedBackLogIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FeedBackDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> FeedBackDate
		{
			get
			{
				return this._FeedBackDate;
			}
			set
			{
				if ((this._FeedBackDate != value))
				{
					this.OnFeedBackDateChanging(value);
					this.SendPropertyChanging();
					this._FeedBackDate = value;
					this.SendPropertyChanged("FeedBackDate");
					this.OnFeedBackDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CstName", DbType="NVarChar(20)")]
		public string CstName
		{
			get
			{
				return this._CstName;
			}
			set
			{
				if ((this._CstName != value))
				{
					this.OnCstNameChanging(value);
					this.SendPropertyChanging();
					this._CstName = value;
					this.SendPropertyChanged("CstName");
					this.OnCstNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TaskNo", DbType="NVarChar(20)")]
		public string TaskNo
		{
			get
			{
				return this._TaskNo;
			}
			set
			{
				if ((this._TaskNo != value))
				{
					this.OnTaskNoChanging(value);
					this.SendPropertyChanging();
					this._TaskNo = value;
					this.SendPropertyChanged("TaskNo");
					this.OnTaskNoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FeedBackContent", DbType="NVarChar(MAX)")]
		public string FeedBackContent
		{
			get
			{
				return this._FeedBackContent;
			}
			set
			{
				if ((this._FeedBackContent != value))
				{
					this.OnFeedBackContentChanging(value);
					this.SendPropertyChanging();
					this._FeedBackContent = value;
					this.SendPropertyChanged("FeedBackContent");
					this.OnFeedBackContentChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CsZrr", DbType="NVarChar(20)")]
		public string CsZrr
		{
			get
			{
				return this._CsZrr;
			}
			set
			{
				if ((this._CsZrr != value))
				{
					this.OnCsZrrChanging(value);
					this.SendPropertyChanging();
					this._CsZrr = value;
					this.SendPropertyChanged("CsZrr");
					this.OnCsZrrChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CsYzResult", DbType="NVarChar(MAX)")]
		public string CsYzResult
		{
			get
			{
				return this._CsYzResult;
			}
			set
			{
				if ((this._CsYzResult != value))
				{
					this.OnCsYzResultChanging(value);
					this.SendPropertyChanging();
					this._CsYzResult = value;
					this.SendPropertyChanged("CsYzResult");
					this.OnCsYzResultChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsKfCl", DbType="NVarChar(10)")]
		public string IsKfCl
		{
			get
			{
				return this._IsKfCl;
			}
			set
			{
				if ((this._IsKfCl != value))
				{
					this.OnIsKfClChanging(value);
					this.SendPropertyChanging();
					this._IsKfCl = value;
					this.SendPropertyChanged("IsKfCl");
					this.OnIsKfClChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_KfZrr", DbType="NVarChar(20)")]
		public string KfZrr
		{
			get
			{
				return this._KfZrr;
			}
			set
			{
				if ((this._KfZrr != value))
				{
					this.OnKfZrrChanging(value);
					this.SendPropertyChanging();
					this._KfZrr = value;
					this.SendPropertyChanged("KfZrr");
					this.OnKfZrrChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EndDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> EndDate
		{
			get
			{
				return this._EndDate;
			}
			set
			{
				if ((this._EndDate != value))
				{
					this.OnEndDateChanging(value);
					this.SendPropertyChanging();
					this._EndDate = value;
					this.SendPropertyChanged("EndDate");
					this.OnEndDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_KfClDate", DbType="NVarChar(10)")]
		public string KfClDate
		{
			get
			{
				return this._KfClDate;
			}
			set
			{
				if ((this._KfClDate != value))
				{
					this.OnKfClDateChanging(value);
					this.SendPropertyChanging();
					this._KfClDate = value;
					this.SendPropertyChanged("KfClDate");
					this.OnKfClDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Wtyy", DbType="NVarChar(MAX)")]
		public string Wtyy
		{
			get
			{
				return this._Wtyy;
			}
			set
			{
				if ((this._Wtyy != value))
				{
					this.OnWtyyChanging(value);
					this.SendPropertyChanging();
					this._Wtyy = value;
					this.SendPropertyChanged("Wtyy");
					this.OnWtyyChanged();
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
	}
}
#pragma warning restore 1591
