using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain.ResourceOptions
{
    public interface IIndicatorType
    {
        int Id { get; set; }
        string Label { get; set; }
        string Abbreviation { get; set; }
        string UnitOfMeasurement { get; set; }
        string Subtype { get; set; }
    }
}
