using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public class EpicLoads : EpicDomainBase
    {
         #region [Properties]


        #endregion

         #region [Constructors]

        public EpicLoads(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Retrieves a Load object based on input identity
        /// </summary>
        /// <param name="id">Load identity</param>
        /// <returns>Load object</returns>
        public Load GetLoad(int id)
        {
            return this.Context.Loads.FirstOrDefault(l => l.Id == id);
        }

        /// <summary>
        /// Returns a list of Load objects linked to input substation Id
        /// </summary>
        /// <param name="substationId">substation db identity</param>
        /// <returns>list of Load objects</returns>
        public IList<Load> GetLoads(int substationId)
        {
            var loads = from l in this.Context.Loads                        
                        join v in this.Context.VoltageLevels on l.VoltageLevelId equals v.Id
                        where v.SubstationId == substationId
                        select l;
            return loads != null ? loads.ToList() : null;
        }

        /// <summary>
        /// Returns a list of Load objects linked to input substation Id 
        /// </summary>
        /// <param name="substationId">substation db identity</param>
        /// <returns>list of Load objects</returns>
        public IList<Load> GetLoads(int substationId, int? phaseId)
        {
            var loads = from l in this.Context.Loads
                        join la in this.Context.LoadAttributes on l.Id equals la.LoadId
                        join v in this.Context.VoltageLevels on l.VoltageLevelId equals v.Id
                        where v.SubstationId == substationId && la.ProjectPhaseId == phaseId
                        select l;
            return loads != null ? loads.ToList() : null;
        }

        /// <summary>
        /// Returns a list of Load objects linked to input substation Id and whose ProjectPhaseId as found in the input array
        /// </summary>
        /// <param name="substationId">substation db identity</param>
        /// <returns>list of Load objects</returns>
        public IEnumerable<Load> GetLoads(int substationId, int[] phaseIds)
        {
             return from l in this.Context.Loads
                    join la in this.Context.LoadAttributes on l.Id equals la.LoadId
                    join v in this.Context.VoltageLevels on l.VoltageLevelId equals v.Id
                    where v.SubstationId == substationId && la.ProjectPhaseId.HasValue && phaseIds.Contains(la.ProjectPhaseId.Value)
                    select l;
            
        }

        /// <summary>
        /// Returns a list of Load objects linked to input substation Id
        /// </summary>
        /// <param name="substationId">substation db identity</param>
        /// <returns>list of Load objects</returns>
        public IList<Load> GetLoadsByResourcePlan(int substationId, int? resourcePlanId)
        {
            var loads = (from l in this.Context.Loads
                        join la in this.Context.LoadAttributes on l.Id equals la.LoadId
                        join v in this.Context.VoltageLevels on l.VoltageLevelId equals v.Id
                        where v.SubstationId == substationId && la.ResourcePlanId == resourcePlanId &&
                        !la.ProjectPhaseId.HasValue
                        select l).Distinct();
            return loads != null ? loads.ToList() : null;
        }
        

        /// <summary>
        /// returns the LoadAttribute object linked to the input LoadId and ResourcePlanId
        /// </summary>
        /// <param name="loadId">Load Db identity</param>
        /// <param name="resourcePlanId">ResourcePlan Db identity</param>
        /// <returns>LoadAttribute object</returns>
        public LoadAttribute GetAttribute(int loadId, int? resourcePlanId)
        {
            return this.Context.LoadAttributes.FirstOrDefault(la => la.LoadId == loadId && la.ResourcePlanId == resourcePlanId && !la.ProjectPhaseId.HasValue);
        }

        /// <summary>
        /// returns the LoadAttribute object linked to the input LoadId and phaseId
        /// </summary>
        /// <param name="loadId">Load Db identity</param>
        /// <param name="resourcePlanId">Project Phase Id</param>
        /// <returns>LoadAttribute object</returns>
        public LoadAttribute GetAttributeByPhase(int loadId, int? phaseId)
        {
            return this.Context.LoadAttributes.FirstOrDefault(la => la.LoadId == loadId && la.ProjectPhaseId == phaseId);
        }

        public IList<LoadAttribute> GetAttributeList(int? resourcePlanId, int? phaseId)
        {
            var attributes = this.Context.LoadAttributes.Where(la => la.ResourcePlanId == resourcePlanId && la.ProjectPhaseId == phaseId);
            return attributes != null ? attributes.ToList() : null;
        }

        public IList<LoadAttribute> GetAttributeList(int? phaseId)
        {
            var attributes = this.Context.LoadAttributes.Where(la => la.ProjectPhaseId == phaseId);
            return attributes != null ? attributes.ToList() : null;
        }

        public void DeleteAttribute(int loadId, int? resourcePlanId, int? phaseId)
        {
            LoadAttribute loadAttribute = this.Context.LoadAttributes.FirstOrDefault(la => la.LoadId == loadId &&
                                                                                    la.ResourcePlanId == resourcePlanId &&
                                                                                    la.ProjectPhaseId == phaseId);
            if (loadAttribute == null) return;
            this.Context.LoadAttributes.DeleteOnSubmit(loadAttribute);
            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Deletes all LoadAttribute objects linked to the input substationId & phaseId
        /// </summary>
        /// <param name="substationId"></param>
        /// <param name="phaseId"></param>
        public void DeleteLoadAttributes(int substationId, int? phaseId)
        {
            var loadAttributes =  from l in this.Context.Loads
                                  join la in this.Context.LoadAttributes on l.Id equals la.LoadId
                                  join v in this.Context.VoltageLevels on l.VoltageLevelId equals v.Id
                                  where v.SubstationId == substationId && la.ProjectPhaseId == phaseId
                                  select la;

            if (loadAttributes == null) return;
            this.Context.LoadAttributes.DeleteAllOnSubmit(loadAttributes);
            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Deletes all LoadAttribute objects linked to the input phaseId
        /// </summary>
        /// <param name="substationId"></param>
        /// <param name="phaseId"></param>
        public void DeleteAttributes(int phaseId)
        {
            var loadAttributes = this.Context.LoadAttributes.Where(la => la.ProjectPhaseId == phaseId);

            if (loadAttributes == null) return;
            this.Context.LoadAttributes.DeleteAllOnSubmit(loadAttributes);
            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Inserts any new Load object to Db or updates an existing Load object (based on Load Identity)
        /// </summary>
        /// <param name="Load">Load object</param>
        public void SaveLoad(Load load)
        {
            Load currentLoad = this.Context.Loads.FirstOrDefault(l => l.Id == load.Id);

            if (currentLoad != null)
            {
                currentLoad.DateUpdated = DateTime.Now;
                currentLoad.UserUpdatedId = load.UserCreatedId;
                currentLoad.AccountNumber = load.AccountNumber;
                currentLoad.StatusId = load.StatusId;
                currentLoad.VoltageLevelId = load.VoltageLevelId;
                load.Id = currentLoad.Id;
            }
            else
                this.Context.Loads.InsertOnSubmit(load);
            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Inserts any new LoadAttribute object to Db or updates an existing LoadAttribute object (based on LoadId and ResourcePlanId)
        /// </summary>
        /// <param name="LoadAttribute">LoadAttribute object</param>
        public void Save(LoadAttribute la)
        {
            LoadAttribute currentLa = this.Context.LoadAttributes.FirstOrDefault(lat => lat.LoadId == la.Id && lat.ResourcePlanId == la.ResourcePlanId && (!lat.ProjectPhaseId.HasValue || lat.ProjectPhaseId == la.ProjectPhaseId));
            if (currentLa != null)
                UpdateLoadAttribute(currentLa, la);
            else
                this.Context.LoadAttributes.InsertOnSubmit(la);
            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Inserts any new LoadAttribute object to Db or updates an existing LoadAttribute object (based on LoadId and ResourcePlanId)
        /// </summary>
        /// <param name="LoadAttribute">LoadAttribute object</param>
        public void SaveLoadAttribute(LoadAttribute loadAttribute)
        {
            LoadAttribute currentLoadAttribute = this.Context.LoadAttributes.FirstOrDefault(la => la.LoadId == loadAttribute.LoadId && la.ResourcePlanId == loadAttribute.ResourcePlanId && !la.ProjectPhaseId.HasValue);
            if (currentLoadAttribute != null)            
                UpdateLoadAttribute(currentLoadAttribute, loadAttribute);
            else
                this.Context.LoadAttributes.InsertOnSubmit(loadAttribute);
            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Inserts any new LoadAttribute object to Db or updates an existing LoadAttribute object (based on LoadId, ResourcePlanId & ProjectPhaseId)
        /// </summary>
        /// <param name="LoadAttribute">LoadAttribute object</param>
        public bool SaveLoadAttributeAgainstPhase(LoadAttribute loadAttribute)
        {
            bool isNewToPhase = false;
            LoadAttribute currentLoadAttribute = this.Context.LoadAttributes.FirstOrDefault(la => la.LoadId == loadAttribute.LoadId &&
                                                                                            la.ResourcePlanId == loadAttribute.ResourcePlanId &&
                                                                                            la.ProjectPhaseId == loadAttribute.ProjectPhaseId);
            if (currentLoadAttribute != null)
                UpdateLoadAttribute(currentLoadAttribute, loadAttribute);
            else
            {
                this.Context.LoadAttributes.InsertOnSubmit(loadAttribute);
                isNewToPhase = true;
            }
            this.Context.SubmitChanges();
            return isNewToPhase;
        }

        /// <summary>
        /// Inserts any new LoadAttribute object to Db or updates an existing LoadAttribute object (based on LoadId, ResourcePlanId & ProjectPhaseId)
        /// Updates input reference param currentGeneratorAttribute with pre update state.  currentLoadAttribute remains null when the object is new 
        /// </summary>
        /// <param name="LoadAttribute">LoadAttribute object</param>
        public bool SaveLoadAttributeAgainstPhase(LoadAttribute newLoadAttribute, ref LoadAttribute currentLoadAttribute)
        {
            bool isNewToPhase = false;
            LoadAttribute exisitingLoadAttribute = this.Context.LoadAttributes.FirstOrDefault(la => la.LoadId == newLoadAttribute.LoadId &&
                                                                                            la.ResourcePlanId == newLoadAttribute.ResourcePlanId &&
                                                                                            la.ProjectPhaseId == newLoadAttribute.ProjectPhaseId);
            if (exisitingLoadAttribute != null)
            {
                currentLoadAttribute = exisitingLoadAttribute.Clone();
                UpdateLoadAttribute(exisitingLoadAttribute, newLoadAttribute);
            }
            else
            {
                this.Context.LoadAttributes.InsertOnSubmit(newLoadAttribute);
                isNewToPhase = true;
            }
            this.Context.SubmitChanges();
            return isNewToPhase;
        }

        private void UpdateLoadAttribute(LoadAttribute currentLoadAttribute, LoadAttribute newLoadAttribute)
        {
            currentLoadAttribute.UserUpdatedId = newLoadAttribute.UserCreatedId;
            currentLoadAttribute.DateUpdated = newLoadAttribute.DateCreated;
            currentLoadAttribute.DateEffective = newLoadAttribute.DateEffective;
            currentLoadAttribute.DateEnd = newLoadAttribute.DateEnd;
            currentLoadAttribute.DateReplaced = newLoadAttribute.DateReplaced;
            currentLoadAttribute.InServiceEndDate = newLoadAttribute.InServiceEndDate;
            currentLoadAttribute.InServiceStartDate = newLoadAttribute.InServiceStartDate;
            currentLoadAttribute.LoadGrossSummerMin = newLoadAttribute.LoadGrossSummerMin;
            currentLoadAttribute.LoadGrossSummerPeak = newLoadAttribute.LoadGrossSummerPeak;
            currentLoadAttribute.LoadGrossWinterMin = newLoadAttribute.LoadGrossWinterMin;
            currentLoadAttribute.LoadGrossWinterPeak = newLoadAttribute.LoadGrossWinterPeak;
            currentLoadAttribute.LoadMw = newLoadAttribute.LoadMw;
            currentLoadAttribute.PowerFactor = newLoadAttribute.PowerFactor;
            currentLoadAttribute.Probability = newLoadAttribute.Probability;
            currentLoadAttribute.ESALoadDemand = newLoadAttribute.ESALoadDemand;
            currentLoadAttribute.LoadGbl = newLoadAttribute.LoadGbl;
            newLoadAttribute.Id = currentLoadAttribute.Id;
        }

        /// <summary>
        /// Saves a Name object to the Db or updates the current name object if one exists (based on Name property)
        /// </summary>
        /// <param name="name">Name object</param>
        /// <returns>db identity</returns>
        public int SaveName(Name name)
        {
            Name currentName = this.Context.Names.FirstOrDefault(n => n.Name1 == name.Name1 && n.NameTypeId == name.NameTypeId && n.LoadId == name.LoadId);
            if (currentName != null)
                return currentName.Id; // name already exists
            else
                this.Context.Names.InsertOnSubmit(name);
            this.Context.SubmitChanges();
            return name.Id;
        }

        /// <summary>
        /// Checks whether the input name is being used to describe an existing GeneratingUnit
        /// </summary>
        /// <param name="name">GeneratingUnit name</param>
        /// <returns>true = name is valid or not is use, false = name is not valid or in use</returns>
        public bool ValidateName(string name, int substationId)
        {
            if (string.IsNullOrEmpty(name)) return false;
            var load = (from l in this.Context.Loads
                           join v in this.Context.VoltageLevels on l.VoltageLevelId equals v.Id
                           join n in this.Context.Names on l.Id equals n.LoadId
                           where v.SubstationId == substationId && n.NameType.Name.ToLower().Equals("long name") && n.Name1.ToLower().Equals(name)
                           select l).FirstOrDefault();
            return load == null;
        }

        public IList<VoltageLevel> GetVoltageLevels(Load load)
        {
            //if (!this.Context.VoltageLevels.Any()) return null;
            //return this.Context.VoltageLevels.ToList();

            if (load.VoltageLevel != null && load.VoltageLevel.Substation != null)
            {
                var voltageLevels = from v in this.Context.VoltageLevels
                                    where v.SubstationId == load.VoltageLevel.SubstationId
                                    select v;
                return voltageLevels != null ? voltageLevels.ToList() : null;
            }
            else
                return null;
        }


        /// <summary>
        /// Retrieve the notes associated with a Load
        /// </summary>
        /// <param name="loadId"></param>
        /// <returns></returns>
        public IList<Note> GetNotes(int loadId)
        {
            return null;

            // TODO: Waiting for linkNotes table to be updated.  
          /*  var notes = from ln in this.Context.LinkNotes.Where(link => link.loadId == loadId)
                        join n in this.Context.Notes on ln.NoteId equals n.Id
                        where n.IsDeleted == false
                        select n;
            return notes != null ? notes.ToList() : null;*/
        }



        #endregion
    }
}
