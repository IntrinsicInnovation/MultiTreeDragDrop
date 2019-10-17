using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpicDataAccess;
using EpicDataAccess.Domain;

namespace EpicDataAccess.Domain
{
    public class EpicGeneratingUnits : EpicDomainBase
    { 
        #region [Properties]
          

        #endregion

        #region [Constructors]

        public EpicGeneratingUnits(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Retrieves a GeneratingUnit object based on input identity
        /// </summary>
        /// <param name="id">GeneratingUnit identity</param>
        /// <returns>GeneratingUnit object</returns>
        public GeneratingUnit GetGeneratingUnit(int id)
        {
            return this.Context.GeneratingUnits.FirstOrDefault(gu => gu.Id == id);
        }

        /// <summary>
        /// Returns a  GeneratorAttribute object linked to the input ResourcePlanId and generatingUnitId
        /// </summary>
        /// <param name="resourcePlanId">ResourcePlan DB Id</param>
        /// <param name="generatingUnitId">generatingUnit DB Id</param>
        /// <returns> GeneratorAttribute object</returns>
        public GeneratorAttribute GetGeneratorAttribute(int generatingUnitId, int resourcePlanId)
        {
            return this.Context.GeneratorAttributes.FirstOrDefault(ga => ga.GeneratingUnitId == generatingUnitId &&
                                                                    ga.ResourcePlanId == resourcePlanId && !ga.ProjectPhaseId.HasValue);
        }

        /// <summary>
        /// Returns a  GeneratorAttribute object given a GeneratorAttributeId
        /// </summary>
        /// <param name="generatingUnitAttributeId">generatingUnitAttributeId DB Id</param>        
        /// <returns> GeneratorAttribute object</returns>
        public GeneratorAttribute GetGeneratorAttribute(int generatingUnitAttributeId)
        {
            return this.Context.GeneratorAttributes.FirstOrDefault(ga => ga.Id == generatingUnitAttributeId);
        }

        /// <summary>
        /// Returns a  GeneratorAttribute object linked to the input Project Phase Id and generatingUnitId
        /// </summary>
        /// <param name="resourcePlanId">Project Phase Id</param>
        /// <param name="generatingUnitId">generatingUnit DB Id</param>
        /// <returns> GeneratorAttribute object</returns>
        public GeneratorAttribute GetGeneratorAttributeByPhase(int generatingUnitId, int? phaseId)
        {
            return this.Context.GeneratorAttributes.FirstOrDefault(ga => ga.GeneratingUnitId == generatingUnitId && ga.ProjectPhaseId == phaseId);
        }

        public IList<GeneratorAttribute> GetGeneratorAttributeList(int resourcePlanId, int? phaseId)
        {
            var attributes = this.Context.GeneratorAttributes.Where(gua => gua.ResourcePlanId == resourcePlanId && gua.ProjectPhaseId == phaseId);
            return attributes != null ? attributes.ToList() : null;
        }

        public IList<GeneratorAttribute> GetGeneratorAttributeList(int? phaseId)
        {
            var attributes = this.Context.GeneratorAttributes.Where(gua => gua.ProjectPhaseId == phaseId);
            return attributes != null ? attributes.ToList() : null;
        }

        public IList<GeneratorAttribute> GetGeneratorAttributeList()
        {
            var attributes = (from ga in this.Context.GeneratorAttributes
                              where !ga.ProjectPhaseId.HasValue 
                              select ga).Distinct();

            return attributes != null ? attributes.ToList() : null;
        }

        /// <summary>
        /// returns a distinct list of all ODS GeneratorAttributes which which are nits nominated,
        /// are nits nominated and belong to the input resource plan
        /// </summary>
        /// <returns></returns>
        public IList<GeneratorAttribute> GetODSGeneratorAttributeList(int resourcePlanId)
        {
            var attributes = (from ga in this.Context.GeneratorAttributes
                              where !ga.ProjectPhaseId.HasValue && ga.ResourcePlanId == resourcePlanId && ga.IsNitsNominated.HasValue && ga.IsNitsNominated.Value
                              select ga).Distinct();

            return attributes != null ? attributes.ToList() : null;
        }

        /// <summary>
        /// return a list of generatorattribute objects where the generating Unit Id is not found input generating Unit Ids
        /// and are nits niminated
        /// </summary>
        /// <param name="generatingUnitIdBlackList">list of generatingUnit ids</param>
        /// <returns>IList<GeneratorAttribute></returns>
        public IList<GeneratorAttribute> GetGeneratorAttributesNotInList(int[] generatingUnitIdBlackList, int resourcePlanId)
        {
            var attributes = this.Context.GeneratorAttributes.Where(ga => !ga.ProjectPhaseId.HasValue && !generatingUnitIdBlackList.Contains(ga.GeneratingUnitId) && ga.ResourcePlanId == resourcePlanId && ga.IsNitsNominated.HasValue && ga.IsNitsNominated.Value).Distinct();
            return attributes != null ? attributes.ToList() : null;
        }

        /// <summary>
        /// return a list of generatorattribute objects where the phase Id is found input phaseId list        
        /// </summary>
        /// <param name="phaseIds">list of phase Ids</param>
        /// <returns>IList<GeneratorAttribute></returns>
        public IList<GeneratorAttribute> GetGeneratorAttributesInList(IList<int> phaseIds)
        {
            var attributes = this.Context.GeneratorAttributes.Where(ga => ga.ProjectPhaseId.HasValue && phaseIds.ToArray().Contains(ga.ProjectPhaseId.Value));
            return attributes != null ? attributes.ToList() : null;
        }


        /// <summary>
        /// Deletes all GeneratorAttribute objects linked to the input substationId & phaseId
        /// </summary>
        /// <param name="substationId"></param>
        /// <param name="phaseId"></param>
        public void DeleteAttribute(int substationId, int? phaseId)
        {
            var generatingUnits = from g in this.Context.GeneratingUnits
                                  join ga in this.Context.GeneratorAttributes on g.Id equals ga.GeneratingUnitId
                                  join v in this.Context.VoltageLevels on g.VoltageLevelId equals v.Id
                                  where v.SubstationId == substationId && ga.ProjectPhaseId == phaseId
                                  select ga;

            if (generatingUnits == null) return;
            this.Context.GeneratorAttributes.DeleteAllOnSubmit(generatingUnits);
            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Deletes all GeneratorAttribute objects linked to the input phaseId
        /// </summary>
        /// <param name="substationId"></param>
        /// <param name="phaseId"></param>
        public void DeleteAttributes(int phaseId)
        {
            var generatingUnits = this.Context.GeneratorAttributes.Where(ga => ga.ProjectPhaseId == phaseId);
            if (generatingUnits == null) return;
            this.Context.GeneratorAttributes.DeleteAllOnSubmit(generatingUnits);
            this.Context.SubmitChanges();
        }

        public void DeleteAttribute(int generatingUnitId, int resourcePlanId, int? phaseId)
        {
            GeneratorAttribute generatorAttribute = this.Context.GeneratorAttributes.FirstOrDefault(ga => ga.GeneratingUnitId == generatingUnitId &&
                                                                                                    ga.ResourcePlanId == resourcePlanId &&
                                                                                                    ga.ProjectPhaseId == phaseId);
            if (generatorAttribute == null) return;
            this.Context.GeneratorAttributes.DeleteOnSubmit(generatorAttribute);
            this.Context.SubmitChanges();
        }

        public bool SetPowerCall(int gid, int? pc)
        {
            GeneratingUnit gen = (from g in this.Context.GeneratingUnits.Where(x => x.Id.Equals(gid))
                                                    select g).FirstOrDefault();

            gen.PowerCallId = pc;

            this.Context.SubmitChanges();
            return true;
        }


        /// <summary>
        /// Inserts any new GeneratingUnit objects to Db and updates any existing GeneratingUnit objects (based on GeneratingUnit Name)
        /// </summary>
        /// <param name="generatingUnit">GeneratingUnit objects</param>
        public void SaveGeneratingUnit(GeneratingUnit generatingUnit)
        {
            GeneratingUnit currentGeneratingUnit = (from g in this.Context.GeneratingUnits.Where(gen => gen.Id.Equals(generatingUnit.Id))
                                            select g).FirstOrDefault();


            if (currentGeneratingUnit != null)
            {
                // we are updaing an existing GeneratingUnit
                currentGeneratingUnit.DateUpdated = DateTime.Now;
                currentGeneratingUnit.UserUpdatedId = generatingUnit.UserCreatedId;
                currentGeneratingUnit.Description = generatingUnit.Description;
                currentGeneratingUnit.EnergySourceId = generatingUnit.EnergySourceId;
                currentGeneratingUnit.GeneratorStatusID = generatingUnit.GeneratorStatusID;
                currentGeneratingUnit.GeneratorTypeID = generatingUnit.GeneratorTypeID;
                currentGeneratingUnit.InitialP = generatingUnit.InitialP;
                currentGeneratingUnit.IsSyncCondenserCapable = generatingUnit.IsSyncCondenserCapable;

                currentGeneratingUnit.ManufacturerId = generatingUnit.ManufacturerId;
                currentGeneratingUnit.Model = generatingUnit.Model;
                currentGeneratingUnit.VoltageLevelId = generatingUnit.VoltageLevelId;
                currentGeneratingUnit.EnergySourceId = generatingUnit.EnergySourceId;
                currentGeneratingUnit.PowerCallId = generatingUnit.PowerCallId;
                currentGeneratingUnit.NumberOfPolePairs = generatingUnit.NumberOfPolePairs;
                currentGeneratingUnit.SerialNumber = generatingUnit.SerialNumber;
                //generatingUnit.Id = currentGeneratingUnit.Id;

                // EMS Data
                currentGeneratingUnit.MaxOperatingP = currentGeneratingUnit.MaxOperatingP;
                currentGeneratingUnit.MinOperatingP = currentGeneratingUnit.MinOperatingP;
                currentGeneratingUnit.NormalPF = generatingUnit.NormalPF;
                currentGeneratingUnit.ShortPF = generatingUnit.ShortPF;
                currentGeneratingUnit.LongPF = currentGeneratingUnit.LongPF;
                currentGeneratingUnit.InitialP = generatingUnit.InitialP;
                this.Context.SubmitChanges();

            }
            else
            {
                // we are creating a new GeneratingUnit

                this.Context.GeneratingUnits.InsertOnSubmit(generatingUnit);
                this.Context.SubmitChanges();
                
            }
            
        }

        /// <summary>
        /// Inserts any new GeneratorAttribute objects to Db and updates any existing GeneratorAttribute objects (based on gen unit Id, plan id & phase id)
        /// </summary>
        /// <param name="generatorAttribute">GeneratorAttribute object</param>
        public void Save(GeneratorAttribute ga)
        {
            GeneratorAttribute currentGa = this.Context.GeneratorAttributes.FirstOrDefault(gua => gua.GeneratingUnitId == ga.GeneratingUnitId && gua.ResourcePlanId == ga.ResourcePlanId && (!gua.ProjectPhaseId.HasValue || gua.ProjectPhaseId == ga.ProjectPhaseId));
            if (currentGa != null)            
                UpdateGeneratorAttribute(currentGa, ga);            
            else
            {
                ga.DateCreated = DateTime.Now;
                this.Context.GeneratorAttributes.InsertOnSubmit(ga);
            }
            this.Context.SubmitChanges();
        }

  
        /// <summary>
        /// Inserts any new GeneratorAttribute objects to Db and updates any existing GeneratorAttribute objects (based on GeneratingUnitId)
        /// </summary>
        /// <param name="generatorAttribute">GeneratorAttribute object</param>
        public void SaveGeneratorAttribute(GeneratorAttribute generatorAttribute)
        {
            GeneratorAttribute currentGeneratorAttribute = this.Context.GeneratorAttributes.FirstOrDefault(ga => ga.GeneratingUnitId == generatorAttribute.GeneratingUnitId && ga.ResourcePlanId == generatorAttribute.ResourcePlanId && !ga.ProjectPhaseId.HasValue);
            if (currentGeneratorAttribute != null)
            {
                UpdateGeneratorAttribute(currentGeneratorAttribute, generatorAttribute);
            }
            else
            {
                generatorAttribute.DateCreated = DateTime.Now;
                this.Context.GeneratorAttributes.InsertOnSubmit(generatorAttribute);
            }
            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Inserts any new GeneratorAttribute objects to Db and updates any existing GeneratorAttribute objects (based on GeneratingUnitId, resourcsPlanId & projectPhaseId)
        /// </summary>
        /// <param name="generatorAttribute">GeneratorAttribute object</param>
        public bool SaveAttributeAgainstPhase(GeneratorAttribute generatorAttribute)
        {
            bool newToPhase = false;
            GeneratorAttribute currentGeneratorAttribute = this.Context.GeneratorAttributes.FirstOrDefault(ga => ga.GeneratingUnitId == generatorAttribute.GeneratingUnitId &&
                                                                                                           ga.ResourcePlanId == generatorAttribute.ResourcePlanId &&
                                                                                                           ga.ProjectPhaseId == generatorAttribute.ProjectPhaseId);
            if (currentGeneratorAttribute != null)
                UpdateGeneratorAttribute(currentGeneratorAttribute, generatorAttribute);
            else
            {
                this.Context.GeneratorAttributes.InsertOnSubmit(generatorAttribute);
                newToPhase = true;
            }            
            this.Context.SubmitChanges();
           
            return newToPhase;
        }

        /// <summary>
        /// Inserts any new GeneratorAttribute objects to Db and updates any existing GeneratorAttribute objects (based on GeneratingUnitId, resourcsPlanId & projectPhaseId)
        /// Updates input reference param currentGeneratorAttribute with pre update state.  currentGeneratorAttribute remains null when the object is new 
        /// </summary>
        /// <param name="generatorAttribute">GeneratorAttribute object</param>
        public bool SaveAttributeAgainstPhase(GeneratorAttribute newGeneratorAttribute, ref GeneratorAttribute currentGeneratorAttribute)
        {
            bool newToPhase = false;
            GeneratorAttribute existingGeneratorAttribute = this.Context.GeneratorAttributes.FirstOrDefault(ga => ga.GeneratingUnitId == newGeneratorAttribute.GeneratingUnitId &&
                                                                                                           ga.ResourcePlanId == newGeneratorAttribute.ResourcePlanId &&
                                                                                                           ga.ProjectPhaseId == newGeneratorAttribute.ProjectPhaseId);
            if (existingGeneratorAttribute != null)
            {
                currentGeneratorAttribute = existingGeneratorAttribute.Clone();                
                UpdateGeneratorAttribute(existingGeneratorAttribute, newGeneratorAttribute);
            }
            else
            {
                this.Context.GeneratorAttributes.InsertOnSubmit(newGeneratorAttribute);
                newToPhase = true;
            }
            this.Context.SubmitChanges();
            return newToPhase;
        }

        private void UpdateGeneratorAttribute(GeneratorAttribute currentGeneratorAttribute, GeneratorAttribute newGeneratorAttribute)
        {
            currentGeneratorAttribute.DateUpdated = DateTime.Now;
            currentGeneratorAttribute.UserUpdatedId = newGeneratorAttribute.UserCreatedId;
            currentGeneratorAttribute.DateEnd = newGeneratorAttribute.DateEnd;
            currentGeneratorAttribute.DateReplaced = newGeneratorAttribute.DateReplaced;            
            currentGeneratorAttribute.DependableGeneratingCapacity = newGeneratorAttribute.DependableGeneratingCapacity;
            currentGeneratorAttribute.EffectiveLoadCarryingCapacity = newGeneratorAttribute.EffectiveLoadCarryingCapacity;
            currentGeneratorAttribute.EPACapacity = newGeneratorAttribute.EPACapacity;
            currentGeneratorAttribute.InServiceEndDate = newGeneratorAttribute.InServiceEndDate;
            currentGeneratorAttribute.InServiceStartDate = newGeneratorAttribute.InServiceStartDate;
            currentGeneratorAttribute.IsNitsNominated = newGeneratorAttribute.IsNitsNominated;
            currentGeneratorAttribute.MaximumPowerOutput = newGeneratorAttribute.MaximumPowerOutput;
            currentGeneratorAttribute.MaxOutputForFirmTransmission = newGeneratorAttribute.MaxOutputForFirmTransmission;
            currentGeneratorAttribute.MaxOutputForNonFirmTransmission = newGeneratorAttribute.MaxOutputForNonFirmTransmission;
            currentGeneratorAttribute.MaxTakeOrPayCapacity = newGeneratorAttribute.MaxTakeOrPayCapacity;
            currentGeneratorAttribute.MonthlyAverageCapacity = newGeneratorAttribute.MonthlyAverageCapacity;
            currentGeneratorAttribute.NamePlateCapacityInMW = newGeneratorAttribute.NamePlateCapacityInMW;
            currentGeneratorAttribute.NitsDesignatedCapacity = newGeneratorAttribute.NitsDesignatedCapacity;
            currentGeneratorAttribute.PhysicalMinimumOutput = newGeneratorAttribute.PhysicalMinimumOutput;
            currentGeneratorAttribute.PointToPointCapacity = newGeneratorAttribute.PointToPointCapacity;
            currentGeneratorAttribute.Probability = newGeneratorAttribute.Probability;
            currentGeneratorAttribute.RatedMVA = newGeneratorAttribute.RatedMVA;
            currentGeneratorAttribute.RatedOverExcitedPowerFactor = newGeneratorAttribute.RatedOverExcitedPowerFactor;
            currentGeneratorAttribute.RatedPowerFactor = newGeneratorAttribute.RatedPowerFactor;
            currentGeneratorAttribute.RatedUnderExcitedPowerFactor = newGeneratorAttribute.RatedUnderExcitedPowerFactor;
            currentGeneratorAttribute.RegulatoryMinimumOutput = newGeneratorAttribute.RegulatoryMinimumOutput;
            currentGeneratorAttribute.ReliabilityMustRunCapacity = newGeneratorAttribute.ReliabilityMustRunCapacity;
            currentGeneratorAttribute.SystemCapacity = newGeneratorAttribute.SystemCapacity;
            currentGeneratorAttribute.LeadingPowerFactor = newGeneratorAttribute.LeadingPowerFactor;
            currentGeneratorAttribute.LaggingPowerFactor = newGeneratorAttribute.LaggingPowerFactor;

            newGeneratorAttribute.Id = currentGeneratorAttribute.Id;
        }

        /// <summary>
        /// Saves a Name object to the Db or updates the current name object if one exists (based on Name property)
        /// </summary>
        /// <param name="name">Name object</param>        
        public void SaveName(Name name)
        {
            Name currentName = this.Context.Names.FirstOrDefault(n => n.Name1 == name.Name1 && n.NameTypeId == name.NameTypeId && n.GeneratingUnitId == name.GeneratingUnitId);
            if (currentName != null)            
                return; // name already exists            
            else
                this.Context.Names.InsertOnSubmit(name);
            this.Context.SubmitChanges();            
        }

        /// <summary>
        /// Returns a List of all GeneratingUnit objects linked to the input SubstationId
        /// </summary>
        /// <param name="resourcePlanId">Substation DB Id</param>
        /// <returns>List of GeneratingUnit objects </returns>
        public IList<GeneratingUnit> GetGeneratingUnits(int substationId)
        {
            var generatingUnits = from g in this.Context.GeneratingUnits
                                  join v in this.Context.VoltageLevels on g.VoltageLevelId equals v.Id
                                  where v.SubstationId == substationId
                                  select g;
            return generatingUnits != null ? generatingUnits.ToList() : null;
        }

        /// <summary>
        /// Returns GeneratingUnit object with a give name, linked to a particular substation
        /// </summary>
        /// <param name="substationId">Substation PK Id</param>
        /// <param name="name">The name to search for</param>
        /// 
        /// <returns>List of GeneratingUnit objects </returns>
        public GeneratingUnit GetGeneratingUnitByName(int substationId, string searchName)
        {
            var genUnits = GetGeneratingUnits(substationId);
            
            foreach (GeneratingUnit gu in genUnits) {
                 var names = GetNames(gu.Id);
                 
                 foreach (Name n in names) {
                     if (n.Name1 == searchName) {
                         return gu;
                     }
                 }
            }
            return null;
        }

        /// <summary>
        /// Returns a List of all names for a particular GeneratingUnit
        /// </summary>
        /// <param name="generatingUnitId">The generating unit to get the names of</param>
        /// <returns>List of Name objects </returns>
        public IList<Name> GetNames(int generatingUnitId)
        {
            var names = from n in this.Context.Names
                             where n.GeneratingUnitId == generatingUnitId
                             select n;

            return names != null ? names.ToList() : null;
        }



        /// <summary>
        /// Returns a List of all GeneratingUnit objects linked to the input SubstationId
        /// </summary>
        /// <param name="resourcePlanId">Substation DB Id</param>
        /// <returns>List of GeneratingUnit objects </returns>
        public IList<GeneratingUnit> GetGeneratingUnits(int substationId, int resourcePlanId)
        {
            var generatingUnits = (from g in this.Context.GeneratingUnits
                                  join ga in this.Context.GeneratorAttributes on g.Id equals ga.GeneratingUnitId
                                  join v in this.Context.VoltageLevels on g.VoltageLevelId equals v.Id
                                  where v.SubstationId == substationId && ga.ResourcePlanId == resourcePlanId
                                  && !ga.ProjectPhaseId.HasValue
                                  select g).Distinct();
            return generatingUnits != null ? generatingUnits.ToList() : null;
        }




        /// <summary>
        /// Returns a List of all GeneratingUnit objects linked to the input SubstationId and phaseId
        /// </summary>
        /// <param name="resourcePlanId">Substation DB Id</param>
        /// <param name="phaseId">project phase Id</param>
        /// <returns>List of GeneratingUnit objects </returns>
        public IList<GeneratingUnit> GetGeneratingUnits(int substationId, int? phaseId)
        {
            var generatingUnits = from g in this.Context.GeneratingUnits
                                  join ga in this.Context.GeneratorAttributes on g.Id equals ga.GeneratingUnitId
                                  join v in this.Context.VoltageLevels on g.VoltageLevelId equals v.Id
                                  where v.SubstationId == substationId && ga.ProjectPhaseId == phaseId
                                  select g;
            return generatingUnits != null ? generatingUnits.ToList() : null;
        }

        /// <summary>
        /// Returns a List of all GeneratingUnit objects linked to the input SubstationId and whose ProjectPhaseId as found in the input array
        /// </summary>
        /// <param name="substationId"></param>
        /// <param name="phaseIds"></param>
        /// <returns></returns>
        public IEnumerable<GeneratingUnit> GetGeneratingUnits(int substationId, int[] phaseIds)
        {
            return  from g in this.Context.GeneratingUnits
                    join ga in this.Context.GeneratorAttributes on g.Id equals ga.GeneratingUnitId
                    join v in this.Context.VoltageLevels on g.VoltageLevelId equals v.Id
                    where v.SubstationId == substationId && ga.ProjectPhaseId.HasValue && phaseIds.Contains(ga.ProjectPhaseId.Value)
                    select g;
        }

        public ResourceStatuse  GetGeneratingUnitStatus(int statusId)
        {
            return this.Context.ResourceStatuses.FirstOrDefault(rs => rs.Id == statusId);
        }

        public GeneratorType GetGeneratingUnitType(int typeId)
        {
            return this.Context.GeneratorTypes.FirstOrDefault(gt => gt.Id == typeId);
        }

        /// <summary>
        /// returns all generatingUnitType objects
        /// </summary>
        /// <returns>List of GeneratorType objects</returns>
        public IList<GeneratorType> GetGeneratingUnitTypes()
        {
            if (!this.Context.GeneratorTypes.Any()) return null;
            return this.Context.GeneratorTypes.ToList();
        }

        /// <summary>
        /// Returns all of the GeneratingUnits associated with a TSR
        /// </summary>
        /// <param name="tsrId"></param>
        /// <returns></returns>
        public List<GeneratingUnit> GetGeneratingUnitsByTsr(int tsrId)
        {
            var genUnits = (from g in this.Context.GeneratingUnits
                               join l in this.Context.LinkTsrGeneratingUnits on g.Id equals l.GeneratingUnitId
                               join t in this.Context.TransmissionServiceRequests on l.TransmissionServiceRequestId equals t.Id
                               where t.Id == tsrId
                               select g);

            
            return genUnits != null ? genUnits.ToList() : null;
        }


        /// <summary>
        /// Returns a list of all TSRs associated with this GeneratingUnit
        /// </summary>
        /// <param name="generatingUnit"></param>
        /// <returns></returns>
        public List<TransmissionServiceRequest> GetTsrs(int gid)
        {
            var tsrs = (from t in this.Context.TransmissionServiceRequests
                        join l in this.Context.LinkTsrGeneratingUnits on t.Id equals l.TransmissionServiceRequestId
                        join g in this.Context.GeneratingUnits on l.GeneratingUnitId equals g.Id
                        where g.Id == gid
                        select t); //.Distinct();

            return tsrs != null ? tsrs.ToList() : null;
        }



        public IList<VoltageLevel> GetVoltageLevels(GeneratingUnit generatingUnit)
        {
            //if (!this.Context.VoltageLevels.Any()) return null;
            //return this.Context.VoltageLevels.ToList();

            if (generatingUnit.VoltageLevel != null && generatingUnit.VoltageLevel.Substation != null)
            {
                var voltageLevels = from v in this.Context.VoltageLevels 
                                    where v.SubstationId == generatingUnit.VoltageLevel.SubstationId
                                    select v;
                return voltageLevels != null ? voltageLevels.ToList() : null;
            }
            else
                return null;
        }

        /// <summary>
        /// Checks whether the input name is being used to describe an existing GeneratingUnit
        /// </summary>
        /// <param name="name">GeneratingUnit name</param>
        /// <returns>true = name is valid or not is use, false = name is not valid or in use</returns>
        public bool ValidateName(string name, int substationId)
        {
            if (string.IsNullOrEmpty(name)) return false;
            var genUnit = (from g in this.Context.GeneratingUnits
                           join v in this.Context.VoltageLevels on g.VoltageLevelId equals v.Id
                           join n in this.Context.Names on g.Id equals n.GeneratingUnitId
                           where v.SubstationId == substationId && n.NameType.Name.ToLower().Equals("long name") && n.Name1.ToLower().Equals(name)
                           select g).FirstOrDefault();
            return genUnit == null;
        }

        public IList<Note> GetNotes(int generatingUnitId)
        {
            var notes = from ln in this.Context.LinkNotes.Where(link => link.GeneratingUnitId == generatingUnitId)
                        join n in this.Context.Notes on ln.NoteId equals n.Id
                        where n.IsDeleted == false
                        select n;
            return notes != null ? notes.ToList() : null;
        }

        #endregion
    }
}
