using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public partial class LoadAttribute
    {
        public LoadAttribute Clone()
        {
            LoadAttribute clone = new LoadAttribute();
            clone.LoadId = this.LoadId;
            clone.ResourcePlanId = this.ResourcePlanId;
            clone.ProjectPhaseId = this.ProjectPhaseId;            
            clone.DateEffective = this.DateEffective;
            clone.DateEnd = this.DateEnd;
            clone.DateReplaced = this.DateReplaced;
            clone.InServiceEndDate = this.InServiceEndDate;
            clone.InServiceStartDate = this.InServiceStartDate;
            clone.LoadGrossSummerMin = this.LoadGrossSummerMin;
            clone.LoadGrossSummerPeak = this.LoadGrossSummerPeak;
            clone.LoadGrossWinterMin = this.LoadGrossWinterMin;
            clone.LoadGrossWinterPeak = this.LoadGrossWinterPeak;
            clone.LoadMw = this.LoadMw;
            clone.PowerFactor = this.PowerFactor;
            clone.Probability = this.Probability;
            clone.ESALoadDemand = this.ESALoadDemand;
            clone.LoadGbl = this.LoadGbl;
            return clone;
        }
    }
}
