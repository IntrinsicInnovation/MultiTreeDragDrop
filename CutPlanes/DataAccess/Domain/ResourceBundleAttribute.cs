using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public partial class ResourceBundleAttribute
    {

        /// <summary>
        /// Inferred from ResourceOptionAttribute
        /// </summary>
        public bool SentToSystemOptimizer
        {
            get
            {                
                var rOA = this.ResourceOptionAttributes.FirstOrDefault();
                return (rOA == null || !rOA.SentToSysOptiimizer.HasValue) ? false : rOA.SentToSysOptiimizer.Value;
            }
        }

        public int? DataSourceId
        {
            get
            {
                var rOA = this.ResourceOptionAttributes.FirstOrDefault();
                return rOA == null ? rOA.DataSourceId : null;
            }
        }
        
        public ResourceBundleAttribute Clone()
        {
            ResourceBundleAttribute clone = new ResourceBundleAttribute();            
            clone.CommercialOperationEndDate = this.CommercialOperationEndDate;
            clone.CommercialOperationStartDate = this.CommercialOperationStartDate;           
            clone.DateEffective = this.DateEffective;
            clone.DateEnd = this.DateEnd;
            clone.DateReplaced = this.DateReplaced;
            clone.DependableGenerationCapacity = this.DependableGenerationCapacity;
            clone.EffectiveLoadCarryingCapacity = this.EffectiveLoadCarryingCapacity;
            clone.OriginalId = this.OriginalId;
            clone.PotentialIncludeCapacity = this.PotentialIncludeCapacity;
            clone.ResourceBundleId = this.ResourceBundleId;
            clone.ResourcePlanId = this.ResourcePlanId;
            clone.SystemCapacity = this.SystemCapacity;
            clone.MaximumPowerOutputCapacity = this.MaximumPowerOutputCapacity;
            clone.NitsDesignatedCapacity = this.NitsDesignatedCapacity;
            clone.IsDesignated = this.IsDesignated;
            clone.NamePlateCapacityInMW = this.NamePlateCapacityInMW;
            clone.FirmEnergy = this.FirmEnergy;
            clone.TotalEnergy = this.TotalEnergy;
            clone.UnitEnergyCostStart = this.UnitEnergyCostStart;
            clone.UnitEnergyCostEnd = this.UnitEnergyCostEnd;
            clone.EnergyAvailableYear = this.EnergyAvailableYear;
            clone.AverageEnergy = this.AverageEnergy;
            clone.Probability = this.Probability;
            clone.EnergyEfficiency = this.EnergyEfficiency;
            clone.DiscountRateId = this.DiscountRateId;
            return clone;
        }
    }
}
