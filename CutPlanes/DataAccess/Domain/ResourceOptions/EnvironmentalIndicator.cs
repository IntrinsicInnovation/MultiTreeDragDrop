using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain.ResourceOptions
{
    public partial class EnvironmentalIndicator : IIndicator
    {
        public IIndicatorType IndicatorType
        {
            get { return this.EnvironmentalIndicatorType; }
        }

        /// <summary>
        /// Environmental IndicatorScenario Id
        /// </summary>
        public int ScenarioId
        {
            get { return this.EnvironmentalIndicatorScenarioId; }
        }
    }
}
