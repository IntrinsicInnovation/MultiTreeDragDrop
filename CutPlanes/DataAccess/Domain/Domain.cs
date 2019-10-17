using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public enum EpicMetersAggregateBy
    {
        Hour = 1,
        Day,
        Month,
        Year
    }

    public enum PlantResourceTypes
    {
        GeneratingUnit = 1,
        Meter,
        Line,
        ElectricityPurchaseAgreement,
        Substation
    }

    public enum SubstationResourceTypes
    {
        GeneratingUnit = 1,
        Meter,
        Line,
        Load,
        Plant
    }

    public enum LoadResourceTypes
    {
        Meter = 1,
        Line,
        Substation,
    }

    public enum GeneratingUnitResourceTypes
    {
        Meter = 1,
        Line,
        ElectricityPurchaseAgreement,
        Substation,
        Plant
    }

    public enum EPAResourceTypes
    {
        Plant = 1,
        GeneratingUnit,
        Meter,
        Line,
        Substation
    }

    public enum EpicTableHistoryType
    {
        Substation_SubstationAttributes = 1,
        Plants = 2,
        GeneratorAttributes = 3,
        GeneratingUnits = 4,
        GeneratorFactors = 5,
        Names = 6,
        ElectricalGroups = 7,
        Project = 8,
        TVC = 9
    }


}
