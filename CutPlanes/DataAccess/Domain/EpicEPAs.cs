using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public class EpicEPAs : EpicDomainBase
    {
        #region [Properties]

        #endregion

        #region [Constructors]

        public EpicEPAs(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Retrieve EPA Information based on code
        /// </summary>
        /// <param name="code">TLA code</param>
        public ElectricityPurchaseAgreement GetEPA(string code)
        {
            ElectricityPurchaseAgreement myEpa = new ElectricityPurchaseAgreement();
            return this.Context.ElectricityPurchaseAgreements.FirstOrDefault(epa => epa.Code == code);
        }

        /// <summary>
        /// Retrieves an EPA object based on input identity
        /// </summary>
        /// <param name="id">EPA identity</param>
        /// <returns>EPA object</returns>
        public ElectricityPurchaseAgreement GetEPA(int id)
        {
            ElectricityPurchaseAgreement myEpa = new ElectricityPurchaseAgreement();
            return this.Context.ElectricityPurchaseAgreements.FirstOrDefault(epa => epa.Id == id);
        }

        public IList<ElectricityPurchaseAgreement> GetEpas()
        {
            if (!this.Context.ElectricityPurchaseAgreements.Any()) return null;
            var list = this.Context.ElectricityPurchaseAgreements.ToList();
            IEnumerable<ElectricityPurchaseAgreement> sortedList = list.OrderBy(i => i.Code);

            return sortedList.ToList();
        }

        public ContractStatus GetEPAContractStatus(int contractstatusId)
        {
            return this.Context.ContractStatus.FirstOrDefault(cs => cs.ID == contractstatusId);
        }

        public EpicUser GetEPAUser(int userid)
        {
            return this.Context.EpicUsers.FirstOrDefault(eu => eu.Id == userid);
        }

        public PowerCall GetEPAPowerCall(int powercallId)
        {
            return this.Context.PowerCalls.FirstOrDefault(pc => pc.Id == powercallId);
        }

        public IList<Note> GetEPANotes(int epaId)
        {
            var notes = from ln in this.Context.LinkNotes.Where(link => link.ElectricityPurchaseAgreementId == epaId)
                        join n in this.Context.Notes on ln.NoteId equals n.Id
                        select n;
            return notes != null ? notes.ToList() : null;
        }

        public IList<Substation> GetSubstations(int epaId)
        {
            var query = from l in this.Context.LinkElectricityPurchaseAgreementsSubstations.Where(link => link.EPAId == epaId)
                        join s in this.Context.Substations on l.SubstationId equals s.Id
                        select s;

            return query != null ? query.ToList() : null;
        }

        /// <summary>
        /// return the list of EPAs associated with a substation 
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public List<ElectricityPurchaseAgreement> GetEpasFromSubstation(int sid)
        {
            var epa = from e in this.Context.ElectricityPurchaseAgreements
                      join l in this.Context.LinkElectricityPurchaseAgreementsSubstations on e.Id equals l.EPAId
                      where l.SubstationId == sid
                      select e;

            return epa != null ? epa.ToList() : null;

        }

        public List<GeneratingUnit> GetGeneratingUnits(int epaId)
        {
            var genunits = from g in this.Context.GeneratingUnits
                           join v in this.Context.VoltageLevels on g.VoltageLevelId equals v.Id
                           join s in this.Context.Substations on v.SubstationId equals s.Id
                           join l in this.Context.LinkElectricityPurchaseAgreementsSubstations on s.Id equals l.SubstationId
                           join e in this.Context.ElectricityPurchaseAgreements on l.EPAId equals e.Id
                           where e.Id == epaId
                           select g;

            return genunits != null ? genunits.ToList() : null;
        }

        /// <summary>
        /// Inserts a new ElectricityPurchaseAgreement object to Db or updates any existing ElectricityPurchaseAgreement objects (based on ElectricityPurchaseAgreement)
        /// </summary>
        /// <param name="epa">ElectricityPurchaseAgreement object</param>        
        public int SaveEPA(ElectricityPurchaseAgreement epa)
        {

            ElectricityPurchaseAgreement currentEPA = this.Context.ElectricityPurchaseAgreements.FirstOrDefault(e => e.Code == epa.Code);
            //ElectricityPurchaseAgreement cClone = null;
            int insertedRecords = 0;

            if (currentEPA != null)
            {
                //cClone = currentEPA.Clone();
                //Log.Info("Updating EPA record for " + currentEPA.Code );
                currentEPA.DateUpdated = epa.DateCreated;
                currentEPA.UserUpdatedId = epa.UserCreatedId;
                currentEPA.EPACapacity = epa.EPACapacity;
                currentEPA.MaxTakeOrPay = epa.MaxTakeOrPay;
                currentEPA.NitsDesignatedCapacity = epa.NitsDesignatedCapacity;
                currentEPA.ContractManager = epa.ContractManager; //.SafeSubstring(0,100);
                currentEPA.SecondaryContractManager = epa.SecondaryContractManager; //.SafeSubstring(0, 100);
                currentEPA.Description = epa.Description;
                currentEPA.Code = epa.Code; //.SafeSubstring(0, 50);
                currentEPA.EPAStatusId = epa.EPAStatusId;
                currentEPA.PowerCallId = epa.PowerCallId;
                currentEPA.EstimatedInServiceDate = epa.EstimatedInServiceDate;
                currentEPA.ActualInServiceDate = epa.ActualInServiceDate;
                currentEPA.OwnerName = epa.OwnerName; //.SafeSubstring(0, 80);
                epa.Id = currentEPA.Id;
            }
            else
            {
                //Log.Info("Creating new record for "+ currentEPA.Code );
                this.Context.ElectricityPurchaseAgreements.InsertOnSubmit(epa);
                insertedRecords = 1;
            }
            this.Context.SubmitChanges();
            //this.ReportPairs.Items.Add(new Report.ReportPair(cClone, epa));
            return insertedRecords;
        }


        public int GetStatusID(string status)
        {
            string actualstatus = String.Empty;
            if (status.Contains("Post-COD"))
                actualstatus = "Existing";
            else if (status.Contains("Pre-COD"))
                actualstatus = "Committed";
            var epaStatus = this.Context.EPAStatuses.Where(epas => epas.Name.Equals(actualstatus)).FirstOrDefault();
            if (null != epaStatus)
                return epaStatus.ID;
            else
                return 1;  //Return 1 if status doesn't exist.
        }


        /// <summary>
        /// Create a new power call entity if it comes through the EPA
        /// </summary>
        /// <param name="powerCall"></param>
        /// <returns></returns>
        public bool CreatePowerCall(string powerCall)
        {
            if (GetPowerCallID(powerCall) == null)
            {
                var pc = new PowerCall();
                pc.DateCreated = DateTime.Now;
                pc.Value = powerCall;

                this.Context.PowerCalls.InsertOnSubmit(pc);
                this.Context.SubmitChanges();

                return true;
            }
            return false;
        }

        /// <summary>
        /// Return the ID of the powercall identified by the given name.   
        /// </summary>
        /// <param name="powerCall"></param>
        /// <returns>The ID of the powercall, or NULL if record not found</returns>
        public int? GetPowerCallID(string powerCall)
        {
            var powercall = this.Context.PowerCalls.Where(pc => pc.Value.Equals(powerCall)).FirstOrDefault();
            if (null != powercall)
                return powercall.Id;
            else
                return null;  //Return null if powercall doesn't exist.
        }

        public LinkElectricityPurchaseAgreementsSubstation[] GetLinks(int epaId)
        {
            var links = this.Context.LinkElectricityPurchaseAgreementsSubstations.Where(l => l.EPAId == epaId);
            return links != null ? links.ToArray() : null;
        }

        public ResourceStatuse GetResourceStatusRecord(string statusValue)
        {
            return this.Context.ResourceStatuses.FirstOrDefault(s => s.Value == statusValue);
        }


        /// <summary>
        /// Set status of all facilities associated with a specific EPA to terminated.
        /// </summary>
        /// <param name="epaId"></param>
        /// <returns></returns>
        public bool TerminateFacilities(int epaId)
        {
            var lnks = GetLinks(epaId);
            int terminated = GetResourceStatusRecord("T").Id;

            foreach (var l in lnks)
            {
                l.Substation.ResourceStatusId = terminated;
            }

            this.Context.SubmitChanges();

            return true;
        }


        /// <summary>
        ///  Get the link record between a specified EPA and Substation
        /// </summary>
        /// <param name="epaId"></param>
        /// <param name="substationId"></param>
        /// <returns></returns>
        public LinkElectricityPurchaseAgreementsSubstation GetLink(int epaId, int substationId)
        {
            var link = this.Context.LinkElectricityPurchaseAgreementsSubstations.FirstOrDefault(l => l.EPAId == epaId && l.SubstationId == substationId);

            return link;
        }


        /// <summary>
        /// Delete a link record between an EPA and Substation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteEpaLink(int id)
        {
            var link = this.Context.LinkElectricityPurchaseAgreementsSubstations.FirstOrDefault(l => l.Id == id);

            if (link != null)
            {
                this.Context.LinkElectricityPurchaseAgreementsSubstations.DeleteOnSubmit(link);
                this.Context.SubmitChanges();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Create a link between an EPA and Substation
        /// </summary>
        /// <param name="epaId"></param>
        /// <param name="substationId"></param>
        /// <returns></returns>
        public bool CreateEpaLink(int epaId, int substationId)
        {
            var exists = this.Context.LinkElectricityPurchaseAgreementsSubstations.FirstOrDefault(l => l.SubstationId == substationId && l.EPAId == epaId);

            if (exists == null)
            {
                LinkElectricityPurchaseAgreementsSubstation l = new LinkElectricityPurchaseAgreementsSubstation();

                l.SubstationId = substationId;
                l.EPAId = epaId;
                this.Context.LinkElectricityPurchaseAgreementsSubstations.InsertOnSubmit(l);
                this.Context.SubmitChanges();
                return true;
            }

            return false;
        }



        #endregion
    }
}
