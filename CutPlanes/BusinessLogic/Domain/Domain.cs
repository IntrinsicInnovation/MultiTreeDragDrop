using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicBusinessLogic.Domain
{
    public enum EpicResourceSortBy
    {
        TLA = 1,
        Name,
        Type,
        ElectricalGroup,
        DateModified
    }

    public enum EpicAnnotationType
    {
        Substation = 1,
        SynchronousMachine,
        GeneratingUnit,
        GeneratorFactor,
        NitsReport,
        Line,
        Load,
        ElectricityPurchaseAgreement,
        Project,
        Meter,
        ResourceBundle, 
        TransmissionServiceRequest,
        TransmissionVoltageCustomer
    }



   


}
