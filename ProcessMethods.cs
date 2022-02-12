using net.zemberek.erisim;
using net.zemberek.yapi;
using net.zemberek.tr.yapi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections;

namespace PreProcesses
{
    public static class ProcessMethods
    {
        static Regex rgxUrl = new Regex(@"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)");
        static Regex rgx = new Regex(@"\w+"); //"[^a-zA-Z0-9 -]"     
        
      
        public static string removeURL(this string sentence) 
        {
            var urlMatches = rgxUrl.Matches(sentence);
            var urlCount = 0;
            var tempSentence = sentence;

            if (urlMatches.Count > 0)
            {
                foreach (var urlMatch in urlMatches)
                {
                    if (urlMatches.Count > 1)
                    {
                        sentence = tempSentence.Replace(urlMatch.ToString(), " ");
                        urlCount++;
                        tempSentence = sentence;

                        if (urlMatches.Count == urlCount)
                        {
                            urlCount = 0;
                        }
                    }
                    else
                    {
                        sentence = sentence.Replace(urlMatch.ToString(), " ");
                    }
                }
            }
           

            sentence = sentence.ToLower().Trim();
            return sentence;
        }

        #region Remove Same Words
        public static List<string> removeSameWords(this string sentence) 
        {
            var splitList = RemoveSpecialChars(sentence).ToList();
            var distinctWords = splitList.Distinct().ToList();
            return distinctWords;
        }
        public static List<string> removeSameWords(this string[] wordsArray)
        {
            string[] tempArray = new string[wordsArray.Length];
            for (int i = 0; i < wordsArray.Length; i++)
            {
                tempArray[i] = wordsArray[i].ToLower();
            }
            var distinctWords = tempArray.Distinct().ToList();
            return distinctWords;
        }
        public static List<string> removeSameWordsFromSentences(this string[] sentenceArray) 
        {
            List<string> wordsList = new List<string>();
            for (int i = 0; i < sentenceArray.Length; i++)
            {
                wordsList.AddRange(RemoveSpecialChars(sentenceArray[i]));
            }

            return removeSameWords(wordsList);
        }
        public static List<string> removeSameWords(this List<string> wordsList)
        {
            List<string> tempList = new List<string>();
            foreach (var item in wordsList)
            {
                tempList.Add(item.ToLower());
            }

            List<string> distinctWords = new List<string>();
            distinctWords.AddRange(tempList.Distinct());
            return distinctWords;
        }
        public static List<string> removeSameWordsFromSentences(this List<string> sentenceList)
        {
            List<string> tempList = new List<string>();

            List<string> wordsList = new List<string>();
            for (int i = 0; i < sentenceList.Count; i++)
            {
                wordsList.AddRange(RemoveSpecialChars(sentenceList[i]));
            }

            foreach (var item in wordsList)
            {
                tempList.Add(item.ToLower());
            }

            List<string> distinctWords = new List<string>();
            distinctWords.AddRange(tempList.Distinct());
            return distinctWords;
        }
        #endregion

        #region Remove Unique Words
        public static List<string> removeOnlyOneWords(this string sentence) 
        {
            string[] splitSentence = RemoveSpecialChars(sentence.ToLower());
            List<string> resultList = new List<string>();

            foreach (var word in splitSentence)
            {
                int count = 0;
                for (int i = 0; i < splitSentence.Length; i++)
			    {
			        if (word.Equals(splitSentence[i]))
                    {
                        count++;
                    }
			    }
                if (count>1 && !resultList.Any(p=> p.Equals(word)))
                {
                    resultList.Add(word);
                }
            }

            return resultList;
        }
        public static List<string> removeOnlyOneWords(this string[] wordsArray) 
        {
            List<string> resultList = new List<string>();
            string[] sentenceList = new string[wordsArray.Length];

            for (int i = 0; i < wordsArray.Length; i++)
            {
                sentenceList[i] = wordsArray[i].ToLower();
            }

            foreach (var word in sentenceList)
            {
                int count = 0;
                for (int i = 0; i < sentenceList.Length; i++)
                {
                    if (word.Equals(sentenceList[i]))
                    {
                        count++;
                    }
                }
                if (count > 1 && !resultList.Any(p => p.Equals(word)))
                {
                    resultList.Add(word);
                }
            }

            return resultList;
        }
        public static List<string> removeOnlyOneWordsFromSentenceArray(this string[] sentenceArray) 
        {
            List<string> resultList = new List<string>();
            List<string> wordsList = new List<string>();
            for (int i = 0; i < sentenceArray.Count(); i++)
            {
                wordsList.AddRange(RemoveSpecialChars(sentenceArray[i].ToLower()));
            }

            foreach (var word in wordsList)
            {
                int count = 0;
                for (int i = 0; i < wordsList.Count; i++)
                {
                    if (word.Equals(wordsList[i]))
                    {
                        count++;
                    }
                }
                if (count > 1 && !resultList.Any(p => p.Equals(word)))
                {
                    resultList.Add(word);
                }
            }

            return resultList;
        }
        public static List<string> removeOnlyOneWords(this List<string> wordList)
        {
            List<string> resultList = new List<string>();
            List<string> lowerWordsList = new List<string>();

            foreach (var item in wordList)
            {
                lowerWordsList.Add(item.ToLower());
            }

            foreach (var word in lowerWordsList)
            {
                int count = 0;
                for (int i = 0; i < lowerWordsList.Count; i++)
                {
                    if (word.Equals(lowerWordsList[i]))
                    {
                        count++;
                    }
                }
                if (count > 1 && !resultList.Any(p => p.Equals(word)))
                {
                    resultList.Add(word);
                }
            }

            return resultList;
        }
        public static List<string> removeOnlyOneWordsFromSentenceList(this List<string> sentenceList)
        {
            List<string> resultList = new List<string>();
            List<string> wordsList = new List<string>();
            foreach (var item in sentenceList)
            {
                wordsList.AddRange(RemoveSpecialChars(item.ToLower()));
            }

            foreach (var word in wordsList)
            {
                int count = 0;
                for (int i = 0; i < wordsList.Count; i++)
                {
                    if (word.Equals(wordsList[i]))
                    {
                        count++;
                    }
                }
                if (count > 1 && !resultList.Any(p => p.Equals(word)))
                {
                    resultList.Add(word);
                }
            }

            return resultList;
        }
        #endregion

