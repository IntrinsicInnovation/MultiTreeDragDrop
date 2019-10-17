using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
        
    public class EpicElectricalGroups : IDisposable
    {
        #region [Properties]

        const string BCEG_MAP = "ELEC";
        const string LTAP_MAP = "LTAP";
        private string ConnectionString { get; set; }
        protected EpicDomainDataContext _context = null;
        protected EpicDomainDataContext Context
        {
            get
            {
                if (_context == null) _context = new EpicDomainDataContext(ConnectionString);
                return _context;
            }
        }

        #endregion

        #region [Constructor]

        public EpicElectricalGroups(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        #endregion

        #region [Functions]

        ///// <summary>
        ///// Deletes an existing ElectricalGroup object and all of its child objects
        ///// </summary>
        ///// <param name="electricalGroup">ElectricalGroup object to delete</param>
        //public void DeleteElectricalGroup(ElectricalGroup electricalGroup)
        //{
        //    Context.ElectricalGroups.DeleteOnSubmit(electricalGroup);
        //    Context.SubmitChanges();
        //}

        /// <summary>
        /// Deletes an existing ElectricalGroup object
        /// </summary>
        public void DeleteElectricalGroup(int key)
        {
            ElectricalGroup electricalGroup = Context.ElectricalGroups.FirstOrDefault(eg => eg.Id.Equals(key));
            Context.ElectricalGroups.DeleteOnSubmit(electricalGroup);
            Context.SubmitChanges();
        }

        /// <summary>
        /// Deletes an existing ElectricalGroup object
        /// </summary>
        /// <param name="electricalGroup">ElectricalGroup object to delete</param>
        public void DeleteElectricalGroup(ElectricalGroup electricalGroup)
        {
            if (electricalGroup == null) return;            
            Context.ElectricalGroups.DeleteOnSubmit(electricalGroup);
            Context.SubmitChanges();
        }

        /// <summary>
        /// Updates an existing ElectricalGroup object
        /// </summary>
        /// <param name="electricalGroup">ElectricalGroup object to update</param>        
        public void UpdateElectricalGroup(ElectricalGroup electricalGroup)
        {
            ElectricalGroup currentElectricalGroup = Context.ElectricalGroups.FirstOrDefault(eg => eg.Id.Equals(electricalGroup.Id));
            if (currentElectricalGroup == null) return;            
            currentElectricalGroup = electricalGroup;
            Context.SubmitChanges();
        }

        /// <summary>
        /// Inserts an existing ElectricalGroup object
        /// </summary>
        /// <param name="electricalGroup">ElectricalGroup object to insert</param>
        /// <returns>Identoty of inserted ElectricalGroup Object</returns>
        public int InsertElectricalGroup(ElectricalGroup electricalGroup)
        {           
            Context.ElectricalGroups.InsertOnSubmit(electricalGroup);
            Context.SubmitChanges();
            return electricalGroup.Id;
        }

        /// <summary>
        /// Commits any changes to the ElectricalGroup objects to the Database
        /// </summary>
        public void CommitChanges()
        {
            Context.SubmitChanges();
        }

        /// <summary>
        /// Gets the root ElectricalGroup Object
        /// </summary>
        /// <returns>ElectricalGroup Object</returns>
        public ElectricalGroup GetRootElectricalGroup()
        {
            return Context.ElectricalGroups.FirstOrDefault(eg => !eg.ParentPk.HasValue);
        }

        public ElectricalGroup GetElectricalGroup(int key)
        {
            return Context.ElectricalGroups.FirstOrDefault(eg => eg.Id.Equals(key));
        }

        public IList<ElectricalGroup> GetElectricalChildGroups(int key)
        {
            var childGroups = Context.ElectricalGroups.Where(eg => eg.ParentPk.Value.Equals(key));
            return childGroups != null ? childGroups.ToList<ElectricalGroup>() : null;
        }

        public IEnumerable<ElectricalGroup> GetElectricalGroups(int parentId)
        {
            return Context.ElectricalGroups.Where(eg => eg.ParentPk.Value.Equals(parentId));            
        }

        public bool ElectricalGroupAbbreviationExists(string abbreviation, int key)
        {
            return Context.ElectricalGroups.Any(e => e.Abbreviation.ToLower() == abbreviation.ToLower() && e.Id != key);
        }

        /// <summary>
        /// Gets a flat list of all Electrical Groups
        /// </summary>        
        /// <returns>List of ElectricalGroup objects</returns>
        public IList<ElectricalGroup> GetElectricalGroups()
        {
            var electricalGroups = this.Context.ElectricalGroups;
            return electricalGroups != null ? electricalGroups.ToList() : null;
        }

        public Map GetMap(int id)
        {
            return this.Context.Maps.FirstOrDefault(m => m.Id == id);
        }


        /// <summary>
        /// Retrieves BCEG Map
        /// </summary>        
        /// <returns>Map object</returns>
        public Map GetBCEGMap()
        {
            return Context.Maps.FirstOrDefault(m => m.MapType.Code.Equals(BCEG_MAP));
        }


        /// <summary>
        /// Retrieves LTAP Map
        /// </summary>        
        /// <returns>Map object</returns>
        public Map GetLTAPMap()
        {
            return Context.Maps.FirstOrDefault(m => m.MapType.Code.Equals(LTAP_MAP));
        }


        /// <summary>
        /// Determines whether the Electrical Group connected to the input Id is linked to a map idenified the by input map name
        /// </summary>
        /// <param name="eGId">Electrical Group Id</param>
        /// <param name="mapName"Map Name</param>
        /// <returns>true or false</returns>
        internal bool ElectricalGroupLinkedToMap(int eGId, Func<Map, bool> mapPredicate)
        {
            var eG = this.Context.ElectricalGroups.First(eEG => eEG.Id == eGId);
            var isLinked = eG.Maps.Any(mapPredicate);
            if (isLinked) return true;
            while (eG.ParentPk.HasValue)
            {
                int pId = eG.ParentPk.Value;
                eG = this.Context.ElectricalGroups.First(eEG => eEG.Id == pId);
                isLinked = eG.Maps.Any(mapPredicate);
                if (isLinked) return true;
            }
            return false;
        }


        /// <summary>
        /// Returns the first active Map linked to the input Electrical Group Id
        /// </summary>
        /// <param name="eGId">Electrical Group Id</param>
        /// <returns>Map object</returns>
        internal Map GetEGMap(int eGId)
        {
            var eG = this.Context.ElectricalGroups.First(eEG => eEG.Id == eGId);
            var map = eG.Maps.FirstOrDefault(m => m.IsActive.HasValue && m.IsActive.Value);
            if (map == null)
            {
                while (eG.ParentPk.HasValue)
                {
                    int pId = eG.ParentPk.Value;
                    eG = this.Context.ElectricalGroups.First(eEG => eEG.Id == pId);
                    map = eG.Maps.FirstOrDefault(m => m.IsActive.HasValue && m.IsActive.Value);
                    if (map != null) break;
                }
            }
            return map;
        }


        //public ElectricalGroup GetLTAPElectricalGroupBranch(int substationId)
        //{
        //    Map egMap = GetLTAPMap();
        //    if (egMap == null)
        //        return null;
         
        //    int matchedEG = 0;
        //    GetMapSubstationEG(egMap.MapDefinitionRootId, substationId, ref matchedEG);
        //    if (matchedEG == 0)
        //        return null;
        //    ElectricalGroup electricalGroup = GetElectricalGroup(matchedEG);
        //    return electricalGroup;
        //}


        ///// <summary>
        ///// Traverse MAP EG Hierarchy for the given EG substation link
        ///// </summary>
        ///// <param name="key">Electrical Group Id</param>
        ///// <param name="key">Substation Id</param>
        ///// <param name="matchingEG">Matched EG Id</param>
        ///// <returns></returns>
        //private void GetMapSubstationEG(int key, int substationId, ref int matchingEG)
        //{
        //    if (SubstationEGLinkExists(key, substationId))
        //    {
        //        matchingEG = key;
        //        return;
        //    }
        //    IList<ElectricalGroup> childGroups = GetElectricalChildGroups(key);
        //    if (childGroups != null && childGroups.Count > 0)
        //    {
        //        foreach (ElectricalGroup eg in childGroups)
        //            GetMapSubstationEG(eg.Id, substationId, ref matchingEG);
        //    }
        //}


        /// <summary>
        /// Checks if the substation / EG Link record exists.
        /// </summary>
        /// <param name="key">Electrical Group Id</param>
        /// <param name="substationId">Substation Id</param>
        /// <returns>bool</returns>
        public bool SubstationEGLinkExists(int key, int substationId)
        {
            LinkSubstationElectricalGroup link = (from lseg in this.Context.LinkSubstationElectricalGroups
                                                  where object.Equals(lseg.ElectricalGroupId, key)
                                                  && object.Equals(lseg.SubstationId, substationId)
                                                  select lseg).FirstOrDefault();
            return link != null ? true : false;
        }


        public IEnumerable<Map> GetMaps()
        {
            return this.Context.Maps;
        }

        public IEnumerable<Map> GetMaps(string type)
        {
            return this.Context.Maps.Where(m => m.MapType.Code.ToLower() == type.ToLower());
        }


        /// <summary>
        /// IDisposable interface
        /// </summary>
        public void Dispose()
        {
            if(_context != null) _context.Dispose();
        }

        #endregion
    }
}
