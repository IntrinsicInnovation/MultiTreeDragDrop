using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicBusinessLogic.Extensions
{
    public static class Date
    {
         /// <summary>
        /// Returns a string representation (yyyy-MM-dd format) of the DateTime value if one exists or null
        /// </summary>
        /// <param name="value">nullable DateTime value</param>
        /// <returns></returns>
        public static string ToStringOrNull(this DateTime? value)
        {
            return value.HasValue ? value.Value.ToString("yyyy-MM-dd") : null;
        }
    }
}