        #region Morphological Methods
        public static List<string> MorphologicalMethods(this string word) 
        {
            string root = "";
            List<string> rootList = new List<string>();
            Zemberek zemberek = new Zemberek(new TurkiyeTurkcesi());
            if (!zemberek.kelimeDenetle(word.ToLower())) 
            {
                Console.WriteLine("Kelime hatalı!");
            }
            Kelime[] solutions = zemberek.kelimeCozumle(word.ToLower());
            foreach (var item in solutions)
            {
                Console.WriteLine(item);
            }
            
            //String kok = zemberek.dilBilgisi().kokler().kokBul(word).ToString();
            //Kok kok = (Kok)zemberek.dilBilgisi().kokler().kokBul("word")[0];
            IList<IList<string>> ayrisimlar = zemberek.kelimeAyristir(word);

            
            //var x= solutions[0].kok().tip().ToString();

                foreach (var item in solutions)
                {
                    rootList.Add(item.kok().icerik().ToString());
                }

            
            //foreach (IList<String> result in ayrisimlar)
            //{
            //    System.Console.Write("[");
            //    foreach (String str in result)
            //    {
            //        System.Console.Write(str + "-");
            //    }
            //    System.Console.WriteLine("]");
            //}
            return rootList;
        }
        public static List<string> MorphologicalMethods(this string[] wordsArray) 
        {
            string[] tempArray = new string[wordsArray.Length];
            List<string> result = new List<string>();
            for (int i = 0; i < wordsArray.Length; i++)
            {
                tempArray[i] = wordsArray[i].ToLower();
            }

            foreach (var item in tempArray)
            {
                result.AddRange(MorphologicalMethods(item));
            }
            return result.Distinct().ToList();
        }
        public static List<string> MorphologicalMethods(this List<string> wordList)
        {
            string[] tempArray = new string[wordList.Count];
            List<string> result = new List<string>();
            for (int i = 0; i < wordList.Count; i++)
            {
                tempArray[i] = wordList[i].ToLower();
            }
            foreach (var item in tempArray)
            {
                result.AddRange(MorphologicalMethods(item));
            }
            return result.Distinct().ToList();
        }
        public static List<string> MorphologicalMethodsForSentence(this string sentence) 
        {
            var splitList = removeSameWordsFromSentences(RemoveSpecialChars(removeURL(sentence).ToString()).ToList());
            List<string> result = new List<string>();
            result.AddRange(MorphologicalMethods(splitList));
            return result.Distinct().ToList();
        }
        public static List<string> MorphologicalMethodsForSentence(this string[] sentence)
        {
            List<string> splitList = new List<string>();
            for (int i = 0; i < sentence.Length; i++)
            {
                splitList.AddRange(removeSameWordsFromSentences(RemoveSpecialChars(removeURL(sentence[i]).ToString()).ToList()));
            }
            //var splitList = removeSameWordsFromSentences(removeSpecialChars(removeURL(sentence).ToString()).ToList());
            List<string> result = new List<string>();
            result.AddRange(MorphologicalMethods(splitList));
            return result.Distinct().ToList();
        }
        public static List<string> MorphologicalMethodsForSentence(this List<string> sentence)
        {
            List<string> splitList = new List<string>();
            for (int i = 0; i < sentence.Count; i++)
            {
                splitList.AddRange(removeSameWordsFromSentences(RemoveSpecialChars(removeURL(sentence[i]).ToString()).ToList()));
            }
            //var splitList = removeSameWordsFromSentences(removeSpecialChars(removeURL(sentence).ToString()).ToList());
            List<string> result = new List<string>();
            result.AddRange(MorphologicalMethods(splitList));
            return result.Distinct().ToList();
        }
        #endregion

        #region Remove Digits
        public static List<string> removeDigit(this string sentence) 
        {
            List<string> result = new List<string>();
            var allMatches = rgx.Matches(sentence.ToLower());
            string[] words = new string[allMatches.Count];
            for (int i = 0; i < allMatches.Count; i++)
            {
                words[i] = allMatches[i].ToString().ToLower();
            }

            result = words.Where(p => p.Any(c => !Char.IsDigit(c))).ToList();
            return result;
        }

        public static List<string> removeDigit(this string[] sentenceArray)
        {
            List<string> result = new List<string>();
            //List<IEnumerable> allMatches = new List<IEnumerable>();


            List<string> words = new List<string>();
            for (int i = 0; i < sentenceArray.Length; i++)
            {
                var allMatches = rgx.Matches(sentenceArray[i].ToLower());
                for (int j = 0; j < allMatches.Count; j++)
                {
                    words[j] = allMatches[j].ToString().ToLower();
                }
                //words[i] = allMatches[i].ToString().ToLower();
            }

            result = words.Where(p => p.Any(c => !Char.IsDigit(c))).ToList();
            return result;
        }

        #endregion

    }
}
