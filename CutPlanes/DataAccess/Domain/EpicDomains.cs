using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public class EpicDomains : EpicDomainBase
    {
        public EpicDomains(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
    }
}
