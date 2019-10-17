using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public partial class LinkSubstationElectricalGroup : ILinkToMap
    {
        /// <summary>
        /// SubstationId
        /// </summary>
        public int EntityId
        {
            get { return this.SubstationId; }
            set { this.SubstationId = value; }
        }

        private EpicElectricalGroups _eGrps = null;
        private EpicElectricalGroups EGrps
        {
            get 
            {
                var cS = System.Configuration.ConfigurationManager.ConnectionStrings["EpicDB"].ConnectionString;
                if (_eGrps == null) _eGrps = new EpicElectricalGroups(cS); return _eGrps; 
            }
        }

       /// <summary>
       /// Determines whether the current Link object is connected to the BCEG Map
       /// </summary>
        public bool IsBCEGLink
        {
            get
            {               
                if (this.Map != null)
                    if (this.Map.MapType.Code == "ELEC")
                        return true;
                return false;
                //return this.EGrps.ElectricalGroupLinkedToMap(this.ElectricalGroupId, m => m.IsActive.HasValue && m.IsActive.Value && m.MapType.Code == "ELEC");  
            }
        }


        /// <summary>
        /// Determines whether the current Link object is connected to the LTAP Map
        /// </summary>
        public bool IsLTAPLink
        {
            get
            {
                if (this.Map != null)
                    if (this.Map.MapType.Code == "LTAP")
                        return true;
                return false;
            }
        }

        public int? MapId
        {
            get
            {
                int? id = null;
                if (this.Map != null)
                    id = this.Map.Id;
                return id;
            }
        }

        private Map _m = null;
        /// <summary>
        /// Returns the map to which current the Electrical Group in linked.
        /// </summary>
        internal Map Map
        {
            get
            {
                if (_m == null)                
                    _m = this.EGrps.GetEGMap(this.ElectricalGroupId);                
                return _m;
            }
        }

        /// <summary>
        /// Determines whether the current Link object is connected to the input map Id
        /// </summary>    
        internal bool LinkedToMap(int mapId)
        {
            return this.EGrps.ElectricalGroupLinkedToMap(this.ElectricalGroupId, m => m.Id == mapId);
        }
    }
}
