using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public partial class Load : IResource
    {
        private string _name = string.Empty;
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    var name = this.Names.FirstOrDefault(n => n.NameType.Name.Equals("Long Name"));
                    _name = name != null ? name.Name1 : string.Empty;
                }
                return _name;
            }
            set { _name = value; }
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
            get { return typeof(Load).Name; }
        }

     
        public string NavigationUrl
        {
            get
            {
                return string.Format("/Pages/Data/LoadNavigation.aspx?loadid={0}", this.Id);
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
