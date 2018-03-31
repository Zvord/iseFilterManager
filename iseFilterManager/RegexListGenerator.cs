using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace iseFilterManager
{
    class RegexListGenerator
    {
        static Regex firstRegex = new Regex(@"WARNING:(\w+):(\d+) - (.*)\n");
        static Regex secondRegex = new Regex(@"(?<=[<'])\w+(?=[>'])");
        public static string[] Act(string input)
        {
            HashSet<string> found = new HashSet<string>();
            List<string> rStrings = new List<string>();
            var matches = firstRegex.Matches(input);
            foreach (Match m in matches)
            {
                if (found.Contains(m.Groups[2].Value))
                    continue;
                else
                    found.Add(m.Groups[2].Value);
                string outString = m.Groups[1].Value + " " + m.Groups[2].Value;
                string message = m.Groups[3].Value;
                if (!secondRegex.IsMatch(message))
                    continue;
                message = secondRegex.Replace(message, @"(\w+)");
                outString += " " + message;
                rStrings.Add(outString);
            }
            rStrings.Sort();
            return rStrings.ToArray();
        }
    }
}
