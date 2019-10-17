using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain.ResourceOptions
{
    public interface IIndicator
    {
        int Id { get; set; }
        int ScenarioId { get; }
        decimal Measure { get; set; }
        int ResourceOptionAttributeId { get; set; }
        IIndicatorType IndicatorType { get; }        
    }
}
