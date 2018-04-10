using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace iseFilterManager
{
    class Warning
    {
        static Regex fileNameRegex = new Regex("\"(.*)\"");
        //static Regex sourceRegex   = new Regex(@"WARNING:(.*?):");
        //static Regex numberRegex   = new Regex(@"WARNING:.*?:(\d*)");

        public string Source { get; set; }
        public int Number { get; set; }
        public string Full { get; set; }
        //public string File { get; set; }
        public string ArgumentsString { get; set; }

        public List<string> Arguments;

        public Warning(string input)
        {
            Regex numberSourceRegex = new Regex(@"<msg type=""warning"" file=""(\w+)"" num=""(\d+)"".*?>");
            Regex bodyRegex = new Regex(@"<msg.*?>(.*)\r\n</msg>");
            Regex argumentRegex = new Regex(@"<arg fmt=""%\w"" index=""(\d+)"">(.*?)</arg>");
            Match match = numberSourceRegex.Match(input);
            Source = match.Groups[1].Value;
            Number = Convert.ToInt32(match.Groups[2].Value);
            match = bodyRegex.Match(input);
            string body = match.Groups[1].Value;
            var ms = argumentRegex.Matches(body);
            Arguments = new List<string>();
            ArgumentsString = "";
            foreach (Match m in ms)
            {
                string val = m.Groups[2].Value;
                Arguments.Add(val);
                ArgumentsString += val + ", ";
            }
            if (ms.Count > 0)
                ArgumentsString.Remove(ArgumentsString.Length - 3);
            string cleanArgs = argumentRegex.Replace(body, m => m.Groups[2].Value);
            string cleanLess = cleanArgs.Replace("&lt;", "<");
            string cleanGreater = cleanLess.Replace("&gt;", ">");
            string cleanApos = cleanGreater.Replace("&apos;", "'");
            string cleanQuotes = cleanApos.Replace("&quot;", "\"");
            Full = cleanQuotes;
        }

    }
}
