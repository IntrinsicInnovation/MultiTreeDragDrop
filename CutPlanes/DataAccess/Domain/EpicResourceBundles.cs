using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public class EpicResourceBundles : EpicDomainBase
    {
        #region [Properties]

        public int PlannedStatusId
        {
            get
            {                
                var status = this.Context.ResourceStatuses.FirstOrDefault(rs => rs.Value.ToLower() == "p");
                if (status == null)
                {
                    Log.Error("EpicResourceBundles.PlannedStatusId --> The 'Planned' status cannot be found in the lookup.ResourceStatuses table");
                    return 0;
                }
                return status.Id;
            }
        }

        #endregion

        #region [Constructors]

        public EpicResourceBundles(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        #endregion

        #region [Methods]

        public IList<ResourceBundle> GetList()
        {
            if (!this.Context.ResourceBundles.Any()) return null;
            return this.Context.ResourceBundles.ToList();
        }

        public ResourceBundle Get(int id)
        {
            return this.Context.ResourceBundles.FirstOrDefault(r => r.Id == id);
        }

        /// <summary>
        /// returns the ResourceBundleAttribute object linked to the input LoadId and phaseId
        /// </summary>
        /// <param name="resourceBundleId">resource bundle db identity</param>
        /// <param name="phaseId">project phase id</param>
        /// <returns></returns>
        public ResourceBundleAttribute GetAttribute(int resourceBundleId, int? phaseId)
        {
            return this.Context.ResourceBundleAttributes.FirstOrDefault(ra => ra.ResourceBundleId == resourceBundleId && ra.ProjectPhaseId == phaseId);
        }

        /// <summary>
        /// returns the ResourceBundleAttribute object linked to the Id
        /// </summary>
        /// <param name="id">ResourceBundleAttribute id</param>        
        /// <returns></returns>
        public ResourceBundleAttribute GetAttribute(int id)
        {
            return this.Context.ResourceBundleAttributes.FirstOrDefault(ra => ra.Id == id);
        }

        /// <summary>
        /// returns the ResourceBundleAttribute object linked to the input phaseId
        /// </summary>
        /// <param name="phaseId">project phase id</param>        
        /// <returns></returns>
        public ResourceBundleAttribute GetAttribute(int? phaseId)
        {
            return this.Context.ResourceBundleAttributes.FirstOrDefault(ra => ra.ProjectPhaseId == phaseId);
        }

        /// <summary>
        /// returns the ResourceBundleAttribute object linked to the input LoadId and resource plan id
        /// </summary>
        /// <param name="resourceBundleId">resource bundle db identity</param>
        /// <param name="planId">Resource Plan Id</param>
        /// <returns></returns>
        public ResourceBundleAttribute GetAttribute(int rBId, int planId)
        {
            return this.Context.ResourceBundleAttributes.FirstOrDefault(ra => ra.ResourceBundleId == rBId && ra.ResourcePlanId == planId && !ra.ProjectPhaseId.HasValue);
        }

        public IList<ResourceBundle> GetResourceBundles(int resourcePlanId)
        {
            var resourceBundle = (from r in this.Context.ResourceBundles
                                 join ra in this.Context.ResourceBundleAttributes on r.Id equals ra.ResourceBundleId
                                 where ra.ResourcePlanId == resourcePlanId && !ra.ProjectPhaseId.HasValue
                                 select r).Distinct();

            return resourceBundle != null ? resourceBundle.ToList() : null;
        }

        public IList<ResourceBundle> GetList(int? projectPhaseId)
        {
            var resourceBundle = from r in this.Context.ResourceBundles
                                 join ra in this.Context.ResourceBundleAttributes on r.Id equals ra.ResourceBundleId
                                 where ra.ProjectPhaseId == projectPhaseId
                                 select r;

            return resourceBundle != null ? resourceBundle.ToList() : null;
        }

        /// <summary>
        /// Gets a list of all resource bundles whose phaseId is found in the input array
        /// </summary>
        /// <param name="resourcePlanId">projectPhaseId</param>
        /// <returns>List of ResourceBundle objects</returns>
        public IEnumerable<ResourceBundle> GetList(int[] phaseIds)
        {
            return from r in this.Context.ResourceBundles
                        join ra in this.Context.ResourceBundleAttributes on r.Id equals ra.ResourceBundleId
                        where ra.ProjectPhaseId.HasValue && phaseIds.Contains(ra.ProjectPhaseId.Value)
                        select r;
        }
        /// <summary>
        /// return a list of ResourceBundleAttribute objects where the phase Id is found input phaseId list        
        /// </summary>
        /// <param name="phaseIds">list of phase Ids</param>
        /// <returns>IList<ResourceBundleAttribute></returns>
        public IList<ResourceBundleAttribute> GetResourceBundleAttributesInList(int[] phaseIdList)
        {
            var attributes = this.Context.ResourceBundleAttributes.Where(ra => ra.ProjectPhaseId.HasValue && phaseIdList.Contains(ra.ProjectPhaseId.Value));
            return attributes != null ? attributes.ToList() : null;
        }

        public void Save()
        {
            this.Context.SubmitChanges();
        }

        public void SaveResourceBundle(ResourceBundle rb)
        {            
            ResourceBundle currentRb = this.Context.ResourceBundles.FirstOrDefault(r => r.Id == rb.Id);
            if (currentRb != null)
            {
                currentRb.Comments = rb.Comments;
                currentRb.DateUpdated = rb.DateCreated;
                currentRb.UserUpdatedId = rb.UserCreatedId;
                currentRb.ResourceBundleStatusId = rb.ResourceBundleStatusId;
                currentRb.ElectricalGroupId = rb.ElectricalGroupId;
                currentRb.FuelTypeId = rb.FuelTypeId;
                currentRb.Latitude = rb.Latitude;
                currentRb.Longitude = rb.Longitude;
                currentRb.PowerCall = rb.PowerCall;
                currentRb.PoiCircuitDesignation = rb.PoiCircuitDesignation;
                currentRb.PoiVoltage = rb.PoiVoltage;
                currentRb.PrimaryPoi = rb.PrimaryPoi;
                currentRb.SecondaryPoi = rb.SecondaryPoi;
            }
            else
                this.Context.ResourceBundles.InsertOnSubmit(rb);
            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Saves the input ResourceBundle object.  If a ResourceBundle exsits with an equal name, the existing resource bundle is updated
        /// </summary>
        /// <param name="rb">ResourceBundle object</param>
        public bool Save(ResourceBundle rb)
        {
            bool isNew = true;
            ResourceBundle currentRb = this.Context.ResourceBundles.FirstOrDefault(erb => erb.Names.Any(n => n.Name1 == rb.Name));
            if (currentRb != null)
            {
                currentRb.Comments = rb.Comments;
                currentRb.DateUpdated = rb.DateCreated;
                currentRb.UserUpdatedId = rb.UserCreatedId;
                currentRb.ResourceBundleStatusId = rb.ResourceBundleStatusId;                
                currentRb.FuelTypeId = rb.FuelTypeId;
                currentRb.Latitude = rb.Latitude;
                currentRb.Longitude = rb.Longitude;
                currentRb.PowerCall = rb.PowerCall;
                currentRb.PoiCircuitDesignation = rb.PoiCircuitDesignation;
                currentRb.PoiVoltage = rb.PoiVoltage;
                currentRb.PrimaryPoi = rb.PrimaryPoi;
                currentRb.SecondaryPoi = rb.SecondaryPoi;
                rb.Id = currentRb.Id;
                isNew = false;
            }
            else
                this.Context.ResourceBundles.InsertOnSubmit(rb);
            this.Context.SubmitChanges();
            return isNew;
        }

       /// <summary>
        /// Inserts a new ResourceBundle into the Db if another object by the same name does not exist
       /// </summary>
        /// <param name="rb">ResourceBundle</param>
       /// <returns>bool value indicating wheter the input object was inserted into the db</returns>
        public bool SaveByName(ResourceBundle rb)
        {
            bool isNew = true;
            ResourceBundle currentRb = this.Context.ResourceBundles.FirstOrDefault(erb => erb.Names.Any(n => n.Name1 == rb.Name));
            if (currentRb != null)
            {
                currentRb.DateUpdated = rb.DateCreated;
                currentRb.UserUpdatedId = rb.UserCreatedId;
                rb.Id = currentRb.Id;
                isNew = false;
            }
            else
                this.Context.ResourceBundles.InsertOnSubmit(rb);
            this.Context.SubmitChanges();
            return isNew;
        }

        /// <summary>
        /// Checks if a db record exists with the same ResourceBundleAttribute identity.
        /// If a records exists, that record is updated.  If no record exists one is created.
        /// </summary>
        /// <param name="rba">ResourceBundleAttribute</param>
        public void Save(ResourceBundleAttribute rba)
        {
            ResourceBundleAttribute currentRba = this.Context.ResourceBundleAttributes.FirstOrDefault(ra => ra.ResourceBundleId == rba.ResourceBundleId && ra.ResourcePlanId == rba.ResourcePlanId && (!ra.ProjectPhaseId.HasValue || ra.ProjectPhaseId == rba.ProjectPhaseId));
            if (currentRba != null)
            {
                currentRba.DateUpdated = DateTime.Now;
                currentRba.UserUpdatedId = rba.UserCreatedId;
                currentRba.CommercialOperationEndDate = rba.CommercialOperationEndDate;
                currentRba.CommercialOperationStartDate = rba.CommercialOperationStartDate;
                currentRba.DateEffective = rba.DateEffective;
                currentRba.DateEnd = rba.DateEnd;
                currentRba.DateReplaced = rba.DateReplaced;
                currentRba.DependableGenerationCapacity = rba.DependableGenerationCapacity;
                currentRba.EffectiveLoadCarryingCapacity = rba.EffectiveLoadCarryingCapacity;
                currentRba.OriginalId = rba.OriginalId;
                currentRba.PotentialIncludeCapacity = rba.PotentialIncludeCapacity;
                currentRba.SystemCapacity = rba.SystemCapacity;
                currentRba.MaximumPowerOutputCapacity = rba.MaximumPowerOutputCapacity;
                currentRba.NitsDesignatedCapacity = rba.NitsDesignatedCapacity;
                currentRba.NamePlateCapacityInMW = rba.NamePlateCapacityInMW;
                currentRba.IsDesignated = rba.IsDesignated;
                rba.Id = currentRba.Id;
            }
            else
                this.Context.ResourceBundleAttributes.InsertOnSubmit(rba);
            this.Context.SubmitChanges();
        }

        public void SaveResourceBundleAttributeAgainstPhase(ResourceBundleAttribute rba)
        {
            ResourceBundleAttribute currentRba = this.Context.ResourceBundleAttributes.FirstOrDefault(ra => ra.ResourcePlanId == rba.ResourcePlanId &&
                                                                                                                          ra.ProjectPhaseId == rba.ProjectPhaseId &&
                                                                                                                          ra.ResourceBundleId == rba.ResourceBundleId);
            if (currentRba != null)
            {
                currentRba.DateUpdated = DateTime.Now;
                currentRba.UserUpdatedId = rba.UserCreatedId;
                currentRba.CommercialOperationEndDate = rba.CommercialOperationEndDate;
                currentRba.CommercialOperationStartDate = rba.CommercialOperationStartDate;
                currentRba.DateEffective = rba.DateEffective;
                currentRba.DateEnd = rba.DateEnd;
                currentRba.DateReplaced = rba.DateReplaced;
                currentRba.DependableGenerationCapacity = rba.DependableGenerationCapacity;
                currentRba.EffectiveLoadCarryingCapacity = rba.EffectiveLoadCarryingCapacity;
                currentRba.OriginalId = rba.OriginalId;
                currentRba.PotentialIncludeCapacity = rba.PotentialIncludeCapacity;
                currentRba.SystemCapacity = rba.SystemCapacity;
                currentRba.MaximumPowerOutputCapacity = rba.MaximumPowerOutputCapacity;
                currentRba.NitsDesignatedCapacity = rba.NitsDesignatedCapacity;
                currentRba.NamePlateCapacityInMW = rba.NamePlateCapacityInMW;
                currentRba.IsDesignated = rba.IsDesignated;
            }
            else
                this.Context.ResourceBundleAttributes.InsertOnSubmit(rba);
            this.Context.SubmitChanges();
        }

        public void SaveResourceBundleAttributeAgainstPhase(ResourceBundleAttribute newRba, ref ResourceBundleAttribute currentRba)
        {
            ResourceBundleAttribute existingRba = this.Context.ResourceBundleAttributes.FirstOrDefault(ra => ra.ResourcePlanId == newRba.ResourcePlanId &&
                                                                                                                          ra.ProjectPhaseId == newRba.ProjectPhaseId &&
                                                                                                                          ra.ResourceBundleId == newRba.ResourceBundleId);
            if (existingRba != null)
            {
                currentRba = existingRba.Clone();
                existingRba.DateUpdated = DateTime.Now;
                existingRba.UserUpdatedId = newRba.UserCreatedId;
                existingRba.CommercialOperationEndDate = newRba.CommercialOperationEndDate;
                existingRba.CommercialOperationStartDate = newRba.CommercialOperationStartDate;
                existingRba.DateEffective = newRba.DateEffective;
                existingRba.DateEnd = newRba.DateEnd;
                existingRba.DateReplaced = newRba.DateReplaced;
                existingRba.DependableGenerationCapacity = newRba.DependableGenerationCapacity;
                existingRba.EffectiveLoadCarryingCapacity = newRba.EffectiveLoadCarryingCapacity;
                existingRba.OriginalId = newRba.OriginalId;
                existingRba.PotentialIncludeCapacity = newRba.PotentialIncludeCapacity;
                existingRba.SystemCapacity = newRba.SystemCapacity;
                existingRba.NitsDesignatedCapacity = newRba.NitsDesignatedCapacity;
                existingRba.NamePlateCapacityInMW = newRba.NamePlateCapacityInMW;
                existingRba.IsDesignated = newRba.IsDesignated;
            }
            else
                this.Context.ResourceBundleAttributes.InsertOnSubmit(newRba);
            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Checks whether the input name is being used to describe an existing ResourceBundle
        /// </summary>
        /// <param name="tla">ResourceBundle Name</param>
        /// <returns>true = name is valid or not is use, false = not valid or in use</returns>
        public bool ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            return !this.Context.Names.Any(n => n.Name1.ToLower().Equals(name.ToLower()) && n.NameType.Name.ToLower().Equals("long name") && n.ResourceBundleId.HasValue);
        }

        /// <summary>
        /// Checks whether the input name is being used to describe an existing ResourceBundle
        /// </summary>
        /// <param name="tla">ResourceBundle Name</param>
        /// <returns>true = name is valid or not is use, false = not valid or in use</returns>
        public bool ValidateName(string name, int rbId)
        {
            if (string.IsNullOrEmpty(name)) return false;
            return !this.Context.Names.Any(n => n.Name1.ToLower().Equals(name.ToLower()) && n.NameType.Name.ToLower().Equals("long name") && n.ResourceBundleId != rbId);
        }

        /// <summary>
        /// Checks whether the input tla is being used to describe an existing ResourceBundle
        /// </summary>
        /// <param name="tla">ResourceBundle Tla</param>
        /// <returns>true = tla is valid or not is use, false = not valid or in use</returns>
        public bool ValidateTla(string tla)
        {
            if (string.IsNullOrEmpty(tla)) return false;
            return !this.Context.Names.Any(n => n.Name1.ToLower().Equals(tla.ToLower()) && n.NameType.Name.ToLower().Equals("tla") && n.ResourceBundleId.HasValue);
        }

        /// <summary>
        /// Checks whether the input tla is being used to describe an existing ResourceBundle
        /// </summary>
        /// <param name="tla">ResourceBundle Tla</param>
        /// <returns>true = tla is valid or not is use, false = not valid or in use</returns>
        public bool ValidateTla(string tla, int rbId)
        {
            if (string.IsNullOrEmpty(tla)) return false;
            return !this.Context.Names.Any(n => n.Name1.ToLower().Equals(tla.ToLower()) && n.NameType.Name.ToLower().Equals("tla") && n.ResourceBundleId != rbId);
        }

        public void SaveEGLink(LinkResourceBundleElectricalGroup link)
        {
            if(this.Context.LinkResourceBundleElectricalGroups.Any(el => el.ElectricalGroupId == link.ElectricalGroupId && el.ResourceBundleId == link.ResourceBundleId))
                return;
            this.Context.LinkResourceBundleElectricalGroups.InsertOnSubmit(link);
            this.Context.SubmitChanges();
        }
        

        public void SaveName(Name name)
        {
            Name currentName = this.Context.Names.FirstOrDefault(n => n.Name1 == name.Name1 && n.NameTypeId == name.NameTypeId && n.ResourceBundleId == name.ResourceBundleId);
            if (currentName != null)
                return; // name already exists
            else
            {
                var activeNames = this.Context.Names.Where(n => n.ResourceBundleId == name.ResourceBundleId && n.NameTypeId == name.NameTypeId && n.IsActive);
                foreach (Name n in activeNames)
                    n.IsActive = false;
                this.Context.Names.InsertOnSubmit(name);
            }
            this.Context.SubmitChanges();
        }

        public IEnumerable<ResourceBundle> GetBundles(int fuelTypeId, int elecGroupId, int dataSourceId, decimal uECStart, decimal uECEnd)
        {
            return  (from rb in this.Context.ResourceBundles
                    join lnk in this.Context.LinkResourceBundleElectricalGroups on rb.Id equals lnk.ResourceBundleId
                    join rba in this.Context.ResourceBundleAttributes on rb.Id equals rba.ResourceBundleId
                    join roa in this.Context.ResourceOptionAttributes on rba.Id equals roa.ResourceBundleAttributeId
                    where rb.FuelTypeId == fuelTypeId && lnk.ElectricalGroupId == elecGroupId && rba.UnitEnergyCostStart == uECStart
                        && rba.UnitEnergyCostEnd == uECEnd && roa.DataSourceId == dataSourceId 
                    select rb).Distinct();
        }

        public IEnumerable<ResourceBundle> GetBundles(int planId)
        {
            return (from rb in this.Context.ResourceBundles
                   join rba in this.Context.ResourceBundleAttributes on rb.Id equals rba.ResourceBundleId
                   where rba.ResourcePlanId == planId
                   select rb).Distinct();
        }

        public IEnumerable<ResourceBundle> GetBundles(int? dataSourceId)
        {
            return (from rb in this.Context.ResourceBundles
                   join rba in this.Context.ResourceBundleAttributes on rb.Id equals rba.ResourceBundleId
                   join rbo in this.Context.ResourceOptionAttributes on rba.Id equals rbo.ResourceBundleAttributeId
                   where rbo.DataSourceId == dataSourceId
                   select rb).Distinct();
        }

        /// <summary>
        /// Deletes the resource bundle linked to the input id (including all linked entities) from the DB 
        /// </summary>
        /// <param name="id">resource bundle id</param>
        public void DeleteBundle(int id)
        {
            var rB = this.Context.ResourceBundles.FirstOrDefault(erb => erb.Id == id);
            if (rB == null) return;
            var links = rB.LinkResourceBundleElectricalGroups;
            if (links.Count() > 0)
                this.Context.LinkResourceBundleElectricalGroups.DeleteAllOnSubmit(links);
            var rBAs = rB.ResourceBundleAttributes;
            if (rBAs.Count() > 0)
                this.Context.ResourceBundleAttributes.DeleteAllOnSubmit(rBAs);
            this.Context.Names.DeleteAllOnSubmit(rB.Names);
            this.Context.ResourceBundles.DeleteOnSubmit(rB);
            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Delete the input collection of resource bundles (including all linked entities) from the DB 
        /// </summary>
        /// <param name="rBs">collection of resource bundles</param>
        public void DeleteBundles(IEnumerable<ResourceBundle> rBs)
        {            
            if (rBs.Count() == 0) return;
            foreach (var rB in rBs)
            {
                this.Context.LinkResourceBundleElectricalGroups.DeleteAllOnSubmit(rB.LinkResourceBundleElectricalGroups);
                this.Context.ResourceBundleAttributes.DeleteAllOnSubmit(rB.ResourceBundleAttributes);
            }
            this.Context.ResourceBundles.DeleteAllOnSubmit(rBs);
            this.Context.SubmitChanges();
        }

        public void DeleteAttribute(int resourceBundleId, int resourcePlanId, int? phaseId)
        {
            ResourceBundleAttribute resourceBundleAttribute = this.Context.ResourceBundleAttributes.FirstOrDefault(rb => rb.ResourceBundleId == resourceBundleId &&
                                                                                                                    rb.ResourcePlanId == resourcePlanId &&
                                                                                                                    rb.ProjectPhaseId == phaseId);
            if (resourceBundleAttribute == null) return;
            this.Context.ResourceBundleAttributes.DeleteOnSubmit(resourceBundleAttribute);
            this.Context.SubmitChanges();
        }

        public void DeleteAttributes(int phaseId)
        {
            var resourceBundleAttributes = this.Context.ResourceBundleAttributes.Where(ra => ra.ProjectPhaseId == phaseId);
            if (resourceBundleAttributes == null) return;
            this.Context.ResourceBundleAttributes.DeleteAllOnSubmit(resourceBundleAttributes);
            this.Context.SubmitChanges();
        }

        public IList<ResourceBundleAttribute> GetResourceBundleAttributeList(int resourcePlanId, int? phaseId)
        {
            var resourceBundleAttributes = this.Context.ResourceBundleAttributes.Where(ra => ra.ResourcePlanId == resourcePlanId && ra.ProjectPhaseId == phaseId);
            return resourceBundleAttributes != null ? resourceBundleAttributes.ToList() : null;
        }

        public IList<ResourceBundle> GetResourceBundlesForResourcePlan(int resourcePlanId)
        {
            var resourceBundle = (from r in this.Context.ResourceBundles
                                  join ra in this.Context.ResourceBundleAttributes on r.Id equals ra.ResourceBundleId
                                  where ra.ResourcePlanId == resourcePlanId
                                  select r).Distinct();

            return resourceBundle != null ? resourceBundle.ToList() : null;
        }

        public IList<ResourceBundleAttribute> GetResourceBundleAttributeList(int? phaseId)
        {
            var resourceBundleAttributes = this.Context.ResourceBundleAttributes.Where(ra => ra.ProjectPhaseId == phaseId);
            return resourceBundleAttributes != null ? resourceBundleAttributes.ToList() : null;
        }

        /// <summary>
        /// Return a list of resource plan objects linked to the input resource Bundle id
        /// </summary>
        /// <param name="rBId">resourceBundle identity</param>
        /// <returns>list of resource plan objects</returns>
        public IList<ResourcePlan> GetResourcePlans(int rBId)
        {
            var plans = (from rp in this.Context.ResourcePlans
                         join rba in this.Context.ResourceBundleAttributes on rp.Id equals rba.ResourcePlanId
                         where rba.ResourceBundleId == rBId
                         select rp).Distinct();
            return plans != null ? plans.OrderBy(rp => rp.PlanName).ToList() : null;
        }

        /// <summary>
        /// Get the Annotation linked to the input ResourceBundleId
        /// </summary>
        /// <param name="rbId">ResourceBundleId</param>
        /// <returns>List of Notes</returns>
        public IList<Note> GetNotes(int rbId)
        {
            var notes = from ln in this.Context.LinkNotes.Where(link => link.ResourceBundleId == rbId)
                        join n in this.Context.Notes on ln.NoteId equals n.Id
                        where n.IsDeleted == false
                        select n;
            return notes != null ? notes.ToList() : null;
        }

        public void SaveChanges()
        {
            this.Context.SubmitChanges();
        }

        #endregion
    }
}
