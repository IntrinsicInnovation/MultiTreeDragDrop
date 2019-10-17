using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public class EpicCustomers : EpicDomainBase
    {
             
        public EpicCustomers(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// Create a new customer record
        /// </summary>
        /// <returns></returns>
        public Customer Create(String Name, int userId) {

            if (Name != null && Name.Length != 0)
            {
                var c = new Customer();
                c.Name = Name;
                c.DateCreated = DateTime.Now;
                c.UserCreatedId = userId;
                c.Description = "";

                this.Context.Customers.InsertOnSubmit(c);
                this.Context.SubmitChanges();
                return c;
            }
            return null;
        }

    }
}
