using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicBusinessLogic.Extensions
{
    public static class Decimal
    {
        /// <summary>
        /// Returns a string representation of the value if one exists or null
        /// </summary>
        /// <param name="value">nullable decimal value</param>
        /// <returns></returns>
        public static string ToStringOrNull(this decimal? value)
        {
            return value.HasValue ? value.Value.ToString("###0.0#########") : null;
        }

        /// <summary>
        /// Returns a string representation of the value if one exists or a dash (-)
        /// </summary>
        /// <param name="value">nullable decimal value</param>
        /// <returns></returns>
        public static string ToStringOrDash(this decimal? value)
        {
            return value.HasValue ? value.Value.ToString("###0.0#########") : "-";
        }

        /// <summary>
        /// Returns a currency formatted string representation of the decimal value (###0.00)
        /// </summary>
        /// <param name="value">nullable decimal value</param>
        /// <returns>string</returns>
        public static string ToCStringOrNull(this decimal? value)
        {
            return value.HasValue ? value.Value.ToString("###0.00") : null;
        }

        /// <summary>
        /// Returns a formatted string representation of the decimal value (###0.0##)
        /// </summary>
        /// <param name="value">decimal value</param>
        /// <returns>string</returns>
        public static string ToFString(this decimal value)
        {
            return value.ToString("###0.00");
        }

        /// <summary>
        /// Returns a formatted string representation of the decimal value.  The format returned is dependant upon the input indicator Label and unit of measure (ref: resource options)
        /// </summary>
        /// <param name="value">decimal value</param>       
        /// <param name="label">resource option indicator unit of measure</param>
        /// <returns>string</returns>
        public static string ToFString(this decimal value, string uOM)
        {
            if (uOM == "yr(s)" || string.IsNullOrEmpty(uOM))
                return value.ToString("###0");
            if (uOM == "%")
                return (value * 100).ToString("###0");
            return value.ToString("###0.00");
        }

        /// <summary>
        /// Returns a currency formatted string representation of the decimal value (###0.00)
        /// </summary>
        /// <param name="value">decimal value</param>
        /// <returns>string</returns>
        public static string ToCString(this decimal value)
        {
            return value.ToString("###0.00");
        }
    }
}
