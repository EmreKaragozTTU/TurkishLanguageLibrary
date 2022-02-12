using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections;
using ML.WordNetLibrary.ExtractionExtensions;
namespace ML.WordNetLibrary.NGramExtensions
{
    public static class NGramExtensions
    {
        public static IEnumerable<string> ToOneGrams(this string sentence)
        {
            var totalOneGrams= sentence.ExtractWords()
                .Concat(sentence.ExtractEmotions());
            return totalOneGrams;
                
        }
        
        public static IEnumerable<string> ToBiGrams(this string sentence)
        {
            return ToBiGrams(sentence.ToOneGrams());
        }
        public static IEnumerable<string> ToBiGrams(this IEnumerable<string> words)
        {
            var parts = words.ToArray();
            for (int i = 1; i < parts.Length; i++)
            {
                yield return parts[i - 1] + " " + parts[i];
            }
        }

        public static IEnumerable<string> ToTreeGrams(this string sentence)
        {
            return ToTreeGrams(sentence.ToOneGrams());
        }
        public static IEnumerable<string> ToTreeGrams(this IEnumerable<string> words)
        {
            var parts = words.ToArray();
            for (int i = 2; i < parts.Length; i++)
            {
                yield return parts[i - 2] + " " + parts[i - 1] + " " + parts[i];
            }
        }
       


    }
}
