using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicBusinessLogic.Extensions
{
    public static class Bool
    {
        /// <summary>
        /// Returns a string representation of the value if one exists or null
        /// </summary>
        /// <param name="value">nullable boolean value</param>
        /// <returns></returns>
        public static string ToStringOrNull(this bool? value)
        {
            return value.HasValue ? value.Value.ToString() : null;
        }
    }
}
