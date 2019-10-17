using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public class EpicFacilityTempCodes : EpicDomainBase
    {
        public EpicFacilityTempCodes(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        
        public List<Department> GetDepartments() {
            var y = this.Context.Departments.ToList().OrderBy(x => x.Id);

            return y.ToList();
        }

        public List<Customer> GetCustomers()
        {
            var y = this.Context.Customers.ToList().OrderBy(x => x.Id);

            return y.ToList();
        }

        public List<FacilityCodeType> GetFacilityCodeTypes() {
            var y = this.Context.FacilityCodeTypes.ToList().OrderBy(x => x.Id);

            return y.ToList();

        }

        public FacilityCodeType GetFacilityCodeType(int id)
        {
            var y = this.Context.FacilityCodeTypes.FirstOrDefault(x => x.Id == id);

            return y;
        }


        public List<FacilityTempCode> GetFacilityTempCodes() {
            var y = this.Context.FacilityTempCodes.ToList().OrderBy(x => x.Id);

            return y.ToList();
        }


        public FacilityTempCode CreateTempFacilityCode(string Name, string Code, int FacilityCodeTypeId, int userId)
        {
            var f = new FacilityTempCode();

            f.Name = Name;
            f.Code = Code;

            f.DateCreated = DateTime.Now;
            f.UserCreatedId = userId;
            f.FacilityCodeTypeId = FacilityCodeTypeId;

            this.Context.FacilityTempCodes.InsertOnSubmit(f);
            this.Context.SubmitChanges();
            return f;
        }


        /// <summary>
        /// Update an existing temporary facility code.  This is called when a user clicks the save dialog
        /// on the web application
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public FacilityTempCode UpdateTempFacilityCode(int id, string Name, string Code)
        {
            var f = this.Context.FacilityTempCodes.FirstOrDefault(x => x.Id == id);
            if (f == null)
            {
                return null;
            }

            f.Name = Name;
            f.Code = Code;  
            
            this.Context.SubmitChanges();
            return f;
        }

        /// <summary>
        /// Return the facility temp code identified by a particular ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FacilityTempCode GetFacilityTempCode(int id)
        {
            return this.Context.FacilityTempCodes.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Return the next available facility temp code.   The system will create the record in the database - if the
        /// user cancels, the record should be deleted from the database or else it will no longer be available.
        /// </summary>
        /// <param name="FacilityCodeTypeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public FacilityTempCode GetNextFacilityCode(int FacilityCodeTypeId, int userId)
        {
            var codeType = GetFacilityCodeType(FacilityCodeTypeId);
            var count = this.Context.FacilityTempCodes.Where(x => x.FacilityCodeTypeId == FacilityCodeTypeId).Count();
                

            //string code = codeType.Prefix;
            String code = "Z";
            code += String.Format("{0:D3}", count + 1);

            var facilityCode = CreateTempFacilityCode("", code, FacilityCodeTypeId, userId);
            return facilityCode;
        }


        /// <summary>
        /// Delete an existing facility code.   This is called by the web application when a user cancels the 
        /// create facility code dialog.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteFacilityCode(int id)
        {
            var code = this.Context.FacilityTempCodes.FirstOrDefault(x => x.Id == id);
            if (code == null)
            {
                return false;
            }

            this.Context.FacilityTempCodes.DeleteOnSubmit(code);
            this.Context.SubmitChanges();

            return true;
        }

    }
}
