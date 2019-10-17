using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public partial class SubstationAttribute
    {
        public SubstationAttribute Clone()
        {
            SubstationAttribute clone = new SubstationAttribute();
            clone.SubstationId = this.SubstationId;              
            clone.DateEffective = this.DateEffective;
            clone.DateEnd = this.DateEnd;
            clone.DateReplaced = this.DateReplaced;            
            clone.InServiceEndDate = this.InServiceEndDate;
            clone.InServiceStartDate = this.InServiceStartDate;
            clone.InterconnectionsRequestCapacity = this.InterconnectionsRequestCapacity;
            clone.DependableGeneratingCapacity = this.DependableGeneratingCapacity;
            clone.EffectiveLoadCarryingCapacity = this.EffectiveLoadCarryingCapacity;
            clone.EPACapacity = this.EPACapacity;
            clone.LoadGrossSummerPeak = this.LoadGrossSummerPeak;
            clone.LoadGrossWinterPeak = this.LoadGrossWinterPeak;
            clone.LoadGrossSummerMin = this.LoadGrossSummerMin;
            clone.LoadGrossWinterMin = this.LoadGrossWinterMin;
            clone.MaxOutputForFirmTransmission = this.MaxOutputForFirmTransmission;
            clone.MaxOutputForNonFirmTransmission = this.MaxOutputForNonFirmTransmission;
            clone.MaximumPowerOutput = this.MaximumPowerOutput;            
            clone.MaxTakeOrPayCapacity = this.MaxTakeOrPayCapacity;
            clone.NamePlateCapacityInMW = this.NamePlateCapacityInMW;
            clone.NitsDesignatedCapacity = this.NitsDesignatedCapacity;
            clone.Nominated = this.Nominated;
            clone.OriginalId = this.OriginalId;
            clone.PointToPointCapacity = this.PointToPointCapacity;
            clone.Probability = this.Probability;
            clone.ProjectPhaseId = this.ProjectPhaseId;
            clone.ResourcePlanId = this.ResourcePlanId;           
            clone.RatedMVA = this.RatedMVA;            
            clone.RatedOverExcitedMvar = this.RatedOverExcitedMvar;
            clone.RatedOverExcitedPowerFactor = this.RatedOverExcitedPowerFactor;
            clone.RatedPowerFactor = this.RatedPowerFactor;            
            clone.RatedUnderExcitedMvar = this.RatedUnderExcitedMvar;
            clone.RatedUnderExcitedPowerFactor = this.RatedUnderExcitedPowerFactor;
            clone.ReliabilityMustRunCapacity = this.ReliabilityMustRunCapacity;
            clone.UsePdgcMasteredValue = this.UsePdgcMasteredValue;
            clone.UsePmpoMasteredValue = this.UsePmpoMasteredValue;
            clone.UsePptpMasteredValue = this.UsePptpMasteredValue;
            clone.UsePrmrMasteredValue = this.UsePrmrMasteredValue;
            clone.UsePndcMasteredValue = this.UsePndcMasteredValue;
            clone.UsePnpMasteredValue = this.UsePnpMasteredValue;
            clone.UsePdmx0MasteredValue = this.UsePdmx0MasteredValue;
            clone.UsePdmx1MasteredValue = this.UsePdmx1MasteredValue;
            clone.UsePelccMasteredValue = this.UsePelccMasteredValue;
            return clone;
        }
    }
}
