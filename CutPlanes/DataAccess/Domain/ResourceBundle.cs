using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public partial class ResourceBundle : IResource
    {
        #region [Properties]

        private string _name = string.Empty;
        public string Name
        {
            get
            {
                if(string.IsNullOrEmpty(_name))
                {
                    var name = this.Names.FirstOrDefault(n => n.NameType.Name.Equals("Long Name") && n.IsActive.Equals(true));
                    _name = name == null ? string.Empty : name.Name1;
                }
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private string _tla = string.Empty;
        public string TLA
        {
            get
            {
                if (string.IsNullOrEmpty(_tla))
                {
                    var name = this.Names.FirstOrDefault(n => n.NameType.Name.ToLower().Equals("tla") && n.IsActive.Equals(true));
                    _tla = name == null ? string.Empty : name.Name1;
                }
                return _tla;
            }
            set { _tla = value; }
        }

        public DateTime DateModified
        {
            get { return this.DateUpdated.HasValue ? this.DateUpdated.Value : this.DateCreated; }
        }

        public string TypeString
        {
            get { return typeof(ResourceBundle).Name; }
        }

        public string BcegCode
        {
            get
            {
                if (this.BCEG != null) return this.BCEG.Abbreviation;
                return string.Empty;
            }
        }

        public int? BcegId
        {
            get
            {
                if (this.BCEG != null) return this.BCEG.Id;
                return null;
            }
        }

        private ElectricalGroup _bCEG = null;
        internal ElectricalGroup BCEG
        {
            get
            {
                if (_bCEG == null)
                {
                    var link = this.LinkResourceBundleElectricalGroups.FirstOrDefault(li => li.IsBCEGLink == true);
                    _bCEG = link != null ? link.ElectricalGroup : null;
                }
                return _bCEG;
            }
        }

        private ElectricalGroup _ltap = null;
        public ElectricalGroup Ltap
        {
            get
            {
                if (_ltap == null)
                {
                    var link = this.LinkResourceBundleElectricalGroups.FirstOrDefault(li => li.IsLTAPLink == true);
                    _ltap = link != null ? link.ElectricalGroup : null;
                }
                return _ltap;
            }
        }        

        public string ResourceIdentifer
        {
            get { return this.TLA; }
        }

        public string DateLastModifiedString
        {
            get { return this.DateUpdated.HasValue ? this.DateUpdated.Value.ToString("yyyy-MM-dd") : this.DateCreated.ToString("yyyy-MM-dd"); }
        }

        public string NavigationUrl
        {
            get { return string.Format("/Pages/Data/ResourceBundleNavigation.aspx?rbid={0}", this.Id); }
        }

        public decimal? Voltage
        {
            get { return null; }
        }

        public bool? IsNew { get; set; }

        #endregion
    }
}
