using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace EpicDataAccess.Domain.Facilities
{
    public class FacilityLite
    {
        public FacilityLite(int id, string code, string name, string type, string electricalGroupCode, DateTime dateCreated, DateTime? dateModified)
        {
            this.Id = id.ToString();
            this.Code = code;
            this.Name = name;
            this.Type = type;
            this.ElectricalGroupCode = string.IsNullOrEmpty(electricalGroupCode) ? string.Empty : electricalGroupCode;
            this.DateModified = dateModified.HasValue ? dateModified.Value : dateCreated;
            this.DateModifiedString = this.DateModified.ToString("dd/MM/yyyy");
        }

        public FacilityLite() { }


        #region [Properties]

        public string Id { get; set; }
        /// <summary>
        /// Faciltiy Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Faciltiy Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Facility Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Electrical Group Code
        /// </summary>
        public string ElectricalGroupCode { get; set; }
        /// <summary>
        /// Date facility was last modified.
        /// </summary>
        public string DateModifiedString { get; set; }
        
        
        /// <summary>
        /// Date facility was last modified.
        /// Used to sort
        /// </summary>
        [ScriptIgnore]
        public DateTime DateModified { get; set; }

        #endregion
    }
}
