using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicBusinessLogic.Extensions
{
    public static class ObjectHelper
    {
        /// <summary>
        /// Returns an unformated string representation of the value or null
        /// </summary>
        /// <param name="value">object</param>
        /// <returns></returns>
        public static string ToStringOrNull(this object value)
        {
            if (value == null) return null;
            return value.ToString();
        }

        public static int? ToNullableInt32(this object value)
        {
            if (value == null) return null;
            int n = 0;
            bool ok = int.TryParse(value.ToString(), out n);
            if (!ok) return null;
            return n;
        }

        public static bool? ToNullableBool(this object value)
        {
            if (value == null) return null;
            bool b = false;
            bool ok = bool.TryParse(value.ToString(), out b);
            if (!ok) return null;
            return b;
        }
    }
}
