using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpicDataAccess.Domain;

namespace EpicDataAccess.Domain
{ 
    public partial class TableHistoryResult //: esult> // ISearchResult, IEquatable<SearchResult>
    {
        public long Id { get; set; }
        public string TableName { get; set; }
        public int? ChangedRowId { get; set; }
        public string UserChangedByLoginName { get; set; }
        public string ColumnName { get; set; }
        public string NewValue { get; set; }
        public string OldValue { get; set; }
        public DateTime ChangedDateTime { get; set; }
        public byte IsDeleted { get; set; }

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

        //private string _navigationUrl = string.Empty;
        //public string NavigationUrl
        //{
        //    get
        //    {
        //        return string.Format("/Pages", this.TableName, this.Id);
        //    }
        //}

        //public bool Equals(SearchResult other)
        //{

        //    //Check whether the compared object is null.
        //    if (Object.ReferenceEquals(other, null)) return false;

        //    //Check whether the compared object references the same data.
        //    if (Object.ReferenceEquals(this, other)) return true;

        //    //Check whether the products' properties are equal.
        //    return ID.Equals(other.ID) && ShortName.Equals(other.ShortName);
        //}

        //// If Equals() returns true for a pair of objects 
        //// then GetHashCode() must return the same value for these objects.

        //public override int GetHashCode()
        //{

        //    //Get hash code for the Name field if it is not null.
        //    int hashSearchResultName = ShortName == null ? 0 : ShortName.GetHashCode();

        //    //Get hash code for the Code field.
        //    int hashSearchResultID = ID.GetHashCode();

        //    //Calculate the hash code for the product.
        //    return hashSearchResultName ^ hashSearchResultID;
        //}

    }
}
