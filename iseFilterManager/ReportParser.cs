using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace iseFilterManager
{
    class ReportParser
    {
        /// <summary>
        /// Returns list of all warnings in plain text
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static List<string> TextGetter(string input)
        {
            Regex rx = new Regex(@"<msg type=""warning"".*?>(.*?)</msg>", RegexOptions.Singleline);
            var matches = rx.Matches(input);
            List<string> list = new List<string>();
            foreach (Match m in matches)
                list.Add(m.Value);
            return list;
        }

        static List<Warning> WarningGetter(List<string> input)
        {
            List<Warning> list = new List<Warning>();
            foreach (string str in input)
                list.Add(new Warning(str));
            return list;
        }

        public static List<Warning> Main(string input)
        {
            var plain = TextGetter(input);
            var list = WarningGetter(plain);
            return list;
        }
    }
}
