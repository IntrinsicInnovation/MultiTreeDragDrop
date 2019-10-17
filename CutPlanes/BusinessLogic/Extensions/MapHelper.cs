using System;
using System.Collections.Generic;
//using EpicDataAccess.Domain;

namespace EpicBusinessLogic.Extensions
{
    public static class MapHelper
    {
        /// <summary>
        /// Returns the first ILinkToMap instance object connected to the input mapId
        /// </summary>
        /// <param name="value">any collection of objects which are derivitive of ILinkToMap</param>
        /// <param name="mapId">map db idenity</param>
        /// <returns>ILinkToMap instance object if match is found.  default = null</returns>
        //public static ILinkToMap FirstInMapOrDefault(this IEnumerable<ILinkToMap> value, int mapId)
        //{              
        //    var eGMngr = new EpicBusinessLogic.Domain.ElectricalGroups.EpicElectricalGroupsManager();
        //    var map = eGMngr.GetMap(mapId);
        //    foreach (var link in value)
        //    {
        //        if (map.MapDefinitionRootId == link.ElectricalGroupId) return link;
        //        var eG = eGMngr.GetElectricalGroup(link.ElectricalGroupId, false);
        //        while (eG.ParentPk.HasValue)
        //        {
        //            eG = eGMngr.GetElectricalGroup(eG.ParentPk.Value, false);
        //            if (map.MapDefinitionRootId == eG.Id) return link;
        //        }
        //    }
        //    return null;
        //}
    }
}
