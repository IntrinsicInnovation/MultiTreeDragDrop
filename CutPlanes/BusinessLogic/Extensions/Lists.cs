using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicBusinessLogic.Extensions
{
    public static class Lists
    {
        /// <summary>
        /// Converts a list of type string to a CSV
        /// </summary>
        /// <param name="list">list of type string</param>
        /// <returns></returns>
        public static string ToCSV(this IList<string> list)
        {
            if (list == null || list.Count == 0) return null;

            char[] delim = {','};
            StringBuilder csv = new StringBuilder();
            foreach (string item in list)
                csv.Append(item + delim[0]);
            return csv.ToString().TrimEnd(delim);
        }
    }
}
