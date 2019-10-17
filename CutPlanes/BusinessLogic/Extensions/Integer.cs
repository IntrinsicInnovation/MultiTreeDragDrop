using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicBusinessLogic.Extensions
{
    public static class Integer
    {
        /// <summary>
        /// Returns a string representation of the value if one exists or null
        /// </summary>
        /// <param name="value">nullable int value</param>
        /// <returns></returns>
        public static string ToStringOrNull(this int? value)
        {
            return value.HasValue ? value.Value.ToString() : null;
        }

        /// <summary>
        /// Returns a string representation of the value if one exists or a dash (-)
        /// </summary>
        /// <param name="value">nullable decimal value</param>
        /// <returns></returns>
        public static string ToStringOrDash(this int? value)
        {
            return value.HasValue ? value.Value.ToString() : "-";
        }
    }
}
