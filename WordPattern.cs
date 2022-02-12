using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ML.WordNetLibrary
{
    internal class WordPattern
    {
        internal HashSet<string> Patterns { get; private set; }
        private  HashSet<Regex> _regexPatterns { get; set; }
        public WordPattern(string word)
        {
            this.Word = word;
            this.Patterns = new HashSet<string>();
            this._regexPatterns = new HashSet<Regex>();
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public string Word { get; private set; }
        public void AddPattern(string pattern)
        {
            if (this.Patterns.Contains(pattern)) return;
            this.Patterns.Add(pattern);
            this._regexPatterns.Add(new Regex(pattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Compiled));

        }


        public bool IsMatch(string word)
        {
            foreach(var rx in this._regexPatterns)
            {
                if (rx.IsMatch(word))
                    return true;

            }
            return false;
        }
    }
}
