using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public partial class Line : IResource
    {
        public string ResourceIdentifer
        {
            get { return this.Name.ToString(); }
        }

        public string DateLastModifiedString
        {
            get { return this.DateCreated.ToString("yyyy-MM-dd"); }
        }

        public string TypeString
        {
            get { return typeof(Line).Name; }            
        }
        
     

        private string _navigationUrl = string.Empty;
        public string NavigationUrl
        {
            get
            {
                return string.Format("/Pages/Data/LineNavigation.aspx?lid={0}", this.Id);
            }
        }

        public decimal? Voltage
        {
            get
            {
                return this.BaseVoltage.Voltage;
            }
        }

    }
}
