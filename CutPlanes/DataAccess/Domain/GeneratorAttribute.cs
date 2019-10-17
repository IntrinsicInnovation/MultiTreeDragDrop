using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public partial class GeneratorAttribute
    {
        /// <summary>
        /// Que
        /// </summary>
        public decimal? DerivedRatedUnderExcitedPowerFactor
        {
            get
            {
                decimal? value = null;
                
                if (this.LeadingPowerFactor.HasValue&& this.NamePlateCapacityInMW.HasValue && this.LeadingPowerFactor.Value < 1 && this.LeadingPowerFactor.Value != 0)                
                    value = -1 * Convert.ToDecimal(Convert.ToDouble(this.NamePlateCapacityInMW.Value) * Math.Tan(Math.Acos(Convert.ToDouble(this.LeadingPowerFactor.Value))));
                
                return value;
            }
        }

        /// <summary>
        /// Qoe
        /// </summary>
        public decimal? DerivedRatedOverExcitedPowerFactor
        {
            get
            {
                decimal? value = null;
                if (this.LaggingPowerFactor.HasValue && this.NamePlateCapacityInMW.HasValue && this.LaggingPowerFactor.Value < 1 && this.LaggingPowerFactor.Value != 0)                
                    value = Convert.ToDecimal(Convert.ToDouble(this.NamePlateCapacityInMW.Value) * Math.Tan(Math.Acos(Convert.ToDouble(this.LaggingPowerFactor.Value))));
                
                return value;
            }
        }

        /// <summary>
        /// Returns elcc for intermittent fuel types
        /// Returns dgcc for all other fuel types
        /// </summary>
        public decimal? DerivedSystemCapacity
        {
            get
            {
                if (this.GeneratingUnit != null && this.GeneratingUnit.FuelType != null && !this.GeneratingUnit.FuelType.IsNonIntermittent)
                    return this.EffectiveLoadCarryingCapacity;
                else
                    return this.DependableGeneratingCapacity;
            }
        }
        

        public GeneratorAttribute Clone()
        {
            GeneratorAttribute clone = new GeneratorAttribute();
            clone.GeneratingUnitId = this.GeneratingUnitId;
            clone.ResourcePlanId = this.ResourcePlanId;
            clone.ProjectPhaseId = this.ProjectPhaseId;           
            clone.DateEnd = this.DateEnd;
            clone.DateReplaced = this.DateReplaced;
            clone.InServiceStartDate = this.InServiceStartDate;
            clone.InServiceEndDate = this.InServiceEndDate;                    
            clone.DependableGeneratingCapacity = this.DependableGeneratingCapacity;
            clone.EffectiveLoadCarryingCapacity = this.EffectiveLoadCarryingCapacity;
            clone.EPACapacity = this.EPACapacity;
            clone.IsNitsNominated = this.IsNitsNominated;           
            clone.MaximumPowerOutput = this.MaximumPowerOutput;
            clone.MaxOutputForFirmTransmission = this.MaxOutputForFirmTransmission;
            clone.MaxOutputForNonFirmTransmission = this.MaxOutputForNonFirmTransmission;
            clone.LaggingPowerFactor = this.LaggingPowerFactor;
            clone.LeadingPowerFactor = this.LeadingPowerFactor;
            clone.MaxTakeOrPayCapacity = this.MaxTakeOrPayCapacity;
            clone.MonthlyAverageCapacity = this.MonthlyAverageCapacity;
            clone.NamePlateCapacityInMW = this.NamePlateCapacityInMW;
            clone.NitsDesignatedCapacity = this.NitsDesignatedCapacity;
            clone.PhysicalMinimumOutput = this.PhysicalMinimumOutput;
            clone.PointToPointCapacity = this.PointToPointCapacity;
            clone.Probability = this.Probability;           
            clone.RatedMVA = this.RatedMVA;           
            clone.RatedOverExcitedPowerFactor = this.RatedOverExcitedPowerFactor;
            clone.RatedPowerFactor = this.RatedPowerFactor;            
            clone.RatedUnderExcitedPowerFactor = this.RatedUnderExcitedPowerFactor;
            clone.RegulatoryMinimumOutput = this.RegulatoryMinimumOutput;
            clone.ReliabilityMustRunCapacity = this.ReliabilityMustRunCapacity;
            clone.SystemCapacity = this.SystemCapacity;
            return clone;
        }
    }
}
