using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpicBusinessLogic;
//using EpicDataAccess.Domain.ResourceOptions;

namespace EpicBusinessLogic.Extensions
{
    public static class EnumerableHelper
    {
        public static void Update<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }

        //public static IEnumerable<T> OrderBy<T, TResult>(this IEnumerable<T> source, Func<T, TResult> sortParamSelector, SortDirection dir)
        //{
        //    if (dir == SortDirection.Desc)
        //        return source.OrderByDescending(sortParamSelector);
        //    return source.OrderBy(sortParamSelector);
        //}

        /// <summary>
        /// Returns the sum of the collection properties indenified by the input propertyPredicate and predicated by the input financial indicatorPredicate
        /// </summary>
        /// <param name="source">collection of ResourceOptionAttribute objects</param>
        /// <param name="indicatorPredicate">FinancialIndicator predicate</param>
        /// <param name="propertyPredicate">ResourceOptionAttribute property predicate</param>
        /// <returns>Sum</returns>
        //public static decimal PredicatedSum(this IEnumerable<ResourceOptionAttribute> source,  Func<FinancialIndicator, bool> indicatorPredicate, Func<ResourceOptionAttribute, decimal> propertyPredicate)
        //{
        //    source.Update(roa => roa.FinancialIndicatorPredicate = indicatorPredicate);
        //    return source.Sum(propertyPredicate);
        //}

        /// <summary>
        /// Returns the sum of the collection properties indenified by the input propertyPredicate and predicated by the input non-financial indicatorPredicate
        /// </summary>
        /// <param name="source">collection of ResourceOptionAttribute objects</param>
        /// <param name="indicatorPredicate">IIndicator predicate</param>
        /// <param name="propertyPredicate">ResourceOptionAttribute property predicate</param>
        /// <returns>Sum</returns>
        //public static decimal PredicatedSum(this IEnumerable<ResourceOptionAttribute> source, Func<IIndicator, bool> indicatorPredicate, Func<ResourceOptionAttribute, decimal> propertyPredicate)
        //{
        //    source.Update(roa => roa.IndicatorPredicate = indicatorPredicate);
        //    return source.Sum(propertyPredicate);
        //}
    }
}
