using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace iseFilterManager
{
    class RegexDict
    {
        public static Dictionary<Tuple<string, int>, Regex> Dict = new Dictionary<Tuple<string, int>, Regex>();
        private static Regex rx = new Regex(@"(\w+)\s(\d+)\s+(.*)");

        public static void RegexDictInit()
        {
            const string filename = "regex.list";
            string[] content = System.IO.File.ReadAllLines(filename);
            foreach (string str in content)
            {
                Match m = rx.Match(str);
                string source = m.Groups[1].Value;
                int number = Convert.ToInt32(m.Groups[2].Value);
                string regexString = m.Groups[3].Value;
                Regex regex = new Regex(regexString);
                Tuple<string, int> tuple = new Tuple<string, int>(source, number);
                Dict.Add(tuple, regex);
            }
        }
    }
}
