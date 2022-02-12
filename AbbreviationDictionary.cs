using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimilarWordsGenerationAlgorithmLibrary
{
    public static class AbbreviationDictionary
    {
        private static Dictionary<string, string> _abbrevations = null;
        private static void ensureLoaded()
        {
            if (_abbrevations != null) return;
            var rawtxts = File.ReadAllLines("kisaltmalar.txt");
            _abbrevations = new Dictionary<string, string>();
            for (int i = 0; i < rawtxts.Length - 1; i = i + 2)
            {
                if (_abbrevations.ContainsKey(rawtxts[i + 1])) continue;
                _abbrevations.Add(rawtxts[i + 1], rawtxts[i]);
                
            }
        }
        public static string GetAbbrevation(string word)
        {
            ensureLoaded();
            return _abbrevations.ContainsKey(word) ? _abbrevations[word] : null;
        }
    }
}
