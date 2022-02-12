using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ML.WordNetLibrary.NGramExtensions;
namespace ML.WordNetLibrary.ExtractionExtensions
{
    public static class RegexExtensions
    {
        private const string EMOTION_PATTERN = @"\S{2,5}";
        private const string URL_PATTERN = @"(http|ftp|sftp|ssh)\S+";
        private const string EMAIL_PATTERN = @"(\S*@\S*)";
        private const string NUMERIC_PATTERN = @"([\.,]{0,1}\d+)+";
        private const string WORD_PATTERN = @"[\w]+";
        
        private static Regex regexNumericExtractor = new Regex(RegexExtensions.NUMERIC_PATTERN, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
        private static Regex regexEmotionExtractor = new Regex(RegexExtensions.EMOTION_PATTERN, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
        private static Regex regexUrl_PatternExtractor = new Regex(RegexExtensions.URL_PATTERN, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
        private static Regex regexEmailExtractor = new Regex(RegexExtensions.EMAIL_PATTERN, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
        private static Regex regexWordExtractor = new Regex(RegexExtensions.WORD_PATTERN, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        private static IEnumerable<string> extract(Regex rx,string text)
        {
            foreach (Match m in rx.Matches(text))
                yield return m.Groups[0].Value.Trim();
        }
        public static IEnumerable<string> ExtractEmotions(this string text)
        {
            var emotions= extract(regexEmotionExtractor, text).Where(emotion=>emotion.IsInWordsDictionary(WordDictionaryType.Emotions));
            return emotions;
        }
        public static string SubstractURLs(this string text)
        {
            return regexUrl_PatternExtractor.Replace(text, "");
        }
        public static string SubstractNumerics(this string text)
        {
            return regexNumericExtractor.Replace(text, "");
        }
        public static string SubstractEmails(this string text)
        {
            return regexEmailExtractor.Replace(text, "");
        }
      
        public static IEnumerable<string> ExtractWords(this string text)
        {
            var cleanOneGrams = extract(regexWordExtractor, text.SubstractURLs().SubstractEmails().SubstractNumerics());
            var expressionReplaced = cleanOneGrams.ReplaceWithDefinedExpressions();
            return expressionReplaced;
           
        }

       
    }

}
