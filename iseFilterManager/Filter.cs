using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace iseFilterManager
{
    class Filter : IEquatable<Filter>
    {
        public string Source { get; set; }
        public int Number { get; set; }
        private List<string> arguments;
        public string ArgumentsString { get; set; }
        public string Full { get; set; }
        public bool Enabled { get; set; }
        public int NumberOfMatches { get; set; }
        public List<string> Arguments
        {
            get { return arguments; }
            set
            {
                arguments = value;
                UpdateArgumentsString();
                UpdateFull();
            }
        }
        
        

        static Regex SourceNumberRegex = new Regex(@"file=""(\w+)"" num=""(\d+)""");
        static Regex ArgumentRegex = new Regex(@"<arg index=""(\d+)""( match_type=""wildcard"")?>(?<="">)(.+?)(?=</arg>)");

        public Filter(string source, int number, List<string> arguments)
        {
            Enabled = true;
            Source = source;
            Number = number;
            Arguments = arguments;
            NumberOfMatches = 0;
            UpdateFull();
            UpdateArgumentsString();
        }

        /// <summary>
        /// Parse filter from a filter file string
        /// </summary>
        /// <param name="input"></param>
        public Filter(string input)
        {
            Enabled = true;
            Match m = SourceNumberRegex.Match(input);
            Source = m.Groups[1].Value;
            Number = Convert.ToInt32(m.Groups[2].Value);
            Full = input;
            var matches = ArgumentRegex.Matches(input);
            this.arguments = new List<string>();
            foreach (Match mm in matches)
            {
                arguments.Add(mm.Groups[3].Value);
            }
            UpdateArgumentsString();
        }

        static string WrapArgument(string arg, int n)
        {
            return "<arg index=\"" + n.ToString() + "\" match_type=\"wildcard\">" + arg + "</arg>";
        }

        public override int GetHashCode()
        {
            return Source.GetHashCode() + Number.GetHashCode() + Arguments.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj as Filter);
        }

        public bool Equals(Filter another)
        {
            //Filter another = obj as Filter;
            return Source == another.Source && Number == another.Number && Arguments.SequenceEqual(another.Arguments);
        }

        private void UpdateArgumentsString()
        {
            ArgumentsString = "";
            if (arguments.Count == 0)
                return;
            foreach (string str in arguments)
                ArgumentsString += str + ", ";
            ArgumentsString = ArgumentsString.Remove(ArgumentsString.Length - 2);
        }

        private void UpdateFull()
        {
            Full = "<filter task=\"xst\" file=\"" + Source + "\" num=\"" + Number.ToString() + "\" type=\"warning\">";
            int n = 1;
            foreach (string arg in Arguments)
            {
                Full += WrapArgument(arg, n);
                n++;
            }
            Full += "</filter>";
        }

        public static bool FilterWarning(List<Filter> list, Warning warn)
        {
            List<Filter> filts = FindFilter(list, warn);
            if (filts == null)
                return false;
            foreach (Filter f in filts)
            {
                if (f.Apply(warn) == true)
                {
                    f.NumberOfMatches++;
                    return true;
                }
            }
            return false;
        }

        private static List<Filter> FindFilter(List<Filter> list, Warning warn)
        {
            return list.FindAll(f => f.Number == warn.Number && f.Source == warn.Source);
        }

        private bool Apply(Warning warning)
        {
            if (Enabled == false)
                return false;

            bool valid = true;
            if (arguments.Count != warning.Arguments.Count)
                throw new Exception("Different number of arguments");
            for (int i = 0; i < arguments.Count; i++)
            {
                string better = arguments[i].Replace("*", ".*").Replace(@"\", @"\\");
                Regex regex = new Regex(better);
                var match = regex.Match(warning.Arguments[i]);
                valid &= match.Value == warning.Arguments[i]; // regex should cover all message
                //valid &= regex.IsMatch(warning.Arguments[i]);
            }
            return valid;
        }
    }
}
