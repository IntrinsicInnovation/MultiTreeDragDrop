using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpicDataAccess.Domain;

namespace EpicDataAccess.Domain
{ 
    public partial class TableHistory : IResource // IEnumerable<SearchResult> // ISearchResult, IEquatable<SearchResult>
    {
       

        public string ResourceIdentifer
        {
            get { return this.TableName; }
        }

        public string DateLastModifiedString
        {
            get { return (this.ChangedDateTime.ToString("yyyy-MM-dd")); }
        }

        public string TypeString
        {
            get { 
                return typeof(TableHistory).Name;
            }
        }

        private string _navigationUrl = string.Empty;
        public string NavigationUrl
        {
            get
            {
                return string.Format("/Pages", this.TableName, this.Id);
            }
        }

        public decimal? Voltage
        {
            get
            {
                return null;
            }
        }

    }
}
