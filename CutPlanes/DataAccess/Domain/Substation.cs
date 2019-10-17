using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpicDataAccess.Domain;

namespace EpicDataAccess.Domain
{
    public partial class Substation : IResource
    {
        #region [Properties]
        
        public string Name
        {
            get
            {
                var name = this.Names.FirstOrDefault(n => n.NameType.Name.Equals("Long Name") && n.IsActive.Equals(true));
                return name == null ? string.Empty : name.Name1;
            }
        }

        private string _tla = string.Empty;
        public string TLA
        {
            get
            {
                if(string.IsNullOrEmpty(_tla))
                {
                    var name = this.Names.FirstOrDefault(n => n.NameType.Name.Equals("TLA") && n.IsActive.Equals(true));
                    _tla = name == null ? string.Empty : name.Name1;
                }
                return _tla;
            }
            set { _tla = value; }
        }

        public IList<VoltageLevel> VoltageLevelList
        {
            get
            {
                if (this.VoltageLevels == null) return null;
                return this.VoltageLevels.Distinct().ToList();
            }
        }

        public string ResourceIdentifer
        {
            get { return this.TLA; }
        }
       
        public string NavigationUrl 
        { 
            get 
            {   
                return string.Format("/Pages/Data/SubstationNavigation.aspx?tla={0}&sid={1}", this.TLA, this.Id);                
            }           
        }

        public DateTime DateLastModified
        {
            get { return this.DateUpdated.HasValue ? this.DateUpdated.Value : this.DateCreated; }
        }

        public string DateLastModifiedString
        {
            get { return DateLastModified.ToString("yyyy-MM-dd"); }
        }

        public string TypeString
        {
            get { return this.SubstationType != null ? this.SubstationType.Value.Trim() : string.Empty; }
        }

        public string BcegCode
        {
            get
            {
                if (this.Bceg != null) return this.Bceg.Abbreviation;
                return string.Empty;
            }
        }

        public int? BcegId
        {
            get
            {
                if (this.Bceg != null) return this.Bceg.Id;
                return null;
            }
        }


        public string LtapCode
        {
            get
            {
                if (this.Ltap != null) return this.Ltap.Abbreviation;
                return string.Empty;
            }
        }

        public int? LtapId
        {
            get
            {
                if (this.Ltap != null) return this.Ltap.Id;
                return null;
            }
        }




        private ElectricalGroup _bceg = null;
        internal ElectricalGroup Bceg
        {
            get
            {
                if (_bceg == null)
                {                                //Should be BCEG
                                                                                //IsBCEGLink
                    var link = this.LinkSubstationElectricalGroups.Where(li => li.IsBCEGLink == true).FirstOrDefault();
                    _bceg = link != null ? link.ElectricalGroup : null;
                }
                return _bceg;
            }
        }

        private ElectricalGroup _ltap = null;
        public ElectricalGroup Ltap
        {
            get
            {
                if (_ltap == null)
                {
                    var link = this.LinkSubstationElectricalGroups.Where(li => li.IsLTAPLink == true).FirstOrDefault();
                    _ltap = link != null ? link.ElectricalGroup : null;
                }
                return _ltap;
            }
        }

        public int? MapId { get; set; }
        private int? mEGId = null;
        /// <summary>
        /// Returns the Electrical Group Id linked to Substation.MapId property (***MapId property must be set)
        /// </summary>
        public int? MapElectricalGroupId
        {
            get
            {
                if (!mEGId.HasValue)
                {
                    if (!this.MapId.HasValue) return null;
                    var link = this.LinkSubstationElectricalGroups.FirstOrDefault(li => li.LinkedToMap(this.MapId.Value));
                    if (link != null) mEGId = link.ElectricalGroupId;                    
                }
                return mEGId;
            }
        }

        public string OwnershipCode
        {
            get
            {
                if (this.OwnershipType == null) return string.Empty;
                if (this.OwnershipType.Value == "Heritage") return "BCH";
                else if (this.OwnershipType.Value.Contains("Fortis")) return "F";
                else return "NH"; // Non-Heritage
            }
        }

        public decimal? Voltage
        {
            get
            {
                return null;
            }
        }

        public bool? IsNew { get; set; }

        #endregion
        
    }
}
