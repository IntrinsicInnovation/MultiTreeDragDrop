//==========================================================================================================//
//                                                                                                          //
// Energy Planning Information Central (EPIC) System                                                        //
//                                                                                                          //  
// Copyright (c) 2011-2012 BC Hydro and Power Authority                                                     //  
//                                                                                                          //
// This file is the property of the BC Hydro and may not be distributed, copied or modified without         //
// the express written permission of the BC Hydro and Power Authority.                                      //
//                                                                                                          //
//==========================================================================================================//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public class EpicSubstations : EpicDomainBase
    {

        #region [Properties]

        #endregion

        #region [Constructors]

        public EpicSubstations(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Retrive Substation Information based on TLA code
        /// </summary>
        /// <param name="code">TLA code</param>
        public Substation GetSubstation(string code)
        {
            var substation = from n in this.Context.Names.Where(na => na.Name1.Equals(code))
                             join s in this.Context.Substations on n.SubstationId equals s.Id
                             select s;

            return substation.FirstOrDefault();
        }

        /// <summary>
        /// Gets a list of all Substation objects in acsending order (by TLA) 
        /// </summary>
        /// <returns>List of Substation objects</returns>
        public List<Substation> GetSubstations()
        {
            var substations = from s in this.Context.Substations
                              join n in this.Context.Names on s.Id equals n.SubstationId
                              orderby n.Name1
                              where n.NameType.Name.Equals("TLA")
                              select s;
            return substations != null ? substations.ToList()  : null;
        }

        /// <summary>
        /// Gets a list of all Substation objects linked to the base resource plan in acsending order (by TLA) 
        /// </summary>
        /// <returns>List of Substation objects</returns>
        public IEnumerable<Substation> GetResourcePlanAssociatedSubstations()
        {
            return (from s in this.Context.Substations
                    join sa in this.Context.SubstationAttributes on s.Id equals sa.SubstationId
                    join n in this.Context.Names on s.Id equals n.SubstationId
                    orderby n.Name1
                    where n.NameType.Name.Equals("TLA")
                    select s).Distinct();
        }

        /// <summary>
        /// Gets a list of all Substation objects in acsending order (by TLA) 
        /// </summary>
        /// <param name="resourcePlanId">resource Plan Id</param>
        /// <returns>List of Substation objects</returns>
        public List<Substation> GetSubstations(int resourcePlanId)
        {
            var substations = (from s in this.Context.Substations
                              join n in this.Context.Names on s.Id equals n.SubstationId
                              join sa in this.Context.SubstationAttributes on s.Id equals sa.SubstationId
                              orderby n.Name1
                              where n.NameType.Name.Equals("TLA") && sa.ResourcePlanId == resourcePlanId &&
                              !sa.ProjectPhaseId.HasValue
                              select s).Distinct();
            return substations != null ? substations.ToList() : null;
        }

        /// <summary>
        /// Gets a list of all Substation objects associated with a TSR
        /// </summary>
        /// <param name="resourcePlanId">TSR Id</param>
        /// <returns>List of Substation objects</returns>
        public List<Substation> GetSubstationsByTsr(int tsrId)
        {
            var substations = (from s in this.Context.Substations
                               join l in this.Context.LinkTsrSubstations on s.Id equals l.SubstationId
                               join t in this.Context.TransmissionServiceRequests on l.TransmissionServiceRequestId equals t.Id
                               where t.Id == tsrId
                               select s);
            return substations != null ? substations.ToList() : null;
        }


        /// <summary>
        /// Gets a list of all Substation objects associated with a TSR
        /// </summary>
        /// <param name="resourcePlanId">sid/param>
        /// <returns>List of transmission service requests for a particular substation</returns>
        public List<TransmissionServiceRequest> GetTsrs(int sid)
        {
            var tsrs = (from t in this.Context.TransmissionServiceRequests
                        join l in this.Context.LinkTsrSubstations on t.Id equals l.TransmissionServiceRequestId
                        join s in this.Context.Substations on l.SubstationId equals s.Id
                        where s.Id == sid
                        select t); //.Distinct();

            return tsrs != null ? tsrs.ToList() : null;
        }

        /// <summary>
        /// Returns a List of all GeneratingUnit objects linked to the input SubstationId
        /// </summary>
        /// <param name="substationId">Substation DB Id</param>
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
        /// Inserts a new Substation object to Db or updates any existing Substation objects (based on Substation TLA)
        /// </summary>
        /// <param name="substation">Substation object</param>        
        public void SaveSubstation(Substation substation)
        {
            Substation currentSubstation = (from n in this.Context.Names.Where(na => na.Name1.Equals(substation.TLA))
                                            join s in this.Context.Substations on n.SubstationId equals s.Id
                                            select s).FirstOrDefault();

            if (currentSubstation != null)
            {
                currentSubstation.DateUpdated = DateTime.Now;
                currentSubstation.UserUpdatedId = substation.UserCreatedId;
                currentSubstation.GeoLocationId = substation.GeoLocationId;
                currentSubstation.Description = substation.Description;                
                currentSubstation.IsSynchronousCondenserCapable = substation.IsSynchronousCondenserCapable;
                currentSubstation.Latitude = substation.Latitude;
                currentSubstation.Longitude = substation.Longitude;
                currentSubstation.Description = substation.Description;
                currentSubstation.LocalOperationOrderLink = substation.LocalOperationOrderLink;
                currentSubstation.MeterId = substation.MeterId;
                currentSubstation.ElectricalGroupId = substation.ElectricalGroupId;
                currentSubstation.OwnershipTypeId = substation.OwnershipTypeId;
                currentSubstation.PowerCallId = substation.PowerCallId;
                currentSubstation.ResourceStatusId = substation.ResourceStatusId;
                currentSubstation.SingleLineDiagramUri = substation.SingleLineDiagramUri;
                currentSubstation.SubStationTypeId = substation.SubStationTypeId;                
                currentSubstation.FuelTypeId = substation.FuelTypeId;
                currentSubstation.CIMLoadTS = substation.CIMLoadTS;
                currentSubstation.LRBLoadTS = substation.LRBLoadTS;
                currentSubstation.FacilityLoadTS = substation.FacilityLoadTS;
                currentSubstation.NITSPlanLoadTS = substation.NITSPlanLoadTS;
                currentSubstation.LTAPSubRegionId = substation.LTAPSubRegionId;
                currentSubstation.PrimaryPoi = substation.PrimaryPoi;
                currentSubstation.SecondaryPoi = substation.SecondaryPoi;
                currentSubstation.PoiCircuitDesignation = substation.PoiCircuitDesignation;
                currentSubstation.PoiVoltage = substation.PoiVoltage;
                substation.Id = currentSubstation.Id;
            }
            else                           
                this.Context.Substations.InsertOnSubmit(substation);
            this.Context.SubmitChanges();
        }


        /// <summary>
        /// Inserts a new Substation object to Db or updates any existing Substation objects (based on Substation ID)
        /// </summary>
        /// <param name="substation">Substation object</param>        
        public void SaveSubstationbyID(Substation substation)
        {
            Substation currentSubstation = (from s in this.Context.Substations.Where(sub => sub.Id.Equals(substation.Id))
                                            select s).FirstOrDefault();

            if (currentSubstation != null)
            {
                currentSubstation.DateUpdated = DateTime.Now;
                currentSubstation.UserUpdatedId = substation.UserCreatedId;
                currentSubstation.GeoLocationId = substation.GeoLocationId;
                //currentSubstation.Description = substation.Description;
                currentSubstation.IsSynchronousCondenserCapable = substation.IsSynchronousCondenserCapable;
                currentSubstation.Latitude = substation.Latitude;
                currentSubstation.Longitude = substation.Longitude;                
                currentSubstation.Description = substation.Description;
                currentSubstation.LocalOperationOrderLink = substation.LocalOperationOrderLink;
                currentSubstation.MeterId = substation.MeterId;
                currentSubstation.ElectricalGroupId = substation.ElectricalGroupId;
                currentSubstation.OwnershipTypeId = substation.OwnershipTypeId;
                currentSubstation.PowerCallId = substation.PowerCallId;
                currentSubstation.ResourceStatusId = substation.ResourceStatusId;
                currentSubstation.SingleLineDiagramUri = substation.SingleLineDiagramUri;
                currentSubstation.SubStationTypeId = substation.SubStationTypeId;
                currentSubstation.FuelTypeId = substation.FuelTypeId;
                currentSubstation.PrimaryPoi = substation.PrimaryPoi;
                currentSubstation.SecondaryPoi = substation.SecondaryPoi;
                currentSubstation.PoiCircuitDesignation = substation.PoiCircuitDesignation;
                currentSubstation.PoiVoltage = substation.PoiVoltage;
                substation.Id = currentSubstation.Id;
            }
            else
                this.Context.Substations.InsertOnSubmit(substation);
            var cs = this.Context.GetChangeSet();
            this.Context.SubmitChanges();
        }


        /// <summary>
        /// Maps properties of newSubstationAttribute object to currentSubstationAttribute object.  Changes are not commited to DB.
        /// </summary>
        /// <param name="newSubstationAttribute">SubstationAttribute object</param>
        /// <param name="currentSubstationAttribute">SubstationAttribute object</param>
        private void UpdateAttribute(SubstationAttribute newSa, SubstationAttribute currentSa)
        {
            currentSa.DateUpdated = DateTime.Now;
            currentSa.UserUpdatedId = newSa.UserCreatedId;
            currentSa.DateEffective = newSa.DateEffective;
            currentSa.DateEnd = newSa.DateEnd;
            currentSa.DateReplaced = newSa.DateReplaced;
            currentSa.DependableGeneratingCapacity = newSa.DependableGeneratingCapacity;
            currentSa.EffectiveLoadCarryingCapacity = newSa.EffectiveLoadCarryingCapacity;
            currentSa.EPACapacity = newSa.EPACapacity;
            currentSa.InServiceEndDate = newSa.InServiceEndDate;
            currentSa.InServiceStartDate = newSa.InServiceStartDate;
            currentSa.LoadGrossSummerPeak = newSa.LoadGrossSummerPeak;
            currentSa.LoadGrossWinterPeak = newSa.LoadGrossWinterPeak;
            currentSa.LoadGrossSummerMin = newSa.LoadGrossSummerMin;
            currentSa.LoadGrossWinterMin = newSa.LoadGrossWinterMin;
            //currentSa.MaximumContinousRating = newSa.MaximumContinousRating;
            //currentSa.MaximumPowerOutput = newSa.MaximumPowerOutput;
            //currentSa.MaxReactiveAbsorptionInMVars = newSa.MaxReactiveAbsorptionInMVars;
            //currentSa.MaxReactiveProductionInMVars = newSa.MaxReactiveProductionInMVars;
            currentSa.MaxTakeOrPayCapacity = newSa.MaxTakeOrPayCapacity;
            currentSa.NamePlateCapacityInMW = newSa.NamePlateCapacityInMW;
            currentSa.NitsDesignatedCapacity = newSa.NitsDesignatedCapacity;
            currentSa.Nominated = newSa.Nominated;
            currentSa.OriginalId = newSa.OriginalId;
            currentSa.PointToPointCapacity = newSa.PointToPointCapacity;
            currentSa.Probability = newSa.Probability;
            currentSa.ProjectPhaseId = newSa.ProjectPhaseId;
            //currentSa.RatedAmp = newSa.RatedAmp;
            currentSa.MaxOutputForFirmTransmission = newSa.MaxOutputForFirmTransmission;
            currentSa.MaxOutputForNonFirmTransmission = newSa.MaxOutputForNonFirmTransmission;
            currentSa.RatedMVA = newSa.RatedMVA;
            //currentSa.RatedMW = newSa.RatedMW;
            currentSa.RatedOverExcitedMvar = newSa.RatedOverExcitedMvar;
            currentSa.RatedOverExcitedPowerFactor = newSa.RatedOverExcitedPowerFactor;
            currentSa.RatedPowerFactor = newSa.RatedPowerFactor;
            //currentSa.RatedSpeed = newSa.RatedSpeed;
            currentSa.RatedUnderExcitedMvar = newSa.RatedUnderExcitedMvar;
            currentSa.RatedUnderExcitedPowerFactor = newSa.RatedUnderExcitedPowerFactor;
            currentSa.ReliabilityMustRunCapacity = newSa.ReliabilityMustRunCapacity;
            currentSa.MaximumPowerOutput = newSa.MaximumPowerOutput;
            currentSa.MaxOutputForNonFirmTransmission = newSa.MaxOutputForNonFirmTransmission;
            currentSa.MaxOutputForFirmTransmission = newSa.MaxOutputForFirmTransmission;
            // radio buttons
            currentSa.UsePndcMasteredValue = newSa.UsePndcMasteredValue;
            currentSa.UsePdgcMasteredValue = newSa.UsePdgcMasteredValue;
            currentSa.UsePmpoMasteredValue = newSa.UsePmpoMasteredValue;
            currentSa.UsePptpMasteredValue = newSa.UsePptpMasteredValue;
            currentSa.UsePrmrMasteredValue = newSa.UsePrmrMasteredValue;
            currentSa.UsePdmx0MasteredValue = newSa.UsePdmx0MasteredValue;
            currentSa.UsePdmx1MasteredValue = newSa.UsePdmx1MasteredValue;
            currentSa.UsePnpMasteredValue = newSa.UsePnpMasteredValue;
            currentSa.UsePelccMasteredValue = newSa.UsePelccMasteredValue;
            currentSa.InterconnectionsRequestCapacity = newSa.InterconnectionsRequestCapacity;
            newSa.Id = currentSa.Id;
        }



        /// <summary>
        /// Inserts a new SubstationAttribute object to Db or updates any existing SubstationAttribute objects (based on SubstationId and ResourcePlanId)
        /// </summary>       
        /// <param name="substationAttribute">SubstationAttribute object</param>       
        public void SaveAttribute(SubstationAttribute sa)
        {
            SubstationAttribute currentSa = this.Context.SubstationAttributes.FirstOrDefault(ssa => ssa.SubstationId == sa.SubstationId && ssa.ResourcePlanId == sa.ResourcePlanId && !ssa.ProjectPhaseId.HasValue);
            if (currentSa != null)
                UpdateAttribute(sa, currentSa);
            else
                this.Context.SubstationAttributes.InsertOnSubmit(sa);
            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Inserts a new SubstationAttribute object to Db or updates any existing SubstationAttribute objects (based on substation Id, plan id & phase id)
        /// </summary>       
        /// <param name="substationAttribute">SubstationAttribute object</param>       
        public void Save(SubstationAttribute sa)
        {
            SubstationAttribute currentSa = this.Context.SubstationAttributes.FirstOrDefault(ssa => ssa.SubstationId == sa.SubstationId && ssa.ResourcePlanId == sa.ResourcePlanId && (!ssa.ProjectPhaseId.HasValue || ssa.ProjectPhaseId == sa.ProjectPhaseId));
            if (currentSa != null)
                UpdateAttribute(sa, currentSa);
            else
                this.Context.SubstationAttributes.InsertOnSubmit(sa);
            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Inserts a new SubstationAttribute object to Db or updates any existing SubstationAttribute objects (based on SubstationId, ResourcePlanId and ProjectPhaseId)
        /// </summary>       
        /// <param name="substationAttribute">SubstationAttribute object</param>       
        public bool SaveAttributeAgainstProjectPhase(SubstationAttribute sa)
        {
            bool newToPhase = false;
            SubstationAttribute currentSa = this.Context.SubstationAttributes.FirstOrDefault(ssa => ssa.SubstationId == sa.SubstationId &&
                                                                                                                     ssa.ResourcePlanId == sa.ResourcePlanId &&
                                                                                                                     ssa.ProjectPhaseId == sa.ProjectPhaseId);

            if (currentSa != null)
                UpdateAttribute(sa, currentSa);
            else
            {
                this.Context.SubstationAttributes.InsertOnSubmit(sa);
                newToPhase = true;
            }
            this.Context.SubmitChanges();
            return newToPhase;
        }

        public bool SaveAttributeAgainstProjectPhase(SubstationAttribute newSa, ref SubstationAttribute currentSa)
        {
            SubstationAttribute existingSa = this.Context.SubstationAttributes.FirstOrDefault(ssa => ssa.SubstationId == newSa.SubstationId &&
                                                                                                                    ssa.ResourcePlanId == newSa.ResourcePlanId &&
                                                                                                                    ssa.ProjectPhaseId == newSa.ProjectPhaseId);
            bool newToPhase = false;
            if (existingSa != null)
            {
                currentSa = existingSa.Clone();
                UpdateAttribute(newSa, existingSa);
            }
            else
            {
                this.Context.SubstationAttributes.InsertOnSubmit(newSa);
                newToPhase = true;
            }
            this.Context.SubmitChanges();
            return newToPhase;
        }

        /// <summary>
        /// Checks whether the input tla is being used to describe an existing substation
        /// </summary>
        /// <param name="tla">Substation Tla</param>
        /// <returns>true = tla is valid or not is use, false = not valid or in use</returns>
        public bool ValidateTla(string tla)
        {
            if (string.IsNullOrEmpty(tla)) return false;
            return !this.Context.Names.Any(n => n.Name1.ToLower().Equals(tla.ToLower()) && n.NameType.Name.ToLower().Equals("tla") && n.SubstationId.HasValue);
        }


        /// <summary>
        /// Saves a Name object to the Db or updates the current name object if one exists (based on Name property)
        /// </summary>
        /// <param name="name">Name object</param>
        /// <returns>db identity</returns>
        public int SaveName(Name name)
        {
            Name currentName = this.Context.Names.FirstOrDefault(n => n.Name1 == name.Name1 && n.NameTypeId == name.NameTypeId && n.SubstationId == name.SubstationId);
            bool inserted = false;

            if (currentName != null)            
                return currentName.Id; // already exists            
            else
            {
                this.Context.Names.InsertOnSubmit(name);
                inserted = true;
            }

            this.Context.SubmitChanges();

            //must deactivate all other name records that are the same type and substationID to allow name changes.
            if (inserted)
            {
                var query = from n in this.Context.Names
                            where n.NameTypeId == name.NameTypeId
                                && n.Name1 != name.Name1
                                && n.SubstationId == name.SubstationId
                                && n.IsActive == true
                            select n;
                foreach (var current in query)
                {
                    current.IsActive = false;
                }
                Context.SubmitChanges();
            }
            return name.Id;
        }


        /// <summary>
        /// Saves the input VoltageLevel object to DB
        /// </summary>
        /// <param name="voltageLevel">VoltageLevel object</param>
        public void SaveVoltageLevel(VoltageLevel voltageLevel)
        {
            if (this.Context.VoltageLevels.Any(vl => vl.NominalVoltage == voltageLevel.NominalVoltage && vl.SubstationId == voltageLevel.SubstationId)) return;
                
            this.Context.VoltageLevels.InsertOnSubmit(voltageLevel);
            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Gets the list of voltage levels for a substation
        /// 
        /// MTM: A similar function must be coded somewhere else in the system, but I couldn't find it...
        /// TODO: Review & possibly remove
        /// </summary>
        /// <param name="voltageLevel">VoltageLevel object</param>
        public IList<VoltageLevel> GetVoltageLevels(int SubstationId)
        {
            var voltageLevels = from v in this.Context.VoltageLevels
                        where v.SubstationId == SubstationId
                        select v;

            return voltageLevels != null ? voltageLevels.ToList() : null;
        }



        /// <summary>
        /// Returns the Substation Attributes object associated to the input substation Id & resourcePlanId
        /// </summary>
        /// <param name="substionId">substation Id</param>
        /// <param name="resourcePlanId">resourcePlan Id</param>
        /// <returns>SubstationAttribute object</returns>
        public SubstationAttribute GetSubstationAttribute(int substionId, int resourcePlanId)
        {
            return this.Context.SubstationAttributes.FirstOrDefault(ssa => ssa.SubstationId == substionId && ssa.ResourcePlanId == resourcePlanId && !ssa.ProjectPhaseId.HasValue);            
        }

        public IEnumerable<Substation> GetSubstations(int? projectPhaseId, int resourcePlanId)
        {
            return from s in this.Context.Substations
                   join sa in this.Context.SubstationAttributes on s.Id equals sa.SubstationId
                   where sa.ProjectPhaseId == projectPhaseId && sa.ResourcePlanId == resourcePlanId
                   select s;
        }

        public IEnumerable<Substation> GetSubstations(int[] phaseIds)
        {
            return from s in this.Context.Substations
                   join sa in this.Context.SubstationAttributes on s.Id equals sa.SubstationId
                   where sa.ProjectPhaseId.HasValue && phaseIds.Contains(sa.ProjectPhaseId.Value)
                   select s;
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
                              //join sa in this.Context.SubstationAttributes on s.Id equals sa.SubstationId
                              where s.ElectricalGroupId == electricalGroupId 
                              select s;
            return substations != null ? substations.ToList() : null;
        }

        /// <summary>
        /// Returns the Substation Attributes object associated to the input substation Id & resourcePlanId
        /// </summary>
        /// <param name="substionId">substation Id</param>
        /// <param name="resourcePlanId">resourcePlan Id</param>
        /// <param name="phaseId">project phase Id</param>
        /// <returns>SubstationAttribute object</returns>
        public SubstationAttribute GetAttribute(int substionId, int resourcePlanId, int? phaseId)
        {
            if (phaseId.HasValue)
            {
                return this.Context.SubstationAttributes.FirstOrDefault(ssa => ssa.SubstationId == substionId &&
                                                                    ssa.ResourcePlanId == resourcePlanId &&
                                                                    ssa.ProjectPhaseId == phaseId);
            }
            else
            {
                return this.Context.SubstationAttributes.FirstOrDefault(ssa => ssa.SubstationId == substionId &&
                                                                    ssa.ResourcePlanId == resourcePlanId &&
                                                                    ssa.ProjectPhaseId == null);
            }  
        }

        public IList<SubstationAttribute> GetAttributeList(int resourcePlanId, int? phaseId)
        {
            var attributes = this.Context.SubstationAttributes.Where(ssa => ssa.ResourcePlanId == resourcePlanId && ssa.ProjectPhaseId == phaseId);
            return attributes != null ? attributes.ToList() : null;
        }

        public IList<SubstationAttribute> GetAttributeList(int? phaseId)
        {
            var attributes = this.Context.SubstationAttributes.Where(ssa => ssa.ProjectPhaseId == phaseId);
            return attributes != null ? attributes.ToList() : null;
        }

        public void DeleteAttribute(int substationId, int resourcePlanId, int? phaseId)
        {
            SubstationAttribute substationAttribute = GetAttribute(substationId, resourcePlanId, phaseId);
            if (substationAttribute == null) return;
            this.Context.SubstationAttributes.DeleteOnSubmit(substationAttribute);
            this.Context.SubmitChanges();            
        }

        public void DeleteAttributes(int phaseId)
        {
            var substationAttributes = this.Context.SubstationAttributes.Where(sa => sa.ProjectPhaseId == phaseId);
            if (substationAttributes == null) return;
            this.Context.SubstationAttributes.DeleteAllOnSubmit(substationAttributes);
            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Retrieves a substation object based on input identity
        /// </summary>
        /// <param name="id">substation identity</param>
        /// <returns>substation object</returns>
        public Substation GetSubstation(int id)
        {
            return this.Context.Substations.FirstOrDefault(s => s.Id == id);
        }

        public SubstationType GetSubstationType(int typeId)
        {
            return this.Context.SubstationTypes.FirstOrDefault(st => st.Id == typeId);
        }

        /// <summary>
        /// Gets a list of all SubstationType Objects
        /// </summary>
        /// <returns></returns>
        public IList<SubstationType> GetSubstationTypes()
        {
            if (!this.Context.SubstationTypes.Any()) return null;
            return this.Context.SubstationTypes.ToList();
        }

   
        public OwnershipType GetOwnershipType(int typeId)
        {
            return this.Context.OwnershipTypes.FirstOrDefault(ot => ot.Id == typeId);
        }

        public ResourceStatuse GetResourceStatus(int statusId)
        {
            return this.Context.ResourceStatuses.FirstOrDefault(rs => rs.Id == statusId);
        }

        /// <summary>
        /// Gets a list of all phase ids linked to the input substation id
        /// </summary>
        /// <param name="id">substation identity</param>
        /// <returns></returns>
        public int[] GetPhaseIdList(int id, int resourcePlanId)
        {
            var Ids = this.Context.SubstationAttributes.Where(ssa => ssa.SubstationId == id && ssa.ProjectPhaseId.HasValue && ssa.ResourcePlanId == resourcePlanId).Select(ssa => ssa.ProjectPhaseId.Value);
            return Ids != null ? Ids.ToArray() : null;
        }
        
                
        public IList<Note> GetNotes(int substationId)
        {
            var notes = from ln in this.Context.LinkNotes.Where(link => link.SubstationId == substationId)
                        join n in this.Context.Notes on ln.NoteId equals n.Id
                        where n.IsDeleted == false
                        select n;
            return notes != null ? notes.ToList() : null;
        }

        public IList<ElectricityPurchaseAgreement> GetEpas(int substationId) {
            var query = from l in this.Context.LinkElectricityPurchaseAgreementsSubstations.Where(link => link.SubstationId == substationId)
                        join e in this.Context.ElectricityPurchaseAgreements on l.EPAId equals e.Id
                        select e;

            return query != null ? query.ToList() : null;
        }

        public ElectricityPurchaseAgreement GetDefaultEpa(int substationId)
        {
            var query = from l in this.Context.LinkElectricityPurchaseAgreementsSubstations.Where(link => link.SubstationId == substationId)
                        join e in this.Context.ElectricityPurchaseAgreements on l.EPAId equals e.Id
                        select e;

            // TODO MTM not really checking here what is the real default EPA... just returning the first result.
            // this will work for now...
            return query != null ? query.FirstOrDefault() : null;
        }


        public DateTime? GetLastWriteTime()
        {
            if (!this.Context.Substations.Any()) return null;
            DateTime? updated = this.Context.Substations.Max(s => s.DateUpdated);
            DateTime created = this.Context.Substations.Max(s => s.DateCreated);
            return updated.HasValue && updated > created ? updated : created;
            //return this.Context.Substations.Max(s => s.DateCreated);
        }

        public DateTime GetLastAttributeWriteTime()
        {
            DateTime lastWrite = new DateTime();
            if (this.Context.SubstationAttributes.Any())             
                lastWrite = this.Context.SubstationAttributes.Max(sa => sa.DateCreated);
            
            return lastWrite;
        }

        /// <summary>
        /// Return a list of resource plan objects linked to the input substation id
        /// </summary>
        /// <param name="substationid">substation identity</param>
        /// <returns>list of resource plan objects</returns>
        public IList<ResourcePlan> GetResourcePlans(int substationid)
        {
            var plans = (from rp in this.Context.ResourcePlans
                         join sa in this.Context.SubstationAttributes on rp.Id equals sa.ResourcePlanId
                         where sa.SubstationId == substationid
                         select rp).Distinct();
            return plans != null ? plans.ToList() : null;
        }

        #endregion
       
    }
}
