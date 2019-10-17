using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain.ResourceOptions
{
    public partial class FinancialIndicator : IIndicator
    {
        public IIndicatorType IndicatorType
        {
            get { return this.FinancialIndicatorType; }
        }

        /// <summary>
        /// Financial IndicatorScenario Id
        /// </summary>
        public int ScenarioId
        {
            get { return this.FinancialIndicatorScenarioId; }
        }
    }
}
