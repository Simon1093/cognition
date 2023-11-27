using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;

namespace input_generator.Classes
{
    class Helper
    {
        public static string CorrectSeparatorValue(string value, string separator)
        {
            string newValueStr = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (!Char.IsDigit(value[i]) && value[i] != '-' && value[i] != '+')
                {
                    newValueStr = newValueStr + separator;
                }
                else
                {
                    newValueStr = newValueStr + value[i];
                }
            }
            return newValueStr;
        }

        public static double ConvertToDouble(string s)
        {
            char systemSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
            double result = 0;
            try
            {
                if (s != null)
                    if (!s.Contains(","))
                        result = double.Parse(s, CultureInfo.InvariantCulture);
                    else
                        result = Convert.ToDouble(s.Replace(".", systemSeparator.ToString()).Replace(",", systemSeparator.ToString()));
            }
            catch (Exception e)
            {
                try
                {
                    result = Convert.ToDouble(s);
                }
                catch
                {
                    try
                    {
                        result = Convert.ToDouble(s.Replace(",", ";").Replace(".", ",").Replace(";", "."));
                    }
                    catch
                    {
                        throw new Exception("Wrong string-to-double format");
                    }
                }
            }
            return result;
        }


        public static string[] SplitStringArray(string source, params int[] sizes)
        {
            var length = sizes.Sum();
            if (length > source.Length) return null;

            var resultSize = sizes.Length;
            if (length < source.Length) resultSize++;

            var result = new string[resultSize];

            var start = 0;
            for (var i = 0; i < resultSize; i++)
            {
                if (i + 1 == resultSize)
                {
                    result[i] = source.Substring(start);
                    break;
                }

                result[i] = source.Substring(start, sizes[i]);
                start += sizes[i];
            }

            return result;
        }

    }
}
