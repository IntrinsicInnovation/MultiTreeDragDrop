using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EpicBusinessLogic.Extensions
{
    public static class StringHelper
    {
        public static string[] SplitWindowsUserName(this string userName)
        {
            char[] delim = {'\\'};
            return userName.Split(delim);
        }

        /// <summary>
        /// Converts a string to a DateTime object
        /// </summary>
        /// <param name="date">string representaion of a date</param>
        /// <param name="formatInfo">Date formatting information object</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string date, DateTimeFormatInfo formatInfo)
        {
            DateTime _date;
            DateTime.TryParse(date, formatInfo, DateTimeStyles.None, out _date);
            return _date;
        }

        /// <summary>
        /// Converts a string in mm/dd/yyyy format to a DateTime object
        /// </summary>
        /// <param name="date">string representaion of a date</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string date)
        {
            DateTimeFormatInfo formatInfo = new DateTimeFormatInfo();
            formatInfo.DateSeparator = "/";
            formatInfo.ShortDatePattern = "MM/dd/yyyy";
            return date.ToDateTime(formatInfo);            
        }

        /// <summary>
        /// Converts a string in mm/dd/yyyy format to a DateTime object or Null if input is null
        /// </summary>
        /// <param name="date">string representaion of a date</param>
        /// <returns>Nullable DateTime object</returns>
        public static DateTime? ToNullableDateTime(this string date)
        {
            if (string.IsNullOrEmpty(date)) return null;
            DateTimeFormatInfo formatInfo = new DateTimeFormatInfo();
            formatInfo.DateSeparator = "/";
            formatInfo.ShortDatePattern = "MM/dd/yyyy";
            DateTime _date;
            bool success = DateTime.TryParse(date, formatInfo, DateTimeStyles.None, out _date);
            if (!success) return null;
            return _date;
        }

        /// <summary>
        /// Attempts to convert a string to an int32 value.  Default value = 0
        /// </summary>
        /// <param name="number">string representation of an integer value</param>
        /// <returns></returns>
        public static int ToInt32(this string number)
        {
            int n = 0;
            bool success = int.TryParse(number, out n);            
            return n;
        }

        /// <summary>
        /// Attempts to convert a string to an int32 value.  Default value = int.MinValue
        /// MTM: the ToInt32 returns int.MinValue if the field is 0.00 ... 
        /// </summary>
        /// <param name="number">string representation of an integer value</param>
        /// <returns></returns>
        public static int ToInt32WithZero(this string number)
        {
            int n = 0;
            if (int.TryParse(number, out n))
            {
                return n;
            }
            else
            {
                return 0;
            }

        }

        /// <summary>
        /// Attempts to convert a string to an nullable integer value.  Default value = null
        /// </summary>
        /// <param name="number">string representation of a nullable integer value</param>
        /// <returns></returns>
        public static int? ToNullableInt32(this string number)
        {
            if (string.IsNullOrEmpty(number)) return null;
            int n = 0;            
            bool ok = int.TryParse(number, out n);
            if (!ok) return null;
            return n;
        }

        /// <summary>
        /// Attempts to convert a string to an decimal value.  Default value = -1
        /// </summary>
        /// <param name="number">string representation of an decimal value</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string number)
        {
            decimal n = 0;
            if (decimal.TryParse(number, out n))
                return n;
            else
                return -1;
        }

        public static decimal ToDecimalWithZero(this string number)
        {
            decimal d = 0;
            if (decimal.TryParse(number, out d))            
                return d;            
            else            
                return 0;
        }

        /// <summary>
        /// Attempts to convert a string to an nullable decimal value.  Default value = null
        /// </summary>
        /// <param name="number">string representation of a nullable decimal value</param>
        /// <returns></returns>
        public static decimal? ToNullableDecimal(this string number)
        {
            if(string.IsNullOrEmpty(number)) return null;
            decimal n = 0;
            bool ok = decimal.TryParse(number, out n);
            if (!ok) return null;
            return n;
        }

        /// <summary>
        /// Attempts to convert a string to an nullable decimal value.  Default value = null.
        /// MTM: Note: The ToNullableDecimal will return null for 0.00.  This method does not.
        /// </summary>
        /// <param name="number">string representation of a nullable decimal value</param>
        /// <returns></returns>
        public static decimal? ToNullableDecimalWithZero(this string number)
        {
            if (string.IsNullOrEmpty(number)) return null;
            decimal n = 0;
            if (decimal.TryParse(number, out n))
            {
                return n;
            }
            else
            {
                return null;
            }
        }



        /// <summary>
        /// Attempts to convert a string to a double value.  Default value = 0
        /// </summary>
        /// <param name="number">string representation of a double value</param>
        /// <returns></returns>
        public static double ToDouble(this string number)
        {
            if (string.IsNullOrEmpty(number)) return 0;
            double n = 0;
            bool ok = double.TryParse(number, out n);
            if (!ok) return 0;
            return n;
        }




        /// <summary>
        /// Attempts to convert a string to an boolen value.  Default value = false
        /// </summary>
        /// <param name="number">string representation of a boolen value</param>
        /// <returns></returns>
        public static bool ToBool(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            string[] values = { "true", "false" };
            if (!values.Contains(value.ToLower())) return false;
            bool b = false;
            bool.TryParse(value, out b);
            return b;
        }

        /// <summary>
        /// Attempts to convert a string to an nullable boolen value.  Default value = null
        /// </summary>
        /// <param name="number">string representation of a nullable boolen value</param>
        /// <returns></returns>
        public static bool? ToNullableBool(this string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            string[] values = {"true", "false"};
            if (!values.Contains(value.ToLower())) return null;
            bool b = false;
            bool.TryParse(value, out b);
            return b;
        }

        /// <summary>
        /// Inserts spaces in input text string after all upper case alpha characters except index 0.
        /// </summary>
        /// <param name="text">text string</param>
        /// <returns>formatted text string</returns>
        public static string InsertSpaces(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            Regex regex = new Regex("[A-Z][a-z]");
            Match m = null;
            int index = 1;           
            while (true)
            {
                if (index > text.Length - 1) break;
                m = regex.Match(text, index);
                if (!m.Success) break;                
                text = text.Insert(m.Index, " ");
                index = m.Index + 2;
            }           
            return text;
        }


        public static string SafeSubstring(this string str, int start, int count)
        {
            if (string.IsNullOrEmpty(str))
                return String.Empty;
            int n = str.Length;
            return str.Substring(Math.Min(start, n), Math.Min(count, n));
        }

    }
}
