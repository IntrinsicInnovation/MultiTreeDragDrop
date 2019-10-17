using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public partial class TvcStagedLoadMeasure 
    {
        public TvcStagedLoadMeasureLite ToLite()
        {
            return new TvcStagedLoadMeasureLite(this);
        }


        public class TvcStagedLoadMeasureLite
        {
            public string Id { get; set; }
            public string TvcId { get; set; }
            public string ActivationDate { get; set; }
            public string StagedLoadValue { get; set; }
            public string DateCreated { get; set; }
            public string UserCreatedId { get; set; }
            public string Comments { get; set; }

            public TvcStagedLoadMeasureLite(TvcStagedLoadMeasure tvc)
            {
                Id = tvc.Id.ToString();
                TvcId = tvc.TvcId.ToString();
                ActivationDate = tvc.ActivationDate.ToString("yyyy-MM-dd");
                StagedLoadValue = string.Format("{0:F2}", tvc.StagedLoadValue);
                DateCreated = tvc.DateCreated.ToString("yyyy-MM-dd");
                UserCreatedId = tvc.UserCreatedId.ToString();
                Comments = tvc.Comments;
            }

        }

    }
}
