using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain 
{
    class EpicBatches : EpicDomainBase
    {
        

        public EpicBatches(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        
    }
}
