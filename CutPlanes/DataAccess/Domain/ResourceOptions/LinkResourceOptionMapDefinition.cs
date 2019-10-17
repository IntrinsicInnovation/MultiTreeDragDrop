using System;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain.ResourceOptions
{
    public partial class LinkResourceOptionMapDefinition : Domain.ILinkToMap
    {
        /// <summary>
        /// ResourceOptionId
        /// </summary>
        public int EntityId
        {
            get { return this.ResourceOptionId; }
            set { this.ResourceOptionId = value; }
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
                return this.EGrps.ElectricalGroupLinkedToMap(this.ElectricalGroupId, m => m.IsActive.HasValue && m.IsActive.Value && m.MapType.Code == "ELEC");
            }
        }

        internal bool LinkedToMap(int mapId)
        {
            return this.EGrps.ElectricalGroupLinkedToMap(this.ElectricalGroupId, m => m.Id == mapId);
        }
    }
}
