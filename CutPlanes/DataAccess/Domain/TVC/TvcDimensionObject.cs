using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider;


namespace EpicDataAccess.Domain.TVC
{
    public class TvcDimensionObject
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserCreated { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

      
    }
}
