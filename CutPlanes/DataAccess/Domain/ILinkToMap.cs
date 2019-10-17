using System;

namespace EpicDataAccess.Domain
{
    /// <summary>
    /// To be inherited by any class which is based on a Link / Join Table joining Electrical Group and another entity (Substation, Resource Option, Resource Bundle, etc)
    /// </summary>
    public interface ILinkToMap
    {
        #region [Properties]

        int ElectricalGroupId { get; set; }
        /// <summary>
        /// SubstationId, ResourceOptionId, ResourceBundleId, etc...
        /// </summary>
        int EntityId { get; set; }

        bool IsBCEGLink { get; }        

        #endregion
    }
}
