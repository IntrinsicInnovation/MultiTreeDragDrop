using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain.ResourceOptions
{
    public partial class TechnicalIndicator : IIndicator
    {        
        public IIndicatorType IndicatorType
        {
            get { return this.TechnicalIndicatorType; }
        }

        /// <summary>
        /// Technical IndicatorScenario Id
        /// </summary>
        public int ScenarioId
        {
            get { return this.TechnicalIndicatorScenarioId; }
        }

    }
}
