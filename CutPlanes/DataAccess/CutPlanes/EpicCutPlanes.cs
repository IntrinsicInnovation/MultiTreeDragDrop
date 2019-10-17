using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using EpicDataAccess.Extensions;

using CutPlanes.DataAccess.CutPlanes;


namespace EpicDataAccess.CutPlanes
{
    public class EpicCutPlanes : IDisposable
    {
        #region [Properties]
        private string ConnectionString { get; set; }

        private CutPlanesEntities _context = null;
        private CutPlanesEntities Context
        {
            get
            {
                if (_context == null) _context = new CutPlanesEntities(); 
                return _context;
            }
        }
        #endregion

        #region [Constructor]

        public EpicCutPlanes(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Gets a list of all Cut Planes
        /// </summary>        
        /// <returns>List of Cut Plane Objects</returns>
        public List<CutPlane> GetCutPlanes()
        {
            var cutPlanes = (from cp in this.Context.CutPlanes
                             join cpt in this.Context.CutPlaneTypes on cp.TypeId equals cpt.Id
                             select cp);
            return cutPlanes != null ? cutPlanes.ToList() : null;
        }


      



        /// <summary>
        /// Creates or Retrieves an existing Time Period (by Year)
        /// </summary>        
        /// <returns>Existing or New CutPlane Time Period Id</returns>
        public int AddGetTimePeriodIdbyYear(int currentYear, DateTime generationTime, int userId)
        {
            var dimCutPlaneTimePeriod = (from dcptp in this.Context.DimCutPlaneTimePeriods
                                         where object.Equals(dcptp.Year, currentYear)
                                         && dcptp.DateCreated.Equals(generationTime)
                                         select dcptp).FirstOrDefault();

            if (dimCutPlaneTimePeriod == null)
            {
                dimCutPlaneTimePeriod = new DimCutPlaneTimePeriod();
                dimCutPlaneTimePeriod.DateCreated = generationTime;
                dimCutPlaneTimePeriod.Name = "F" + currentYear.ToString();
                dimCutPlaneTimePeriod.UserCreated = userId.ToString();
                dimCutPlaneTimePeriod.Year = currentYear;
                this.Context.DimCutPlaneTimePeriods.Add(dimCutPlaneTimePeriod);
                this.Context.SaveChanges();
            }
            return dimCutPlaneTimePeriod.Id;
            //return dimCutPlaneTimePeriod != null ? dimCutPlaneTimePeriod.Id : -1;
        }


        /// <summary>
        /// Returns a list of Substations objects in ascending order (by Electrical Group Id)
        /// </summary>
        /// <param name="substionId">substation Id</param>
        /// <param name="resourcePlanId">resourcePlan Id</param>
        /// <returns>SubstationAttribute object</returns>
        public IList<Substation> GetSubstationsbyElectricalGroup(int electricalGroupId)
        {
            var substations = from s in this.Context.Substations
                              join lseg in this.Context.LinkSubstationElectricalGroups on s.Id equals lseg.SubstationId
                              where lseg.ElectricalGroupId == electricalGroupId
                              select s;
            return substations != null ? substations.ToList() : null;
        }




        /// <summary>
        /// Retrieves a Substation (by ID)
        /// </summary>        
        /// <returns>Substation Object</returns>
        public Substation GetSubstation(int key)
        {
            return Context.Substations.FirstOrDefault(s => s.Id.Equals(key));
        }



        /// <summary>
        /// Retrieves a Cut Plane (by ID)
        /// </summary>        
        /// <returns>Cut Plane Object</returns>
        public CutPlane GetCutPlane(int cutPlaneId)
        {
            return Context.CutPlanes.FirstOrDefault(cp => cp.Id.Equals(cutPlaneId));
        }



        /// <summary>
        /// Retrieves a Cut Plane Type (by ID)
        /// </summary>        
        /// <returns>CutPlaneType Object</returns>
        public CutPlaneType GetCutPlaneTypeById(int cutPlaneTypeId)
        {
            return Context.CutPlaneTypes.FirstOrDefault(cpt => cpt.Id.Equals(cutPlaneTypeId));
        }


        /// <summary>
        /// Retrieves a Cut Plane Type (by ID)
        /// </summary>        
        /// <returns>Cut Plane Type Object</returns>
        public CutPlaneType GetCutPlaneType(int cutPlaneId)
        {
            var cutPlaneType = (from cp in this.Context.CutPlanes.Where(cp => cp.Id == cutPlaneId)
                                join cpt in this.Context.CutPlaneTypes on cp.TypeId equals cpt.Id
                                select cpt).FirstOrDefault();
            return cutPlaneType != null ? cutPlaneType : null;
        }

        /// <summary>
        /// Retrieves all Cut Plane Types
        /// </summary>        
        /// <returns>list of Cut Plane Type Objects</returns>
        public List<CutPlaneType> GetCutPlaneTypes()
        {
            var cutplanetypes = this.Context.CutPlaneTypes;
            return cutplanetypes != null ? cutplanetypes.ToList() : null;
        }


        public int GetCutPlaneTypeId(string value)
        {
            var cutplanetype = Context.CutPlaneTypes.FirstOrDefault(cpt => cpt.Code.Equals(value));
            return cutplanetype != null ? cutplanetype.Id : 0;
        }


        /// <summary>
        /// Retrieves all Pool Types
        /// </summary>        
        /// <returns>list of Pool Type Objects</returns>
        public List<PoolType> GetPoolTypes()
        {
            var pooltypes = this.Context.PoolTypes;
            return pooltypes != null ? pooltypes.ToList() : null;
        }


        public int GetPoolTypeId(string value)
        {
            var pooltype = Context.PoolTypes.FirstOrDefault(pt => pt.Value.Equals(value));
            return pooltype != null ? pooltype.Id : 0;
        }


        /// <summary>
        /// Retrieves all Pools
        /// </summary>        
        /// <returns>list of Pools Objects</returns>
        public List<Pool> GetPools(int cpid)
        {
            var pools = (from p in this.Context.Pools.Where(pl => pl.CutPlaneId.Equals(cpid))
                         select p);
            return pools != null ? pools.ToList() : null;
        }


        /// <summary>
        /// Retrieves all Substations not in the list of added substations
        /// </summary>        
        /// <returns>list of ElectricalGroup objects</returns>
        public List<Substation> GetSourceSubstations(ElectricalGroup parentGroup, List<Substation> addedSubstations)
        {

            // int parentGroupId = GetParentGroupId(mapId);
            // ElectricalGroup parentGroup = GetElectricalParentGroups GetElectricalGroup(parentGroupId);

            if (parentGroup != null)
            {
                //First retrieve list of substations that are in this particular map.
                List<Substation> mappedSubstations = new List<Substation>();
                getChildSubstations(parentGroup, mappedSubstations);

                //Next compare mapped substations to those added into the cutplane,
                //and retrieve only those mapped substations that haven't been added to the cutplane.
                int[] addedarray = addedSubstations.Select(ads => ads.Id).ToArray();
                var substations = mappedSubstations.Where(ms => !addedarray.Contains(ms.Id)).Distinct(); //ms.ElectricalGroupId.HasValue && 
                return substations != null ? substations.ToList() : null;
            }
            else
                return null;
        }


        /// <summary>
        /// Retrieves Root EG Id for a particular Map.
        /// </summary>        
        /// <returns>Root EG Id</returns>
        public int GetParentGroupId(int mapId)
        {
            var map = (from m in this.Context.Maps.Where(m => m.Id == mapId)
                       select m).FirstOrDefault();
            return map.MapDefinitionRootId;
        }


        /// <summary>
        /// Retrieves all mapped child Substations for a particular parentGroup.
        /// </summary>        
        /// <returns>list of ElectricalGroup objects</returns>
        public void getChildSubstations(ElectricalGroup parentGroup, List<Substation> assignedSubstations)
        {
            IList<Substation> substations = GetSubstationsbyElectricalGroup(parentGroup.Id) as IList<Substation>;
            assignedSubstations.AddRange(substations);
            if (parentGroup.ChildGroups != null && parentGroup.ChildGroups.Count > 0)
            {
                foreach (ElectricalGroup cg in parentGroup.ChildGroups)
                {
                    getChildSubstations(cg, assignedSubstations);
                }
            }
        }


        /// <summary>
        /// Retrieves all Substations for a pool
        /// </summary>        
        /// <returns>list of ElectricalGroup objects</returns>
        public List<Substation> GetPoolSubstations(int poolId)
        {


            var substations = (from p in this.Context.Pools
                               where p.Id == poolId
                               join lsp in this.Context.LinkSubstationPools on p.Id equals lsp.PoolId
                               join s in this.Context.Substations on lsp.SubstationId equals s.Id
                               // join lseg in this.Context.LinkSubstationElectricalGroups on s.Id equals lseg.SubstationId
                               // join eg in this.Context.ElectricalGroups on lseg.ElectricalGroupId equals eg.Id
                               select s);
            return substations != null ? substations.ToList() : null;
        }




        /// <summary>
        /// Retrieves Pools
        /// </summary>        
        /// <returns>list of Pools Objects</returns>
        public Pool GetPool(int poolId)
        {
            var pool = (from p in this.Context.Pools.Where(pl => pl.Id.Equals(poolId))
                        select p).FirstOrDefault();
            return pool;
        }


        /// <summary>
        /// Inserts a new Pool object to Db or updates any existing Pool object
        /// </summary>
        /// <param name="cutplane">Pool object</param>        
        public int SavePool(int cutPlaneId, int poolId, string poolName, string poolType)
        {
            Pool currentPool = (from p in this.Context.Pools.Where(pl => pl.Id.Equals(poolId))
                                select p).FirstOrDefault();

            if (currentPool != null)
            {
                // int maxlength = this.Context.Pools.m Max(p => p.Name.Length);
                currentPool.Name = poolName;
                currentPool.PoolTypeId = GetPoolTypeId(poolType);
                poolId = currentPool.Id;
                this.Context.SaveChanges();
            }
            else
            {

                Pool pool = new Pool();
                pool.CutPlaneId = cutPlaneId;
                pool.Name = poolName;
                pool.PoolTypeId = GetPoolTypeId(poolType);
                pool.DateCreated = DateTime.Now;
                pool.UserCreatedId = 1;
                this.Context.Pools.Add(pool);
                this.Context.SaveChanges();
                poolId = pool.Id;
            }
            return poolId;
        }


        /// <summary>
        /// Inserts a new CutPlane object to Db or updates any existing CutPlane object (based on CutPlane ID)
        /// </summary>
        /// <param name="cutplane">Cutplane object</param>        
        public int SaveCutPlanebyID(CutPlane cutplane)
        {
            CutPlane currentCutplane = (from s in this.Context.CutPlanes.Where(cp => cp.Id.Equals(cutplane.Id))
                                        select s).FirstOrDefault();

            if (currentCutplane != null)
            {
                currentCutplane.DateUpdated = cutplane.DateCreated; // DateTime.Now;
                currentCutplane.UserUpdatedId = cutplane.UserCreatedId;
                currentCutplane.Name = cutplane.Name;
                currentCutplane.TypeId = cutplane.TypeId;
                currentCutplane.Description = cutplane.Description;
                cutplane.Id = currentCutplane.Id;
            }
            else
                this.Context.CutPlanes.Add(cutplane);
            this.Context.SaveChanges();
            return cutplane.Id;
        }


        /// <summary>
        /// Inserts a new LinkSubstationPool object to Db (based on substation ID and pool ID)
        /// </summary>
        /// <param name="cutplane">Cutplane object</param>        
        public void SaveSubstationPool(int substationId, int poolId)
        {
            LinkSubstationPool currentlinksubstpool = (from lsp in this.Context.LinkSubstationPools.Where(l => l.SubstationId.Equals(substationId)
                                                       && l.PoolId.Equals(poolId))
                                                       select lsp).FirstOrDefault();
            if (currentlinksubstpool == null)
            {
                LinkSubstationPool linksubstationpool = new LinkSubstationPool();
                linksubstationpool.SubstationId = substationId;
                linksubstationpool.PoolId = poolId;
                linksubstationpool.DateCreated = DateTime.Now;
                this.Context.LinkSubstationPools.Add(linksubstationpool);
                this.Context.SaveChanges();
            }
        }


        /// <summary>
        /// Deletes a LinkSubstationPool record in Db (based on substation ID and pool ID)
        /// </summary>
        /// <param name="substationId">substation id</param>        
        /// <param name="poolId">pool id</param>        
        public void DeleteSubstationPool(int substationId, int poolId)
        {
            LinkSubstationPool currentlinksubstpool = (from lsp in this.Context.LinkSubstationPools.Where(l => l.SubstationId.Equals(substationId)
                                                       && l.PoolId.Equals(poolId))
                                                       select lsp).FirstOrDefault();
            if (currentlinksubstpool != null)
            {
                this.Context.LinkSubstationPools.Remove(currentlinksubstpool);
                this.Context.SaveChanges();
            }
        }

        public ElectricalGroup GetElectricalGroup(int key)
        {
            return Context.ElectricalGroups.FirstOrDefault(eg => eg.Id.Equals(key));
        }


        public ElectricalGroup GetElectricalGroupbySubstationId(int substationId, List<ElectricalGroup> mapEGs) //int mapId,
        {
            List<int> egIdList = new List<int>();

            foreach (ElectricalGroup eg in mapEGs)
            {
                egIdList.Add(eg.Id);
            }

            //first need as List of egs that are in the map, can't use the linksubstationeg table (in its entirety)
            //as it contains duplicate substations.
            //then use this List of egs in Linq Query below or loop.
            var electricalgroupid = (
                from lseg in this.Context.LinkSubstationElectricalGroups
                where lseg.SubstationId == substationId
                where egIdList.Contains(lseg.ElectricalGroupId)
                select lseg.ElectricalGroupId).FirstOrDefault();

            foreach (ElectricalGroup eg in mapEGs)
            {
                if (eg.Id == electricalgroupid) //Should create an iequatable method.
                    return eg;
            }
            return null;
        }

        public List<ElectricalGroup> GetElectricalChildGroups(int key)
        {
            var childGroups = Context.ElectricalGroups.Where(eg => eg.ParentPk.Value.Equals(key));
            return childGroups != null ? childGroups.ToList<ElectricalGroup>() : null;
        }

        /// <summary>
        /// Retrieves a Constraint Set (by poolId)
        /// </summary>        
        /// <returns>ConstraintSet Object</returns>
        public ConstraintSet GetConstraintSet(int constraintSetId)
        {
            return Context.ConstraintSets.FirstOrDefault(cs => cs.Id.Equals(constraintSetId));
        }

        /// <summary>
        /// Retrieves a list of ConstraintSets (by poolId)
        /// </summary>        
        /// <returns>list of ConstraintSet Objects</returns>
        public List<ConstraintSet> GetAllConstraintSets(int poolId)
        {
            var constraintset = Context.ConstraintSets.Where(cs => cs.PoolId.Equals(poolId));
            return constraintset != null ? constraintset.ToList<ConstraintSet>() : null;
        }


        /// <summary>
        /// Retrieves a list of Constraints (by constraintSetId)
        /// </summary>        
        /// <returns>Constraint Object</returns>
        public List<Constraint> GetConstraints(int constraintSetId)
        {
            var constraints = Context.Constraints.Where(c => c.ConstraintSetId.Equals(constraintSetId));
            return constraints != null ? constraints.ToList<Constraint>() : null;
        }

        private void assignValue(Constraint constraint, int colId, string value)
        {
            switch (colId)
            {
                case 0:
                    //constraint.StartDate = Convert.ToDateTime(value);
                    constraint.FiscalYear = Convert.ToInt32(value);
                    break;
                //case 1:
                //    constraint.EndDate = Convert.ToDateTime(value);
                //    break;
                case 1:
                    constraint.SummerN0 = Convert.ToDecimal(value);
                    break;
                case 2:
                    constraint.SummerN1 = Convert.ToDecimal(value);
                    break;
                case 3:
                    constraint.WinterN0 = Convert.ToDecimal(value);
                    break;
                case 4:
                    constraint.WinterN1 = Convert.ToDecimal(value);
                    break;
                case 5:
                    constraint.AreaLossInPercentage = Convert.ToDecimal(value);
                    break;
                case 6:
                    constraint.TotalLossInPercentage = Convert.ToDecimal(value);
                    break;
                case 7:
                    constraint.IntermittentRmr = Convert.ToDecimal(value);
                    break;
            }
        }

        /// <summary>
        /// Inserts a new Constraint object to Db or updates any existing Constraint object
        /// </summary>
        /// <param name="cutplane">Constraint object</param>        
        public int SaveConstraint(int colId, int row, string value, DateTime createdDate, int userId, int constraintSetId)
        {
            Constraint currentConstraint = (from c in this.Context.Constraints.Where(cs => cs.Id.Equals(row))
                                            select c).FirstOrDefault();

            if (currentConstraint != null)
            {
                currentConstraint.DateUpdated = createdDate;
                currentConstraint.UserUpdatedId = userId;
                assignValue(currentConstraint, colId, value);
            }
            else
            {
                currentConstraint = new Constraint();
                currentConstraint.DateCreated = createdDate;
                currentConstraint.UserCreatedId = userId;
                assignValue(currentConstraint, colId, value);
                this.Context.Constraints.Add(currentConstraint);
            }
            this.Context.SaveChanges();
            return currentConstraint.Id;
        }


        /// <summary>
        /// Inserts a new Constraint object to Db or updates any existing Constraint object
        /// </summary>
        /// <param name="cutplane">Constraint object</param>        
        public int SaveConstraintRow(Constraint constraint) //   int rowId, string[] values, DateTime createdDate, int userId, int constraintSetId)
        {
            Constraint currentConstraint = (from c in this.Context.Constraints.Where(cs => cs.Id.Equals(constraint.Id))
                                            select c).FirstOrDefault();

            if (currentConstraint != null)
            {
                currentConstraint.DateUpdated = constraint.DateCreated;
                currentConstraint.UserUpdatedId = constraint.UserCreatedId;
              
                currentConstraint.FiscalYear = constraint.FiscalYear;
                currentConstraint.ConstraintSetId = constraint.ConstraintSetId;
                currentConstraint.SummerN0 = constraint.SummerN0;
                currentConstraint.SummerN1 = constraint.SummerN1;
                currentConstraint.WinterN0 = constraint.WinterN0;
                currentConstraint.WinterN1 = constraint.WinterN1;
              
                currentConstraint.AreaLossInPercentage = constraint.AreaLossInPercentage;
             
                currentConstraint.TotalLossInPercentage = constraint.TotalLossInPercentage;
                currentConstraint.IntermittentRmr = constraint.IntermittentRmr;
                constraint.Id = currentConstraint.Id;
            }
            else
            {
                this.Context.Constraints.Add(constraint);
            }

            this.Context.SaveChanges();
            return constraint.Id;
        }



        /// <summary>
        /// Delete Constraint object from Db
        /// </summary>
        /// <param name="cutplane">Constraint ID</param>        
        public bool DeleteConstraintRow(int id)
        {
            Constraint currentConstraint = (from c in this.Context.Constraints.Where(cs => cs.Id.Equals(id))
                                            select c).FirstOrDefault();
            if (currentConstraint != null)
            {
                this.Context.Constraints.Remove(currentConstraint);
                this.Context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Inserts a new Constraint object to Db or updates any existing Constraint object
        /// </summary>
        /// <param name="cutplane">Constraint object</param>        
        public int SaveConstraintSet(ConstraintSet constraintSet)
        {
            ConstraintSet currentConstraintSet = (from c in this.Context.ConstraintSets.Where(cs => cs.Id.Equals(constraintSet.Id))
                                                  select c).FirstOrDefault();

            if (currentConstraintSet != null)
            {
                currentConstraintSet.DateUpdated = constraintSet.DateCreated;
                currentConstraintSet.UserUpdatedId = constraintSet.UserCreatedId;
                currentConstraintSet.Name = constraintSet.Name;
            }
            else
            {
                currentConstraintSet = new ConstraintSet();
                currentConstraintSet.DateCreated = constraintSet.DateCreated;
                currentConstraintSet.UserCreatedId = constraintSet.UserCreatedId;
                currentConstraintSet.Name = constraintSet.Name;
                currentConstraintSet.PoolId = constraintSet.PoolId;
                this.Context.ConstraintSets.Add(currentConstraintSet);
            }

            this.Context.SaveChanges();
            return currentConstraintSet.Id;
        }


        public void GetElectricalParentGroups(ElectricalGroup eGroup, List<ElectricalGroup> parentGroups)
        {
            var parentGroup = Context.ElectricalGroups.Where(eg => eg.Id.Equals(eGroup.ParentPk)).FirstOrDefault();
            if (parentGroup != null)
            {

                if (!parentGroups.Contains((ElectricalGroup)parentGroup))
                {
                    parentGroups.Add((ElectricalGroup)parentGroup);
                    if (parentGroup.ParentPk != null)
                        GetElectricalParentGroups((ElectricalGroup)parentGroup, parentGroups);
                }

            }
        }



        string GetCutplaneName(int cutPlaneId)
        {
            return this.Context.CutPlanes.Where(cp => object.Equals(cp.Id, cutPlaneId)).FirstOrDefault().Name;
        }

      
        #endregion

        public void Dispose()
        {
            if (_context != null) _context.Dispose();
        }





        /*****************************************************************************************************************************************************************/
        //MAPS

        public List<Substation> GetAllSubstations()
        {
            var substations = (from s in this.Context.Substations
                               select s); //.OrderBy(s => s.TLA);

            List<Substation> substationList = (substations != null ? substations.ToList() : null);
            substationList.Sort();  //Implemented Icomparable.
            return substationList;
        }


        /// <summary>
        /// Retrieves MapType based on ID
        /// </summary>        
        /// <returns>MapType object</returns>
        public MapType GetMapType(int mapTypeId)
        {
            return Context.MapTypes.FirstOrDefault(mt => mt.Id.Equals(mapTypeId));
        }


        /// <summary>
        /// Retrieves Map based on ID
        /// </summary>        
        /// <returns>Map object</returns>
        public Map GetMap(int mapId)
        {
            return Context.Maps.FirstOrDefault(m => m.Id.Equals(mapId));
        }




        /// <summary>
        /// Returns a list of Maps
        /// </summary>
        /// <returns>List of Maps</returns>
        public IList<Map> GetAllMaps()
        {
            var maps = from m in this.Context.Maps
                       select m;
            return maps != null ? maps.ToList() : null;
        }


        /// <summary>
        /// Returns a list of MapType objects 
        /// </summary>
        /// <returns>SubstationAttribute object</returns>
        public IList<MapType> GetAllMapTypes()
        {
            var maptypes = from mt in this.Context.MapTypes
                           select mt;
            return maptypes != null ? maptypes.ToList() : null;
        }


        /// <summary>
        /// Returns a list of MapType objects 
        /// </summary>
        /// <returns>SubstationAttribute object</returns>
        public int GetMapTypeId(string mapTypeCode)
        {
            var map = (from mt in this.Context.MapTypes.Where(mt => mt.Code.Equals(mapTypeCode))
                       select mt).FirstOrDefault();
            return map != null ? map.Id : 0;
        }


        /// <summary>
        /// Inserts a new Map object to Db or updates any existing Map object (based on Map ID)
        /// </summary>
        /// <param name="cutplane">Map object</param>        
        public int SaveMapByID(Map map)
        {
            Map currentMap = (from m in this.Context.Maps.Where(m => m.Id.Equals(map.Id))
                              select m).FirstOrDefault();

            if (currentMap != null)
            {
                currentMap.DateUpdated = map.DateCreated; // DateTime.Now;
                currentMap.UserUpatedId = map.UserCreatedId;
                currentMap.Name = map.Name;
                currentMap.Description = map.Description;
                currentMap.TypeId = map.TypeId;
                map.Id = currentMap.Id;
            }
            else
                this.Context.Maps.Add(map);
            this.Context.SaveChanges();
            return map.Id;
        }


        /// <summary>
        /// Inserts a new Map object to Db or updates any existing Map object (based on Map ID)
        /// </summary>
        /// <param name="cutplane">Map object</param>        
        public int SaveElectricalGroup(ElectricalGroup electricalGroup)
        {
            ElectricalGroup currentEG = (from eg in this.Context.ElectricalGroups.Where(eg => eg.Id.Equals(electricalGroup.Id))
                                         select eg).FirstOrDefault();

            if (currentEG != null)
            {
                currentEG.DateUpdated = electricalGroup.DateCreated;
                currentEG.UserUpdatedId = electricalGroup.UserCreatedId;
                currentEG.Abbreviation = electricalGroup.Abbreviation;
                currentEG.Name = electricalGroup.Name;
                currentEG.ParentPk = electricalGroup.ParentPk;
                electricalGroup.Id = currentEG.Id;
            }
            else
                this.Context.ElectricalGroups.Add(electricalGroup);
            this.Context.SaveChanges();
            return electricalGroup.Id;
        }


        /// <summary>
        /// Inserts a new LinkSubstationElectricalGroups object to Db
        /// </summary>
        /// <param name="cutplane">Map object</param>        
        public int SaveSubstationEG(LinkSubstationElectricalGroup linkSubstationElectricalGroup)
        {
            LinkSubstationElectricalGroup currentLinkSubstationElectricalGroup =
            (from lseg in this.Context.LinkSubstationElectricalGroups.Where(lseg => lseg.SubstationId.Equals(linkSubstationElectricalGroup.SubstationId)
             && lseg.ElectricalGroupId.Equals(linkSubstationElectricalGroup.ElectricalGroupId))
             select lseg).FirstOrDefault();

            if (currentLinkSubstationElectricalGroup != null)
            {

                linkSubstationElectricalGroup.Id = currentLinkSubstationElectricalGroup.Id;
            }
            else
                this.Context.LinkSubstationElectricalGroups.Add(linkSubstationElectricalGroup);
            this.Context.SaveChanges();
            return linkSubstationElectricalGroup.Id;
        }


        /// <summary>
        /// Deletes a LinkSubstationElectricalGroups object to Db
        /// </summary>
        /// <param name="cutplane">Map object</param>        
        public bool DeleteSubstationEG(int substationId, int egId)
        {
            LinkSubstationElectricalGroup currentLinkSubstationElectricalGroup =
            (from lseg in this.Context.LinkSubstationElectricalGroups.Where(lsg => lsg.SubstationId.Equals(substationId)
             && lsg.ElectricalGroupId.Equals(egId))
             select lseg).FirstOrDefault();
            bool deleted = false;
            if (currentLinkSubstationElectricalGroup != null)
            {

                this.Context.LinkSubstationElectricalGroups.Remove(currentLinkSubstationElectricalGroup);
                this.Context.SaveChanges();
                deleted = true;
            }
            return deleted;
        }




        public bool DeleteElectricalGroup(int key)
        {
            ElectricalGroup group = GetElectricalGroup(key);
            if (group == null) return true;
            return DeleteElectricalGroup(group);
        }


        public bool DeleteElectricalGroup(ElectricalGroup group)
        {
            if (group == null) return true;
            bool deleted = true;
        
            if (group.ChildGroups != null && group.ChildGroups.Count > 0)
                foreach (ElectricalGroup g in group.ChildGroups)
                {
                    deleted = DeleteElectricalGroup(g);
                    if (!deleted) return deleted;
                }
            DeleteElectricalGroupPermanent(group);
            return true;
        }


        /// <summary>
        /// Deletes an existing ElectricalGroup object
        /// </summary>
        /// <param name="electricalGroup">ElectricalGroup object to delete</param>
        public void DeleteElectricalGroupPermanent(ElectricalGroup electricalGroup)
        {
            if (electricalGroup == null) return;
            Context.ElectricalGroups.Remove(electricalGroup);
            Context.SaveChanges();
        }


        public bool ElectricalGroupAbbreviationExists(string abbreviation, int key, List<ElectricalGroup> mapEGs)
        {
            return mapEGs.Any(e => e.Abbreviation.ToLower() == abbreviation.ToLower() && e.Id != key);
        }


        public bool ElectricalGroupNameExists(string name, int key, List<ElectricalGroup> mapEGs)
        {
            return mapEGs.Any(e => e.Name.ToLower() == name.ToLower() && e.Id != key);
        }





    }




    public class CutPlanesNameValuePair
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public CutPlanesNameValuePair(string Name, string Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        public CutPlanesNameValuePair()
        {
            this.Name = Name;
            this.Value = Value;
        }
    }


    public class ForecastResults
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NitsResourcePlan { get; set; }
        public string SeasonType { get; set; }
        public string TransmissionCapability { get; set; }
        public string ConstraintsLabel { get; set; }
        public string LoadForecast { get; set; }
        public string CutPlaneName { get; set; }
        public string CutPlaneType { get; set; }
    }


    public class ForecastConstraintsFlow
    {
        public List<CutPlanesNameValuePair> forecastConstraints { get; set; }
        public List<CutPlanesNameValuePair> forecastFlow { get; set; }
    }


    public class FlowForecastReport
    {
        //Generation Forecast Page
        public List<FacilityGenerationLoadForecast> generationForecastList { get; set; }
        public List<CutPlanesNameValuePair> sourceRegionTotalMPO { get; set; }

        //Load Forecast Page
        public List<FacilityGenerationLoadForecast> loadForecastList { get; set; }
        public List<CutPlanesNameValuePair> sourceRegionNonCoincidentPeakLoad { get; set; }

        //Cut-Plane Flow Forecast
        public string LoadLevelOfInterestPercent { get; set; }
        public string SourceRegionLoadCoincidenceFactor { get; set; }
        public List<CutPlanesNameValuePair> cutPlaneLimit { get; set; }
        public List<CutPlanesNameValuePair> cutPlaneFlow { get; set; }
        public List<CutPlanesNameValuePair> regionalLossFactor { get; set; }
        
        public List<CutPlanesNameValuePair> loadLevelOfInterest { get; set; }
        public List<CutPlanesNameValuePair> regionalLoadPlusLosses { get; set; }
    }


    public class FacilityGenerationLoadForecast
    {
        public string ID { get; set; }
        public string FacilityName { get; set; }
        public string FuelType { get; set; }
        public string BCEG { get; set; }
        public List<CutPlanesNameValuePair> facilityForecast { get; set; }
    }












}
