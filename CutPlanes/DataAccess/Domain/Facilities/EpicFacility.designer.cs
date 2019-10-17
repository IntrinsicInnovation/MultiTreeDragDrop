﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EpicDataAccess.Domain.Facilities
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="EpicDevNew")]
	public partial class EpicFacilityDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertFacility(Facility instance);
    partial void UpdateFacility(Facility instance);
    partial void DeleteFacility(Facility instance);
    partial void InsertFacilityType(FacilityType instance);
    partial void UpdateFacilityType(FacilityType instance);
    partial void DeleteFacilityType(FacilityType instance);
    partial void InsertLinkFacilityTypeFaclity(LinkFacilityTypeFaclity instance);
    partial void UpdateLinkFacilityTypeFaclity(LinkFacilityTypeFaclity instance);
    partial void DeleteLinkFacilityTypeFaclity(LinkFacilityTypeFaclity instance);
    partial void InsertFacilityStatuse(FacilityStatuse instance);
    partial void UpdateFacilityStatuse(FacilityStatuse instance);
    partial void DeleteFacilityStatuse(FacilityStatuse instance);
    #endregion
		
		public EpicFacilityDataContext() : 
				base(global::EpicDataAccess.Properties.Settings.Default.EpicDevNewConnectionString1, mappingSource)
		{
			OnCreated();
		}
		
		public EpicFacilityDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public EpicFacilityDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public EpicFacilityDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public EpicFacilityDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Facility> Facilities
		{
			get
			{
				return this.GetTable<Facility>();
			}
		}
		
		public System.Data.Linq.Table<FacilityType> FacilityTypes
		{
			get
			{
				return this.GetTable<FacilityType>();
			}
		}
		
		public System.Data.Linq.Table<LinkFacilityTypeFaclity> LinkFacilityTypeFaclities
		{
			get
			{
				return this.GetTable<LinkFacilityTypeFaclity>();
			}
		}
		
		public System.Data.Linq.Table<FacilityStatuse> FacilityStatuses
		{
			get
			{
				return this.GetTable<FacilityStatuse>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="lookup.Facilities")]
	public partial class Facility : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _FacilityCode;
		
		private string _FacilityName;
		
		private string _PreviousName;
		
		private string _LocationNumber;
		
		private string _Address;
		
		private string _GeneralComments;
		
		private string _FacilityComments;
		
		private string _Headquarters;
		
		private string _Owner;
		
		private System.Nullable<int> _StatusId;
		
		private System.Nullable<System.DateTime> _StatusDate;
		
		private int _UserCreatedId;
		
		private System.DateTime _DateCreated;
		
		private System.Nullable<int> _UserUpdatedId;
		
		private System.Nullable<System.DateTime> _DateUpdated;
		
		private EntityRef<FacilityStatuse> _FacilityStatuse;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnFacilityCodeChanging(string value);
    partial void OnFacilityCodeChanged();
    partial void OnFacilityNameChanging(string value);
    partial void OnFacilityNameChanged();
    partial void OnPreviousNameChanging(string value);
    partial void OnPreviousNameChanged();
    partial void OnLocationNumberChanging(string value);
    partial void OnLocationNumberChanged();
    partial void OnAddressChanging(string value);
    partial void OnAddressChanged();
    partial void OnGeneralCommentsChanging(string value);
    partial void OnGeneralCommentsChanged();
    partial void OnFacilityCommentsChanging(string value);
    partial void OnFacilityCommentsChanged();
    partial void OnHeadquartersChanging(string value);
    partial void OnHeadquartersChanged();
    partial void OnOwnerChanging(string value);
    partial void OnOwnerChanged();
    partial void OnStatusIdChanging(System.Nullable<int> value);
    partial void OnStatusIdChanged();
    partial void OnStatusDateChanging(System.Nullable<System.DateTime> value);
    partial void OnStatusDateChanged();
    partial void OnUserCreatedIdChanging(int value);
    partial void OnUserCreatedIdChanged();
    partial void OnDateCreatedChanging(System.DateTime value);
    partial void OnDateCreatedChanged();
    partial void OnUserUpdatedIdChanging(System.Nullable<int> value);
    partial void OnUserUpdatedIdChanged();
    partial void OnDateUpdatedChanging(System.Nullable<System.DateTime> value);
    partial void OnDateUpdatedChanged();
    #endregion
		
		public Facility()
		{
			this._FacilityStatuse = default(EntityRef<FacilityStatuse>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FacilityCode", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
		public string FacilityCode
		{
			get
			{
				return this._FacilityCode;
			}
			set
			{
				if ((this._FacilityCode != value))
				{
					this.OnFacilityCodeChanging(value);
					this.SendPropertyChanging();
					this._FacilityCode = value;
					this.SendPropertyChanged("FacilityCode");
					this.OnFacilityCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FacilityName", DbType="NVarChar(100)")]
		public string FacilityName
		{
			get
			{
				return this._FacilityName;
			}
			set
			{
				if ((this._FacilityName != value))
				{
					this.OnFacilityNameChanging(value);
					this.SendPropertyChanging();
					this._FacilityName = value;
					this.SendPropertyChanged("FacilityName");
					this.OnFacilityNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PreviousName", DbType="NVarChar(MAX)")]
		public string PreviousName
		{
			get
			{
				return this._PreviousName;
			}
			set
			{
				if ((this._PreviousName != value))
				{
					this.OnPreviousNameChanging(value);
					this.SendPropertyChanging();
					this._PreviousName = value;
					this.SendPropertyChanged("PreviousName");
					this.OnPreviousNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LocationNumber", DbType="NVarChar(10)")]
		public string LocationNumber
		{
			get
			{
				return this._LocationNumber;
			}
			set
			{
				if ((this._LocationNumber != value))
				{
					this.OnLocationNumberChanging(value);
					this.SendPropertyChanging();
					this._LocationNumber = value;
					this.SendPropertyChanged("LocationNumber");
					this.OnLocationNumberChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Address", DbType="NVarChar(MAX)")]
		public string Address
		{
			get
			{
				return this._Address;
			}
			set
			{
				if ((this._Address != value))
				{
					this.OnAddressChanging(value);
					this.SendPropertyChanging();
					this._Address = value;
					this.SendPropertyChanged("Address");
					this.OnAddressChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GeneralComments", DbType="NVarChar(MAX)")]
		public string GeneralComments
		{
			get
			{
				return this._GeneralComments;
			}
			set
			{
				if ((this._GeneralComments != value))
				{
					this.OnGeneralCommentsChanging(value);
					this.SendPropertyChanging();
					this._GeneralComments = value;
					this.SendPropertyChanged("GeneralComments");
					this.OnGeneralCommentsChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FacilityComments", DbType="NVarChar(MAX)")]
		public string FacilityComments
		{
			get
			{
				return this._FacilityComments;
			}
			set
			{
				if ((this._FacilityComments != value))
				{
					this.OnFacilityCommentsChanging(value);
					this.SendPropertyChanging();
					this._FacilityComments = value;
					this.SendPropertyChanged("FacilityComments");
					this.OnFacilityCommentsChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Headquarters", DbType="NVarChar(10)")]
		public string Headquarters
		{
			get
			{
				return this._Headquarters;
			}
			set
			{
				if ((this._Headquarters != value))
				{
					this.OnHeadquartersChanging(value);
					this.SendPropertyChanging();
					this._Headquarters = value;
					this.SendPropertyChanged("Headquarters");
					this.OnHeadquartersChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Owner", DbType="NVarChar(100)")]
		public string Owner
		{
			get
			{
				return this._Owner;
			}
			set
			{
				if ((this._Owner != value))
				{
					this.OnOwnerChanging(value);
					this.SendPropertyChanging();
					this._Owner = value;
					this.SendPropertyChanged("Owner");
					this.OnOwnerChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StatusId", DbType="Int")]
		public System.Nullable<int> StatusId
		{
			get
			{
				return this._StatusId;
			}
			set
			{
				if ((this._StatusId != value))
				{
					if (this._FacilityStatuse.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnStatusIdChanging(value);
					this.SendPropertyChanging();
					this._StatusId = value;
					this.SendPropertyChanged("StatusId");
					this.OnStatusIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StatusDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> StatusDate
		{
			get
			{
				return this._StatusDate;
			}
			set
			{
				if ((this._StatusDate != value))
				{
					this.OnStatusDateChanging(value);
					this.SendPropertyChanging();
					this._StatusDate = value;
					this.SendPropertyChanged("StatusDate");
					this.OnStatusDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserCreatedId", DbType="Int NOT NULL")]
		public int UserCreatedId
		{
			get
			{
				return this._UserCreatedId;
			}
			set
			{
				if ((this._UserCreatedId != value))
				{
					this.OnUserCreatedIdChanging(value);
					this.SendPropertyChanging();
					this._UserCreatedId = value;
					this.SendPropertyChanged("UserCreatedId");
					this.OnUserCreatedIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DateCreated", DbType="DateTime NOT NULL")]
		public System.DateTime DateCreated
		{
			get
			{
				return this._DateCreated;
			}
			set
			{
				if ((this._DateCreated != value))
				{
					this.OnDateCreatedChanging(value);
					this.SendPropertyChanging();
					this._DateCreated = value;
					this.SendPropertyChanged("DateCreated");
					this.OnDateCreatedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserUpdatedId", DbType="Int")]
		public System.Nullable<int> UserUpdatedId
		{
			get
			{
				return this._UserUpdatedId;
			}
			set
			{
				if ((this._UserUpdatedId != value))
				{
					this.OnUserUpdatedIdChanging(value);
					this.SendPropertyChanging();
					this._UserUpdatedId = value;
					this.SendPropertyChanged("UserUpdatedId");
					this.OnUserUpdatedIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DateUpdated", DbType="DateTime")]
		public System.Nullable<System.DateTime> DateUpdated
		{
			get
			{
				return this._DateUpdated;
			}
			set
			{
				if ((this._DateUpdated != value))
				{
					this.OnDateUpdatedChanging(value);
					this.SendPropertyChanging();
					this._DateUpdated = value;
					this.SendPropertyChanged("DateUpdated");
					this.OnDateUpdatedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="FacilityStatuse_Facility", Storage="_FacilityStatuse", ThisKey="StatusId", OtherKey="Id", IsForeignKey=true)]
		public FacilityStatuse FacilityStatuse
		{
			get
			{
				return this._FacilityStatuse.Entity;
			}
			set
			{
				FacilityStatuse previousValue = this._FacilityStatuse.Entity;
				if (((previousValue != value) 
							|| (this._FacilityStatuse.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._FacilityStatuse.Entity = null;
						previousValue.Facilities.Remove(this);
					}
					this._FacilityStatuse.Entity = value;
					if ((value != null))
					{
						value.Facilities.Add(this);
						this._StatusId = value.Id;
					}
					else
					{
						this._StatusId = default(Nullable<int>);
					}
					this.SendPropertyChanged("FacilityStatuse");
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="lookup.FacilityType")]
	public partial class FacilityType : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ID;
		
		private string _Name;
		
		private string _Description;
		
		private string _Comments;
		
		private System.DateTime _DateCreated;
		
		private string _UserCreatedId;
		
		private System.Nullable<System.DateTime> _DateUpdated;
		
		private string _UserUpdatedId;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(int value);
    partial void OnIDChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnDescriptionChanging(string value);
    partial void OnDescriptionChanged();
    partial void OnCommentsChanging(string value);
    partial void OnCommentsChanged();
    partial void OnDateCreatedChanging(System.DateTime value);
    partial void OnDateCreatedChanged();
    partial void OnUserCreatedIdChanging(string value);
    partial void OnUserCreatedIdChanged();
    partial void OnDateUpdatedChanging(System.Nullable<System.DateTime> value);
    partial void OnDateUpdatedChanged();
    partial void OnUserUpdatedIdChanging(string value);
    partial void OnUserUpdatedIdChanged();
    #endregion
		
		public FacilityType()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Description", DbType="NVarChar(100)")]
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Comments", DbType="NVarChar(MAX)")]
		public string Comments
		{
			get
			{
				return this._Comments;
			}
			set
			{
				if ((this._Comments != value))
				{
					this.OnCommentsChanging(value);
					this.SendPropertyChanging();
					this._Comments = value;
					this.SendPropertyChanged("Comments");
					this.OnCommentsChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DateCreated", DbType="DateTime NOT NULL")]
		public System.DateTime DateCreated
		{
			get
			{
				return this._DateCreated;
			}
			set
			{
				if ((this._DateCreated != value))
				{
					this.OnDateCreatedChanging(value);
					this.SendPropertyChanging();
					this._DateCreated = value;
					this.SendPropertyChanged("DateCreated");
					this.OnDateCreatedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserCreatedId", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string UserCreatedId
		{
			get
			{
				return this._UserCreatedId;
			}
			set
			{
				if ((this._UserCreatedId != value))
				{
					this.OnUserCreatedIdChanging(value);
					this.SendPropertyChanging();
					this._UserCreatedId = value;
					this.SendPropertyChanged("UserCreatedId");
					this.OnUserCreatedIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DateUpdated", DbType="DateTime")]
		public System.Nullable<System.DateTime> DateUpdated
		{
			get
			{
				return this._DateUpdated;
			}
			set
			{
				if ((this._DateUpdated != value))
				{
					this.OnDateUpdatedChanging(value);
					this.SendPropertyChanging();
					this._DateUpdated = value;
					this.SendPropertyChanged("DateUpdated");
					this.OnDateUpdatedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserUpdatedId", DbType="NVarChar(100)")]
		public string UserUpdatedId
		{
			get
			{
				return this._UserUpdatedId;
			}
			set
			{
				if ((this._UserUpdatedId != value))
				{
					this.OnUserUpdatedIdChanging(value);
					this.SendPropertyChanging();
					this._UserUpdatedId = value;
					this.SendPropertyChanged("UserUpdatedId");
					this.OnUserUpdatedIdChanged();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="link.LinkFacilityTypeFaclities")]
	public partial class LinkFacilityTypeFaclity : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private int _FacilityId;
		
		private int _FacilityTypeId;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnFacilityIdChanging(int value);
    partial void OnFacilityIdChanged();
    partial void OnFacilityTypeIdChanging(int value);
    partial void OnFacilityTypeIdChanged();
    #endregion
		
		public LinkFacilityTypeFaclity()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FacilityId", DbType="Int NOT NULL")]
		public int FacilityId
		{
			get
			{
				return this._FacilityId;
			}
			set
			{
				if ((this._FacilityId != value))
				{
					this.OnFacilityIdChanging(value);
					this.SendPropertyChanging();
					this._FacilityId = value;
					this.SendPropertyChanged("FacilityId");
					this.OnFacilityIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FacilityTypeId", DbType="Int NOT NULL")]
		public int FacilityTypeId
		{
			get
			{
				return this._FacilityTypeId;
			}
			set
			{
				if ((this._FacilityTypeId != value))
				{
					this.OnFacilityTypeIdChanging(value);
					this.SendPropertyChanging();
					this._FacilityTypeId = value;
					this.SendPropertyChanged("FacilityTypeId");
					this.OnFacilityTypeIdChanged();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="lookup.FacilityStatuses")]
	public partial class FacilityStatuse : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Value;
		
		private int _UserCreatedId;
		
		private System.DateTime _DateCreated;
		
		private System.Nullable<int> _UserUpdatedId;
		
		private System.Nullable<System.DateTime> _DateUpdated;
		
		private string _Name;
		
		private EntitySet<Facility> _Facilities;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnValueChanging(string value);
    partial void OnValueChanged();
    partial void OnUserCreatedIdChanging(int value);
    partial void OnUserCreatedIdChanged();
    partial void OnDateCreatedChanging(System.DateTime value);
    partial void OnDateCreatedChanged();
    partial void OnUserUpdatedIdChanging(System.Nullable<int> value);
    partial void OnUserUpdatedIdChanged();
    partial void OnDateUpdatedChanging(System.Nullable<System.DateTime> value);
    partial void OnDateUpdatedChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    #endregion
		
		public FacilityStatuse()
		{
			this._Facilities = new EntitySet<Facility>(new Action<Facility>(this.attach_Facilities), new Action<Facility>(this.detach_Facilities));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Value", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				if ((this._Value != value))
				{
					this.OnValueChanging(value);
					this.SendPropertyChanging();
					this._Value = value;
					this.SendPropertyChanged("Value");
					this.OnValueChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserCreatedId", DbType="Int NOT NULL")]
		public int UserCreatedId
		{
			get
			{
				return this._UserCreatedId;
			}
			set
			{
				if ((this._UserCreatedId != value))
				{
					this.OnUserCreatedIdChanging(value);
					this.SendPropertyChanging();
					this._UserCreatedId = value;
					this.SendPropertyChanged("UserCreatedId");
					this.OnUserCreatedIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DateCreated", DbType="Date NOT NULL")]
		public System.DateTime DateCreated
		{
			get
			{
				return this._DateCreated;
			}
			set
			{
				if ((this._DateCreated != value))
				{
					this.OnDateCreatedChanging(value);
					this.SendPropertyChanging();
					this._DateCreated = value;
					this.SendPropertyChanged("DateCreated");
					this.OnDateCreatedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserUpdatedId", DbType="Int")]
		public System.Nullable<int> UserUpdatedId
		{
			get
			{
				return this._UserUpdatedId;
			}
			set
			{
				if ((this._UserUpdatedId != value))
				{
					this.OnUserUpdatedIdChanging(value);
					this.SendPropertyChanging();
					this._UserUpdatedId = value;
					this.SendPropertyChanged("UserUpdatedId");
					this.OnUserUpdatedIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DateUpdated", DbType="Date")]
		public System.Nullable<System.DateTime> DateUpdated
		{
			get
			{
				return this._DateUpdated;
			}
			set
			{
				if ((this._DateUpdated != value))
				{
					this.OnDateUpdatedChanging(value);
					this.SendPropertyChanging();
					this._DateUpdated = value;
					this.SendPropertyChanged("DateUpdated");
					this.OnDateUpdatedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
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
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="FacilityStatuse_Facility", Storage="_Facilities", ThisKey="Id", OtherKey="StatusId")]
		public EntitySet<Facility> Facilities
		{
			get
			{
				return this._Facilities;
			}
			set
			{
				this._Facilities.Assign(value);
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
		
		private void attach_Facilities(Facility entity)
		{
			this.SendPropertyChanging();
			entity.FacilityStatuse = this;
		}
		
		private void detach_Facilities(Facility entity)
		{
			this.SendPropertyChanging();
			entity.FacilityStatuse = null;
		}
	}
}
#pragma warning restore 1591