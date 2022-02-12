using net.zemberek.erisim;
using net.zemberek.islemler.cozumleme;
using net.zemberek.tr.yapi;
using net.zemberek.yapi;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ML.WordNetLibrary
{
    public static class TurkishLanguage
    {
        private static CultureInfo trCulture = new CultureInfo("tr-TR");
        private static Zemberek zemberek = new Zemberek(new TurkiyeTurkcesi());
        private static char[] commaSplitterChars = new char[] { ',',';',':' };
        private static char[] spaceSplitterChars = new char[] { ' ', '\t', '\n' };
        private static  Dictionary<string,WordPattern> _wordPatterns = new Dictionary<string,WordPattern>();

        private static HashSet<string> meaningChangerAffixes = new HashSet<string>();
        static TurkishLanguage()
        {
            meaningChangerAffixes.Add("ISIM_KOK");
            meaningChangerAffixes.Add("FIIL_KOK");
            meaningChangerAffixes.Add("SAYI_KOK");
            meaningChangerAffixes.Add("ZAMAN_KOK");
            meaningChangerAffixes.Add("ZAMIR_KOK");
            meaningChangerAffixes.Add("GENEL_KOK");
            meaningChangerAffixes.Add("UNLEM_KOK");
            meaningChangerAffixes.Add("EDAT_KOK");
            meaningChangerAffixes.Add("BAGLAC_KOK");
            meaningChangerAffixes.Add("OZEL_KOK");
            meaningChangerAffixes.Add("IMEK_KOK");
            meaningChangerAffixes.Add("YANKI_KOK");
            meaningChangerAffixes.Add("SORU_KOK");
            meaningChangerAffixes.Add("ISIM_YOKLUK_SIZ");
            meaningChangerAffixes.Add("FIIL_OLUMSUZLUK_DEN");
            meaningChangerAffixes.Add("FIIL_OLUMSUZLUK_ME");
            meaningChangerAffixes.Add("FIIL_OLUMSUZLUK_SIZIN");




        }
        private static bool checkMeaningsSame(Kelime word)
        {
            return !meaningChangerAffixes.Contains(word.sonEk().ad());
        }
        
        private static string findSingleWordRoot(string word,Kelime kelime)
        {
            if (string.IsNullOrEmpty(word) || kelime == null) return word;
            var firstRoot = zemberek.cozumleyici().cozumle(word).FirstOrDefault();
            if (firstRoot == null) return word;
            if (checkMeaningsSame(firstRoot))
                return findSingleWordRoot(firstRoot.kok().icerik(), firstRoot);
            return word;
        }

       
      
        public static string ToRootWord(this string  word)
        {
            
            var firstRoot = zemberek.cozumleyici().cozumle(word).FirstOrDefault();
            return findSingleWordRoot(word, firstRoot);

        }
        public static string ToWordGroupRoot(this string word)
        {
           
            StringBuilder sb = new StringBuilder();
            foreach (var subWord in word.SplitWithSpace())
            {
                var firstRoot = zemberek.cozumleyici().cozumle(subWord).FirstOrDefault();
                sb.Append(findSingleWordRoot(subWord, firstRoot));
                sb.Append(" ");
            }
            return sb.ToString().Trim();


        }

        public static void IncludeWordPattern(string semanticName,string pattern)
        {
           if(semanticName==null || pattern==null) return;
           var semanticNode = semanticName.ToSemanticNode();
           if (!_wordPatterns.ContainsKey(semanticName))
               _wordPatterns.Add(semanticNode.NodeName, new WordPattern(semanticNode.NodeName));
           var wordPattern = _wordPatterns[semanticNode.NodeName];
           wordPattern.AddPattern(pattern);
        }

        public static string ToASCIISolvedWord(this string word)
        {
            var foundPossibleWords = zemberek.asciidenTurkceye(word);
            if (foundPossibleWords == null || foundPossibleWords.Length == 0) return word;
            if (foundPossibleWords.Contains(word)) return word;
            return foundPossibleWords[0];
            
        }
        public static string FindWordFromWordPatterns(this string word)
        {
            foreach(var item in _wordPatterns)
            {

                if (item.Value.IsMatch(word))
                    return item.Key;
            }
            return string.Empty;

        }
        internal static  bool IsMorphologicalValidWord(this string word)
        {

            foreach (var subWord in word.Split(commaSplitterChars, StringSplitOptions.RemoveEmptyEntries))
                if (!zemberek.kelimeDenetle(subWord))
                    return false;

            return true;
        }

        internal static IEnumerable<string> SplitWithComma(this string wordsByCommaSeperated)
        {
            return wordsByCommaSeperated.Split(commaSplitterChars, StringSplitOptions.RemoveEmptyEntries).Select(word=>word.Trim());
                
        }
        internal static IEnumerable<string> SplitWithSpace(this string wordsBySpaceSeperated)
        {
            return wordsBySpaceSeperated.Split(spaceSplitterChars, StringSplitOptions.RemoveEmptyEntries);

        }
        public static string ToTurkishLower(this string text)
        {
            return text.Trim().ToLower(trCulture);
        }
        public static IEnumerable<string> ToTurkishLower(this IEnumerable<string> wordList)
        {
            foreach (var word in wordList)
                yield return word.ToTurkishLower();
        }

        public static string ToTurkishUpper(this string text)
        {
            return text.ToUpper(trCulture);
        }
       
    }
}
