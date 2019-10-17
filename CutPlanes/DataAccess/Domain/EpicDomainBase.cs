using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace EpicDataAccess.Domain
{
    public abstract class EpicDomainBase : IDisposable
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

        private ILog _log = null;
        protected ILog Log
        {
            get
            {
                if (_log == null)
                {
                    _log = LogManager.GetLogger(typeof(EpicDomainBase));
                    log4net.Config.XmlConfigurator.Configure();
                }
                return _log;
            }
        }

        #endregion



        #region [Methods]        

        public NotesType GetNoteType(int noteTypeId)
        {
            return this.Context.NotesTypes.FirstOrDefault(nt => nt.Id == noteTypeId);
        }

        public IList<NotesType> GetNoteTypes()
        {
            var noteTypes = this.Context.NotesTypes;
            return noteTypes != null ? noteTypes.ToList() : null;
        }

        public int SaveNote(Note note, LinkNote linkNote)
        {
            this.Context.Notes.InsertOnSubmit(note);
            this.Context.SubmitChanges();            
            linkNote.NoteId = note.Id;
            this.Context.LinkNotes.InsertOnSubmit(linkNote);
            this.Context.SubmitChanges();

            return note.Id;
        }

        
        /// <summary>
        /// Delete an annotation.  Does not actually delete the note - marks it as "IsDeleted" 
        /// </summary>
        /// <param name="id">The ID of the note to delete</param>
        /// <returns></returns>
        public bool DeleteNote(int id)
        {
            Note note = this.Context.Notes.FirstOrDefault(n => n.Id == id);
            if (note != null)
            {
                note.IsDeleted = true;
               
                this.Context.SubmitChanges();
                return true;
            } 
            return false;
        }

        // Gets the filename of attachment in sharepoint.  Filename will be NULL in db if there is no
        // file associated thus function returns NULL if no file.
        public Note GetNote(int id)
        {
            Note note = this.Context.Notes.FirstOrDefault(n => n.Id == id);
            if (note != null)
            {
                return note;
            }
            return null;
        }

        public int GetNameTypeId(string type)
        {
            var nameType = this.Context.NameTypes.FirstOrDefault(nt => nt.Name.ToLower() == type.ToLower());
            return nameType != null ? nameType.Id : -1;
        }

        public IList<ResourcePlan> GetResourcePlans()
        {
            if (!this.Context.ResourcePlans.Any()) return null;
            return this.Context.ResourcePlans.ToList();
        }

        public IList<ResourcePlanType> GetResourcePlanTypes()
        {
            if (!this.Context.ResourcePlanTypes.Any()) return null;
            return this.Context.ResourcePlanTypes.ToList();
        }

        public bool SaveResourcePlan(ResourcePlan rp)
        {
            if (rp != null)
            {
                ResourcePlan plan = this.Context.ResourcePlans.FirstOrDefault(n => n.Id == rp.Id);
                if (plan == null)
                {
                    this.Context.ResourcePlans.InsertOnSubmit(rp);
                }
                this.Context.SubmitChanges();
            }
            else
            {
                return false;
            }
            return true;

        }


        public IList<FiscalYear> GetFiscalYears()
        {
            if (!this.Context.FiscalYears.Any()) return null;
            return this.Context.FiscalYears.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="includeAll">
        /// True = All Fiscal Years returned
        /// False = Fiscal Years already attributed to a BRP will not to included in list returned
        /// </param>
        /// <returns></returns>
        public IList<FiscalYear> GetFiscalYears(bool includeAll)
        {
            var blackListYrs = this.Context.ResourcePlans.Where(rp => rp.ResourcePlanType.Value.ToLower() == "brp").Select(rp => rp.FiscalYear);
            if (includeAll || (blackListYrs == null || blackListYrs.Count() < 1))
                return GetFiscalYears();

            var fYears = this.Context.FiscalYears.Where(y => y.Year.HasValue && !blackListYrs.ToArray().Contains(y.Year.Value));
            return fYears != null ? fYears.ToList() : null;
        }

        public IList<ResourceStatuse> GetResourceStatuses()
        {
            if (!this.Context.ResourceStatuses.Any()) return null;
            return this.Context.ResourceStatuses.OrderBy(rs => rs.Name).ToList();
        }

        public IList<OwnershipType> GetOwnershipTypes()
        {
            if (!this.Context.OwnershipTypes.Any()) return null;
            return this.Context.OwnershipTypes.OrderBy(ot => ot.Value).ToList();
        }

        public IList<FuelType> GetFuelTypes()
        {
            if (!this.Context.FuelTypes.Any()) return null;
            return this.Context.FuelTypes.OrderBy(ft => ft.Name).ToList();
        }

        public IList<PowerCall> GetPowerCalls()
        {
            if (!this.Context.PowerCalls.Any()) return null;

            return this.Context.PowerCalls.ToList();
        }

        public IList<Batch> GetBatches()
        {
            if (!this.Context.Batches.Any()) return null;
            return this.Context.Batches.ToList();
        }

        public IList<Department> GetDepartments()
        {
            if (!this.Context.Departments.Any()) return null;
            return this.Context.Departments.ToList();
            
        }

        public Department GetDepartment(int Id)
        {
            var dept = Context.Departments.FirstOrDefault(x => x.Id == Id);

            return dept;
        }




        public void Dispose()
        {
            if (_context != null) _context.Dispose();
        }

        #endregion

        
    }
}
