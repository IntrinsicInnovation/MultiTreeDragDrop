using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public class EpicTransmissionVoltageCustomers : EpicDomainBase
    {
        #region [Properties]

        #endregion

        #region [Constructors]

        public EpicTransmissionVoltageCustomers(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        
        /// <summary>
        /// Return a list of all TVC facility types
        /// </summary>
        /// <returns></returns>
        public List<TvcFacilityType> GetTvcFacilityTypes()
        {
            var res = this.Context.TvcFacilityTypes.ToList();
            
            return res.ToList();
        }


        /// <summary>
        /// Return a list of all TVC facility status codes
        /// </summary>
        /// <returns></returns>
        public List<TvcFacilityStatuse> GetTvcFacilityStatuses()
        {
            var res = this.Context.TvcFacilityStatuses.ToList();
            return res.ToList();
        }


        /// <summary>
        /// Return a list of all TVC facility "stages" (state codes)
        /// </summary>
        /// <returns></returns>
        public List<TvcFacilityStage> GetTvcFacilityStages()
        {
            var res = this.Context.TvcFacilityStages.ToList();
            return res.ToList();
        }


        /// <summary>
        /// Save a TVC record
        /// </summary>
        /// <param name="tvc"></param>
        /// <returns></returns>
        public int SaveTvc(TransmissionVoltageCustomer tvc)
        {
            int id = tvc.Id;
            
            TransmissionVoltageCustomer t = (from x in this.Context.TransmissionVoltageCustomers.Where(y => y.Id.Equals(tvc.Id))
                                             select x).FirstOrDefault();

            if (t != null)  // we are updating a record
            {
                t.AccountNumber = tvc.AccountNumber;
                t.CustomerId = tvc.CustomerId;
                t.DateUpdated = DateTime.Now;
                t.DateNominated = tvc.DateNominated;
                t.Description = tvc.Description;
                t.KiloVolt = tvc.KiloVolt;
                t.Latitude = tvc.Latitude;
                t.Longitude = tvc.Longitude;
                t.LineNumber = tvc.LineNumber;
                t.LoadFactor = tvc.LoadFactor;
                t.PowerFactor = tvc.PowerFactor;
                t.ProbOfConnection = tvc.ProbOfConnection;
                t.Rate = tvc.Rate;
                t.StageId = tvc.StageId;
                t.StatusId = tvc.StatusId;
                t.TypeId = tvc.TypeId;
                t.UserUpdatedId = tvc.UserCreatedId;
                t.EpaCode = tvc.EpaCode;
                t.IsIpp = tvc.IsIpp;
                t.IsOnSiteGeneration = tvc.IsOnSiteGeneration;
                t.ProjectName = tvc.ProjectName;
                t.FacilityCode = tvc.FacilityCode;
                t.FacilityId = tvc.FacilityId;

                this.Context.SubmitChanges();
                return t.Id;
            }
 
            tvc.DateCreated = DateTime.Now;         
            this.Context.TransmissionVoltageCustomers.InsertOnSubmit(tvc);
            this.Context.SubmitChanges();
            return tvc.Id;
        }


        public TransmissionVoltageCustomer GetTvc(int id)
        {
            var tvc = this.Context.TransmissionVoltageCustomers.FirstOrDefault(x => x.Id == id);

            return tvc;            
        }

        public TransmissionVoltageCustomer GetTvc(string Code)
        {
            var tvc = this.Context.TransmissionVoltageCustomers.FirstOrDefault(x => x.FacilityCode == Code);

            return tvc;
        }


        public List<TransmissionVoltageCustomer> GetTvcs()
        {
            var tvcs = this.Context.TransmissionVoltageCustomers.Where(x => x.ProjectName != null);
            return tvcs.ToList();
        }



        public TvcStagedLoadMeasure SaveTvcStagedLoadMeasure(TvcStagedLoadMeasure load)
        {
            int id = load.Id;

            var l = this.Context.TvcStagedLoadMeasures.FirstOrDefault(x => x.Id == load.Id);

            if (l != null)
            {   // we are updating
                l.ActivationDate = load.ActivationDate;
                l.StagedLoadValue = load.StagedLoadValue;
                l.TvcId = load.TvcId;
            }
            else
            {
                load.DateCreated = DateTime.Now;
                this.Context.TvcStagedLoadMeasures.InsertOnSubmit(load);
            }

            this.Context.SubmitChanges();

            return l ?? load;

        }


        public int DeleteTvcStagedLoadMeasure(TvcStagedLoadMeasure load)
        {
            int id = load.Id;

            if (load != null)
            {
                this.Context.TvcStagedLoadMeasures.DeleteOnSubmit(load);
                this.Context.SubmitChanges();
            }

            return id;
        }
        #endregion


        public int DeleteTvcStagedLoadMeasure(int id)
        {
            var load = this.Context.TvcStagedLoadMeasures.FirstOrDefault(x => x.Id == id);

            if (load != null)
            {
                return DeleteTvcStagedLoadMeasure(load);
            }
            return 0;

        }

        public List<TvcStagedLoadMeasure> GetTvcStagedLoadMeasures(int tvcId)
        {
            if (tvcId < 1) {
                return null;
            }

            var loads = this.Context.TvcStagedLoadMeasures.Where(x => x.TvcId == tvcId).OrderBy(y => y.ActivationDate);

            if (loads.Count() > 0)
            {
                return loads.ToList();
            }
            else
            {
                return null;
            }
        }

        public TvcStagedLoadMeasure GetTvcStagedLoadMeasure(int id)
        {
            if (id < 1)
            {
                return null;
            }

            var loadMeasure = this.Context.TvcStagedLoadMeasures.FirstOrDefault(x => x.Id == id);

            return loadMeasure;
        }

        public IList<Note> GetNotes(int id)
        {
            var notes = from ln in this.Context.LinkNotes.Where(link => link.TransmissionVoltageCustomerId == id)
                        join n in this.Context.Notes on ln.NoteId equals n.Id
                        where n.IsDeleted == false
                        select n;
            return notes != null ? notes.ToList() : null;
        }

        /// <summary>
        /// Gets a list of all TVC objects associated with a TSR
        /// </summary>
        /// <param name="resourcePlanId">sid/param>
        /// <returns>List of transmission service requests for a particular TVC</returns>
        public List<TransmissionServiceRequest> GetTsrs(int id)
        {
            var tsrs = (from t in this.Context.TransmissionServiceRequests
                        join l in this.Context.LinkTsrTvcs on t.Id equals l.TsrId
                        join s in this.Context.TransmissionVoltageCustomers on l.TvcId equals s.Id
                        where s.Id == id
                        select t); //.Distinct();

            return tsrs != null ? tsrs.ToList() : null;
        }


        /// <summary>
        /// Returns a list of projects associated with a specific TSR ID
        /// </summary>
        /// <param name="tsrId"></param>
        /// <returns></returns>
        public IList<TransmissionVoltageCustomer> GetTvcsByTsr(int tsrId)
        {
            var tvcs = (from p in this.Context.TransmissionVoltageCustomers
                            join l in this.Context.LinkTsrTvcs on p.Id equals l.TvcId
                            join t in this.Context.TransmissionServiceRequests on l.TsrId equals t.Id
                            where t.Id == tsrId
                            select p);
            return tvcs != null ? tvcs.ToList() : null;
        }


        public List<TransVoltageCustLoadForecast> GetTvcLoadForecasts()
        {
            var forecasts = this.Context.TransVoltageCustLoadForecasts.ToList();
            return forecasts;
        }

        public List<TvcForecast> GetTvcForecasts()
        {
            var res = this.Context.TvcForecasts.ToList();
            return res;
        }



    }
}
