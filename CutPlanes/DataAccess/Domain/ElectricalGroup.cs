using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public partial class ElectricalGroup
    {
        /// <summary>
        /// A list of ElectricalGroup objects which have a ParentPk equal to the Id of the current object
        /// </summary>
        public IList<ElectricalGroup> ChildGroups
        {
            get;
            set;
        }

        public DateTime DateLastModified
        {
            get
            {
                return this.DateUpdated.HasValue ? this.DateUpdated.Value : this.DateCreated;
            }
        }
    }
}
