using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using System.Globalization;

class Helper
{
    public class ReplaceString
    {
        static readonly IDictionary<string, string> m_replaceDict
            = new Dictionary<string, string>();

        const string ms_regexEscapes = @"[\a\b\f\n\r\t\v\\""]";

        public static string StringLiteral(string i_string)
        {
            return Regex.Replace(i_string, ms_regexEscapes, match);
        }

        public static string CharLiteral(char c)
        {
            return c == '\'' ? @"'\''" : string.Format("'{0}'", c);
        }

        private static string match(Match m)
        {
            string match = m.ToString();
            if (m_replaceDict.ContainsKey(match))
            {
                return m_replaceDict[match];
            }

            throw new NotSupportedException();
        }

        static ReplaceString()
        {
            m_replaceDict.Add("\a", @"\a");
            m_replaceDict.Add("\b", @"\b");
            m_replaceDict.Add("\f", @"\f");
            m_replaceDict.Add("\n", @"\n");
            m_replaceDict.Add("\r", @"\r");
            m_replaceDict.Add("\t", @"\t");
            m_replaceDict.Add("\v", @"\v");

            m_replaceDict.Add("\\", @"\\");
            m_replaceDict.Add("\0", @"\0");

            //The SO parser gets fooled by the verbatim version 
            //of the string to replace - @"\"""
            //so use the 'regular' version
            m_replaceDict.Add("\"", "\\\"");
        }
    }

    public static double ConvertToDouble(string s)
    {
        char systemSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
        double result = 0;
        try
        {
            if (s != null && s != "")
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

    public static string CollectGenerationRulesToYAML(List<GenerationRules> generationRules)
    {
        OrderedDictionary rules = new OrderedDictionary();

        rules.Add("Delimiter", String.Format("r_bracket{0}r_bracket", generationRules[0].delimiter));
        rules.Add("Separator", String.Format("r_bracket{0}r_bracket", generationRules[0].separator));
        rules.Add("StartDrawMatrixAtLine", String.Format("r_bracket{0}r_bracket", generationRules[0].matrixAtLine));
        rules.Add("NotationRules", generationRules[0].notation);
        rules.Add("Type", String.Format("r_bracket{0}r_bracket", "square"));
        object a = rules;
        var serializer = new YamlDotNet.Serialization.Serializer();
        return serializer.Serialize(a);
    }
}

