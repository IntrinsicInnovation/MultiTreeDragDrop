using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpicDataAccess.Domain;

namespace EpicDataAccess.Domain.ResourceOptions
{
    public partial class ResourceOption : IResource
    {
        #region [Properties]

        public DateTime DateModified
        {
            get { return this.DateUpdated.HasValue ? this.DateUpdated.Value : this.DateCreated; }
        }

        public string Region
        {
            get
            {
                var link = this.LinkResourceOptionMapDefinitions.FirstOrDefault();
                if (link == null) return string.Empty;
                return link.ElectricalGroup.Abbreviation;
            }
        }

        

        public string ResourceIdentifer
        {
            get { return string.Format("RO-{0}", this.Id); }
        }

        public string DateLastModifiedString
        {
            get { return this.DateUpdated.HasValue ? this.DateUpdated.Value.ToString("yyyy-MM-dd") : this.DateCreated.ToString("yyyy-MM-dd"); }
        }

        public string TypeString
        {
            get { return typeof(ResourceOption).Name; }
        }

        public string NavigationUrl
        {
            get { return string.Format("/Pages/Data/ResourceOptionNavigation.aspx?roid={0}", this.Id); }
        }

        public decimal? Voltage
        {
            get { return null; }
        }

        #endregion
    }
}
