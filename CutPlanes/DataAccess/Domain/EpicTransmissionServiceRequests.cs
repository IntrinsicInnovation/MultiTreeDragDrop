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

    public partial class EpicTransmissionServiceRequests : EpicDomainBase
    {
        #region [Properties]
        protected string ConnectionString { get; set; }

        protected EpicDomainDataContext _context = null;
        protected EpicDomainDataContext Context
        {
            get
            {
                if (_context == null) _context = new EpicDomainDataContext(ConnectionString);
                return _context;
            }
        }

        public string ResourceIdentifer
        {
            get { return null; }
        }

        public string DateLastModifiedString
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        public string TypeString
        {
            get { return null; }
        }

        public decimal? Voltage
        {
            get
            {
                return null;
            }
        }


        /// <summary>
        /// Url to Navigate to Meter Navigation Page
        /// </summary>
        public string NavigationUrl
        {
            get
            {
                return null;
            }
        }


        #endregion

        #region [Constructors]

        public EpicTransmissionServiceRequests(string connectionString)
        {
            this.ConnectionString = connectionString;
        }


        #endregion

        #region [Methods]
        public TransmissionServiceRequest GetTsr(string tsrNumber)
        {
            return this.Context.TransmissionServiceRequests.FirstOrDefault(tsr => tsr.TsrNumber == tsrNumber);
        }

        public TransmissionServiceRequest GetTsr(int id)
        {
            return this.Context.TransmissionServiceRequests.FirstOrDefault(tsr => tsr.Id == id);
        }

        // return a sorted list of all TSRs
        public IList<TransmissionServiceRequest> GetTsrs()
        {
            // if no results, return null
            if (!this.Context.TransmissionServiceRequests.Any()) return null;

            var list = this.Context.TransmissionServiceRequests.ToList();
            IEnumerable<TransmissionServiceRequest> sortedList = list.OrderBy(i => i.TsrNumber);

            return sortedList.ToList();
        }

        // save an updated copy of a TSR
        public void Save(TransmissionServiceRequest tsr)
        {
            var currentTsr = this.Context.TransmissionServiceRequests.FirstOrDefault(t => t.Id == tsr.Id);

            // need to explicitly set to null to prevent duplicate key conflict
            if (tsr.FileReferenceId == 0) {
                tsr.FileReferenceId = null;
            }

            if (currentTsr != null)
            {
                //currentTsr.DateCreated = tsr.DateCreated;
                currentTsr.DateUpdated = DateTime.Now;
                currentTsr.UserUpdatedId = tsr.UserCreatedId;
                currentTsr.CapacityMw = tsr.CapacityMw;
                currentTsr.TsrPostingDate = tsr.TsrPostingDate;
                currentTsr.TsrNumber = tsr.TsrNumber;
                currentTsr.FileReferenceId = tsr.FileReferenceId;
            }
            else
            {
                tsr.DateCreated = DateTime.Now;
                tsr.DateUpdated = DateTime.Now;
                tsr.UserUpdatedId = tsr.UserCreatedId;
                this.Context.TransmissionServiceRequests.InsertOnSubmit(tsr);
            }
            this.Context.SubmitChanges();               

        }
        
        // Link a TSR to a substation by adding a record in the join table
        //
        public void LinkTsrToSubstation(int tsrId, int substationId)
        {
            var link = this.Context.LinkTsrSubstations.FirstOrDefault(lts => lts.TransmissionServiceRequestId == tsrId && lts.SubstationId == substationId);

            if (link == null)
            {
                LinkTsrSubstation l = new LinkTsrSubstation();

                l.SubstationId = substationId;
                l.TransmissionServiceRequestId = tsrId;
                l.DateCreated = DateTime.Now;
                l.UserCreatedId = 1;    //TODO get user id
                this.Context.LinkTsrSubstations.InsertOnSubmit(l);
                this.Context.SubmitChanges();
            }
        }

        public void LinkTsrToGeneratingUnit(int tsrId, int genUnitId)
        {
            var link = this.Context.LinkTsrGeneratingUnits.FirstOrDefault(lts => lts.TransmissionServiceRequestId == tsrId && lts.GeneratingUnitId == genUnitId);

            if (link == null)
            {
                LinkTsrGeneratingUnit l = new LinkTsrGeneratingUnit();

                l.GeneratingUnitId = genUnitId;
                l.TransmissionServiceRequestId = tsrId;
                l.DateCreated = DateTime.Now;
                l.UserCreatedId = 1;    //TODO get user id
                this.Context.LinkTsrGeneratingUnits.InsertOnSubmit(l);
                this.Context.SubmitChanges();
            }

        }

        public void LinkTsrToProject(int tsrId, int projectId)
        {
            var link = this.Context.LinkTsrProjects.FirstOrDefault(x => x.TransmissionServiceRequestId == tsrId && x.ProjectId == projectId);

            if (link == null)
            {
                LinkTsrProject l = new LinkTsrProject();

                l.ProjectId = projectId;
                l.TransmissionServiceRequestId = tsrId;
                l.DateCreated = DateTime.Now;
                l.UserCreatedId = 1;    //TODO get user id
                this.Context.LinkTsrProjects.InsertOnSubmit(l);
                this.Context.SubmitChanges();
            }
            
        }


        public void LinkTsrToTvc(int tsrId, int tvcId)
        {
            var link = this.Context.LinkTsrTvcs.FirstOrDefault(x => x.TsrId == tsrId && x.TvcId == tvcId);

            if (link == null)
            {
                LinkTsrTvc l = new LinkTsrTvc();

                l.TvcId = tvcId;
                l.TsrId = tsrId;
                l.DateCreated = DateTime.Now;
                l.UserCreatedId = 1;    //TODO get user id
                this.Context.LinkTsrTvcs.InsertOnSubmit(l);
                this.Context.SubmitChanges();
            }

        }


        public void UnlinkTsr(int tsrId, int resourceId, int annotationType)
        {

           var link = this.Context.LinkTsrSubstations.FirstOrDefault(lts => lts.TransmissionServiceRequestId == tsrId && lts.SubstationId == resourceId);

            if (link != null)
            {
                this.Context.LinkTsrSubstations.DeleteOnSubmit(link);
                this.Context.SubmitChanges();
            }
        }

        public bool DeleteAttachment(int tsrId)
        {
            var tsr = GetTsr(tsrId);
            if (tsr == null)
            {
                return false;
            }

            var fileId = tsr.FileReferenceId;
            tsr.FileReference = null;
            

            var fileRef = this.Context.FileReferences.FirstOrDefault(fr => fr.Id == fileId);

            if (fileRef != null)
            {
                this.Context.FileReferences.DeleteOnSubmit(fileRef);
            }
            else
            {
                return false;
            }

            this.Context.SubmitChanges();

            return true;
        }


        /// <summary>
        /// Delete a TSR record and any associated attachments.
        /// </summary>
        /// <param name="tsrId">The ID of the TSR to delete</param>
        /// <returns>true on success</returns>
        public bool DeleteTsr(int tsrId)
        {
            if (tsrId < 1)
            {
                return false;
            }

            // delete any attachments if they exist
            DeleteAttachment(tsrId);

            // delete the record
            var tsr = GetTsr(tsrId);

            this.Context.TransmissionServiceRequests.DeleteOnSubmit(tsr);
            this.Context.SubmitChanges();

            return true;
        }


        public IList<Note> GetNotes(int id)
        {
            var notes = from ln in this.Context.LinkNotes.Where(link => link.TsrId == id)
                        join n in this.Context.Notes on ln.NoteId equals n.Id
                        where n.IsDeleted == false
                        select n;
            return notes != null ? notes.ToList() : null;
        }


        #endregion

    }
}
