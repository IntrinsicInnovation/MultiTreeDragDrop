using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain.Facilities
{
    //[Obsolete("Not used anymore... Facility is a presentation layer synonym of Substation", true)]
    /// <summary>
    /// Obsolete... Facility is a presentation layer synonym of Substation"
    /// </summary>
    public class EpicFacilities : IDisposable
    {

        #region [Properties]

        private string ConnectionString { get; set; }
        
        private EpicFacilityDataContext _context = null;
        private EpicFacilityDataContext Context
        {
            get
            {
                if (_context == null) _context = new EpicFacilityDataContext(ConnectionString);
                return _context;
            }
        }

        #endregion

        #region [Constructor]

        public EpicFacilities(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        #endregion

        #region [Methods]

        public IList<FacilityLite> GetFaciltiesList(string type)
        {
            string[] types = { type };
            return GetFaciltiesList(types);
        }

        public IList<FacilityLite> GetFaciltiesList(string[] types)
        {
            var facilities = from t in this.Context.FacilityTypes.Where(type => types.Contains(type.Name))
                             join l in this.Context.LinkFacilityTypeFaclities on t.ID equals l.FacilityTypeId
                             join f in this.Context.Facilities on l.FacilityId equals f.Id
                             orderby f.FacilityCode
                             select new FacilityLite(f.Id, f.FacilityCode, f.FacilityName, t.Name, string.Empty, f.DateCreated, f.DateUpdated);
            return facilities != null ? facilities.ToList() : null;
        }

        // Return a single facility record from lookup table
        // added MTM: 2012-02-08
        //
        public Facility GetFacility(string facilityCode)
        {
           var facility = this.Context.Facilities.FirstOrDefault(fa => fa.FacilityCode.Equals(facilityCode));

           return facility;
        }


        public IList<FacilityStatuse> GetFacilityStatuses()
        {
            return this.Context.FacilityStatuses.ToList();
        }


        public IList<FacilityType> GetFacilityTypes()
        {
            return this.Context.FacilityTypes.ToList();
        }


        #endregion

        public void Dispose()
        {
            if (_context != null) _context.Dispose();
        }
    }
}
