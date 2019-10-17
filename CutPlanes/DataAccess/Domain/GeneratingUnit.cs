using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpicDataAccess.Domain;

namespace EpicDataAccess.Domain
{
    public partial class GeneratingUnit : IResource
    {
        private string _name = string.Empty;
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    var name = this.Names.FirstOrDefault(n => n.NameType.Name.Equals("Long Name"));
                    _name = name == null ? string.Empty : name.Name1;
                }
                return _name;
            }
            set { _name = value; }
        }

        public string TLA
        {
            get
            {
                var name = this.Names.FirstOrDefault(n => n.NameType.Name.Equals("TLA"));
                return name == null ? string.Empty : name.Name1;
            }
        }
        
        public string ResourceIdentifer
        {
            get { return this.Name; }
        }

        public string DateLastModifiedString
        {
            get { return (this.DateUpdated.HasValue ? this.DateUpdated.Value : this.DateCreated).ToString("yyyy-MM-dd"); }
        }

        public string TypeString
        {
            get { return typeof(GeneratingUnit).Name; }
        }

        public string NavigationUrl
        {
            get
            {                
                return string.Format("/Pages/Data/GeneratingUnitNavigation.aspx?gid={0}", this.Id);
            }
        }

        public decimal? Voltage
        {
            get
            {
                return this.VoltageLevel != null ? this.VoltageLevel.NominalVoltage : null;
            }
        }

        public bool? IsNew { get; set; }
    }
}
