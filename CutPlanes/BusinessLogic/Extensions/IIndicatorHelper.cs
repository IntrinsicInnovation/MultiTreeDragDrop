using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using EpicDataAccess.Domain.ResourceOptions;

namespace EpicBusinessLogic.Extensions
{
    public static class IIndicatorHelper
    {
        /// <summary>
        /// Returns the aggregated measure value for all Indicators in the collection where unit of measure is not time based
        /// </summary>
        /// <param name="collection">collection of type IIndicator</param>
        /// <returns></returns>
        //public static decimal SumNonTimeMeasures(this IEnumerable<IIndicator> collection)
        //{
        //    var unitOfMeasure = collection.First().IndicatorType.UnitOfMeasurement;
        //    if (unitOfMeasure == "yr(s)" || string.IsNullOrEmpty(unitOfMeasure))
        //        return collection.First().Measure;
        //    return collection.Sum(i => i.Measure);
        //}

        /// <summary>
        /// Returns the aggregated measure value for all Indicators linked to the input scenarioId
        /// </summary>
        /// <param name="collection">collection of type IIndicator</param>
        /// <returns></returns>
        //public static decimal ScenarioSum(this IEnumerable<IIndicator> collection, int scenarioId)
        //{
        //    return collection.Where(i => i.ScenarioId == scenarioId).Sum(i => i.Measure);
        //}

        /// <summary>
        /// Returns the aggregated measure value for all Indicators linked to the input Discount Rate Id
        /// </summary>
        /// <param name="collection">collection of type IIndicator</param>
        /// <returns></returns>
        //public static decimal DiscountRateSum(this IEnumerable<FinancialIndicator> collection, int discountRateId)
        //{
        //    return collection.Where(fi => fi.LinkFinIndicatorDiscountRates.Any(lnk => lnk.DiscountRateId == discountRateId)).Sum(i => i.Measure);
        //}
    }
}
