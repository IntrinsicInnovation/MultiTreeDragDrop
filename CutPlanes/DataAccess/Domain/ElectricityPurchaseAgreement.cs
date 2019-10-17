using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace EpicDataAccess.Domain
{
    public partial class ElectricityPurchaseAgreement : IResource
    {
        private string _conn;
        protected string ConnectionString
        {
            
            get
            {
                if (this._conn == null)
                {
                    this._conn = System.Configuration.ConfigurationManager.ConnectionStrings["EpicDB"].ConnectionString;

                }
                return this._conn;
            }
            set
            {
                this._conn = value;
            }

        }

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
            get { return this.Code; }
        }

        public string DateLastModifiedString
        {
            get { return (this.DateUpdated.HasValue ? this.DateUpdated.Value : this.DateCreated).ToString("yyyy-MM-dd"); }
        }

        public string TypeString
        {
            get { 
                return typeof(ElectricityPurchaseAgreement).Name;
            }
        }

        private string _navigationUrl = string.Empty;
        public string NavigationUrl
        {
            get
            {
                return string.Format("/Pages/Data/EPANavigation.aspx?code={0}&eid={1}", this.Code, this.Id);
            }
        }

        public decimal? Voltage
        {
            get
            {
                return null;
            }
        }

        public ElectricityPurchaseAgreement(string connectionString)
        {
            
            this.ConnectionString = connectionString;
        }

        //public ElectricityPurchaseAgreement()
        //{
        //    this.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EpicDB"].ConnectionString;
        //}

        /// <summary>
        /// Return a non-persistable clone of the EPA object
        /// </summary>
        /// <returns></returns>
        public ElectricityPurchaseAgreement Clone()
        {
            ElectricityPurchaseAgreement clone = new ElectricityPurchaseAgreement();
            clone.Code = this.Code;
            clone.NitsDesignatedCapacity = this.NitsDesignatedCapacity;
            clone.EPACapacity = this.EPACapacity;
            clone.EPAStatusId = this.EPAStatusId;
            clone.EstimatedInServiceDate = this.EstimatedInServiceDate;
            clone.ActualInServiceDate = this.ActualInServiceDate;
            clone.PowerCallId = this.PowerCallId;
            clone.OwnerName = this.OwnerName;
            return clone;
        }


        /// <summary>
        /// Returns the name of the PowerCall with null protection
        /// </summary>
        private string _pc = null;
        public string PowerCallName
        {
            get
            {
                if (this.PowerCall != null)
                    _pc = this.PowerCall.Value;
                else if (this.PowerCall == null && this.PowerCallId.HasValue)
                {
                    var pc = this.Context.PowerCalls.FirstOrDefault(x => x.Id == this.PowerCallId.Value);
                    if (pc != null)
                        _pc = pc.Value;
                }
                return _pc;
            }
        }

        private string _status = null;
        public string Status
        {
            get
            {
                if (this.EPAStatuse != null)
                    _status = this.EPAStatuse.Name;
                else
                {
                    var s = this.Context.EPAStatuses.FirstOrDefault(x => x.ID == this.EPAStatusId);
                    _status = s.Name ?? "";
                }
                return _status;

            }

        }


        private Substation[] _substations = null;
        public Substation[] Substations
        {
            get
            {
                if (_substations != null) return _substations;
                var sl = this.LinkElectricityPurchaseAgreementsSubstations.Select(lnk => lnk.Substation);
                if (sl != null && sl.Count() > 0)
                    _substations = sl.ToArray();
                else
                {
                    var links = this.Context.LinkElectricityPurchaseAgreementsSubstations.Where(l => l.EPAId == this.Id);

                    if (links != null)
                    {
                        var ssl = links.Select(lnk => lnk.Substation);
                        _substations = ssl != null ? ssl.ToArray() : null;
                    }
                }
                return _substations;
            }
        }

    }
}
