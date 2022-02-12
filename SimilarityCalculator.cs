using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimilarWordsGenerationAlgorithmLibrary
{
    public static class SimilarityCalculator 
    {

      
      
      


        private static readonly HashSet<char> alphabet = new HashSet<char>() 
            { 'a', 'b', 'c', 'ç', 'd', 'e', 'f', 'g', 'ğ', 'h', 'ı', 'i', 'j',
              'k', 'l', 'm', 'n', 'o', 'ö', 'p', 'r', 's', 'ş', 't', 'u', 'ü', 
              'v', 'y', 'z'
            };

        private static readonly HashSet<char> vowellst = new HashSet<char>() 
            { 'a', 'e','ı', 'i', 'o','ö', 'u','ü' 
            };

        private static readonly HashSet<char> softLst = new HashSet<char>() { 'b', 'c', 'd', 'g',  'j', 'l', 'm', 'n', 'r', 'v', 'y', 'z' };
        private static readonly HashSet<char> hardLst = new HashSet<char>() {  'f', 'h', 'k', 'p', 's', 't' };
        private static readonly HashSet<char> expoLst = new HashSet<char>() { 'c', 'g', 'k', 'p', 't', 'q' };
        private static readonly HashSet<char> creepLst = new HashSet<char>() { 'j', 's', 'z' };
        private static readonly HashSet<char> simLst = new HashSet<char>() { 'l', 'i' };

        public static string ReplaceChar(this string input,char newChar,int index)
        {
            input = input.Remove(index, 1);
            return input.Insert(index, newChar.ToString());
            //return input.ReplaceString(newChar.ToString(), index);

        }
        public static string ReplaceString(this string input, string newString, int index)
        {
            if (index < 0)
                throw new Exception("index sıfırdan küçük olamaz.");
            if (index > input.Length - newString.Length)
                throw new Exception("index input uzunluğu yeni stringin uzunlugunun eksiğinden fazla olamaz");

            string baslangic = index > 0 ? input.Substring(0, index - 1) : "";
            string son = index < input.Length - newString.Length ? input.Substring(index + newString.Length-1) : "";
            return baslangic + newString + son;

        }
        public static string InsertString(this string input, string insertStr, int index)
        {
            if (index < 0)
                throw new Exception("index sıfırdan küçük olamaz.");
            if (index > input.Length )
                throw new Exception("index input uzunluğundan fazla olamaz");

            //string baslangic = index > 0 ? input.Substring(0, index - 1) : "";
            string baslangic = index > 0 ? input.Substring(0, index) : "";
            string son = index < input.Length  ? input.Substring(index ) : "";
            return baslangic + insertStr + son;

        }
        #region One Operations

        //OK
        public static IEnumerable<Similarity> GetOneReplace(this string input)
        {

            for (int i = 0; i < input.Length; ++i)
            {
                foreach (var item in alphabet)
                {
                    var similarWord = input.ReplaceChar(item, i);
                
                    yield return new Similarity()
                    {
                        Word=similarWord,
                        Value=GramOperations.GetWordSimilarityValue(input,similarWord),
                        Method=SimilarityMethods.OneReplace

                    };
                }
            }
           
        }

        //OK
        public static IEnumerable<Similarity> GetOneVowelsReplace(this string input)
        {

            for (int i = 0; i < input.Length; ++i)
            {
                //if (!vowellst.Contains(input[i])) continue;
                if (vowellst.Contains(input[i])) continue;
                foreach (var item in vowellst)
                {
                    var similarWord = input.ReplaceChar(item, i);

                    yield return new Similarity()
                    {
                        Word = similarWord,
                        Value = GramOperations.GetWordSimilarityValue(input, similarWord),
                        Method = SimilarityMethods.OneVowelReplace

                    };
                }
            }


           
        }

        public static IEnumerable<Similarity> GetOneSemiVowelsReplace(this string input)
        {

            for (int i = 0; i < input.Length; ++i)
            {
             
                if (softLst.Contains(input[i]))
                {
                    foreach (var item in softLst)
                    {
                        var similarWord = input.ReplaceChar(item, i);

                        yield return new Similarity()
                        {
                            Word = similarWord,
                            Value = GramOperations.GetWordSimilarityValue(input, similarWord),
                            Method = SimilarityMethods.OneSemiVowelsReplace

                        };
                    }
                }
                else if (hardLst.Contains(input[i]))
                {
                    foreach (var item in hardLst)
                    {
                        var similarWord = input.ReplaceChar(item, i);

                        yield return new Similarity()
                        {
                            Word = similarWord,
                            Value = GramOperations.GetWordSimilarityValue(input, similarWord),
                            Method = SimilarityMethods.OneSemiVowelsReplace

                        };
                    }
                }
            }
           
        }

        public static IEnumerable<Similarity> GetOneRandomReplace(this string input)
        {
            for (int i = 0; i < input.Length; ++i)
            {

                if (expoLst.Contains(input[i]))
                {
                    foreach (var item in expoLst)
                    {
                        var similarWord = input.ReplaceChar(item, i);

                        yield return new Similarity()
                        {
                            Word = similarWord,
                            Value = GramOperations.GetWordSimilarityValue(input, similarWord),
                            Method = SimilarityMethods.OneRandomReplace

                        };
                    }
                }
                else if (creepLst.Contains(input[i]))
                {
                    foreach (var item in creepLst)
                    {
                        var similarWord = input.ReplaceChar(item, i);

                        yield return new Similarity()
                        {
                            Word = similarWord,
                            Value = GramOperations.GetWordSimilarityValue(input, similarWord),
                            Method = SimilarityMethods.OneRandomReplace

                        };
                    }
                }
                else if (simLst.Contains(input[i]))
                {
                    foreach (var item in simLst)
                    {
                        var similarWord = input.ReplaceChar(item, i);

                        yield return new Similarity()
                        {
                            Word = similarWord,
                            Value = GramOperations.GetWordSimilarityValue(input, similarWord),
                            Method = SimilarityMethods.OneRandomReplace

                        };
                    }
                }
            }
          

           
        }

        //OK
        public static IEnumerable<Similarity> GetOneInsert(this string input)
        {

            for (int i = 0; i <= input.Length; ++i)
            {
                foreach (var item in alphabet)
                {
                    var similarWord = input.InsertString(item.ToString(), i);

                    yield return new Similarity()
                    {
                        Word = similarWord,
                        Value = GramOperations.GetWordSimilarityValue(input, similarWord),
                        Method = SimilarityMethods.OneInsert

                    };
                }

            }

        }

        //OK
        public static IEnumerable<Similarity> GetOneVowelsInsert(this string input)
        {

            for (int i = 0; i < input.Length; ++i)
            {
                if (!vowellst.Contains(input[i])) continue;
                foreach (var item in vowellst)
                {
                    var similarWord = input.InsertString(item.ToString(), i);

                    yield return new Similarity()
                    {
                        Word = similarWord,
                        Value = GramOperations.GetWordSimilarityValue(input, similarWord),
                        Method = SimilarityMethods.OneVowelsInsert

                    };
                }

               
            }
        }

        //OK
        public static IEnumerable<Similarity> GetOneDelete(this string input)
        {
            for (int index = 0; index < input.Length; ++index)
            {
                
                string baslangic = index > 0 ? input.Substring(0, index - 1) : "";
                string son = index < input.Length ? input.Substring(index) : "";
                string similarWord = baslangic + son;
                yield return new Similarity()
                {
                    Word=similarWord,
                    Value=GramOperations.GetWordSimilarityValue(input,similarWord),
                    Method=SimilarityMethods.OneDelete
                };
            }
        }

        //OK
        public static IEnumerable<Similarity> GetOneSwap(this string input)
        {
           

            for (int i = 0; i < input.Length-1; ++i)
            {
                for (int j = (i + 1); j < input.Length; ++j)
                {
                   string similarWord=  input.ReplaceChar(input[j],i).ReplaceChar(input[i],j);
                   yield return new Similarity()
                   {
                       Word = similarWord,
                       Value = GramOperations.GetWordSimilarityValue(input, similarWord),
                       Method = SimilarityMethods.OneSwap
                   };
                }

            }

           
        }

        #endregion

        #region Two Operations

     

        //OK
        private static IEnumerable<string> getAlphabetTwoCombinations()
        {
            var str = alphabet.Select(x => x.ToString());
            return str.SelectMany(x => alphabet, (x, y) => x + y);

         
        }

        //OK
        private static IEnumerable<string> getTwoVowelVariations()
        {
            var str = vowellst.Select(x => x.ToString());
            return str.SelectMany(x => vowellst, (x, y) => x + y);
         
        }

        //OK
        public static IEnumerable<Similarity> GetTwoReplace(this string input)
        {
           
            
            //for(int i=0;i<input.Length-1;i++)
            for (int i = 1; i < input.Length - 1; i++)
            {
                foreach(var item in getAlphabetTwoCombinations())
                {
                    var similarWord = input.ReplaceString(item, i);
                    yield return new Similarity()
                    {
                        Word = similarWord,
                        Value = GramOperations.GetWordSimilarityValue(input, similarWord),
                        Method = SimilarityMethods.TwoReplace

                    };

                    

                }
            }
          
        }

        //OK
        public static IEnumerable<Similarity> GetTwoInsert(this string input)
        {
            for (int i = 0; i <= input.Length; ++i)
            {
                foreach (var item in getAlphabetTwoCombinations())
                {
                    var similarWord = input.InsertString(item.ToString(), i);
                    yield return new Similarity()
                    {
                        Word = similarWord,
                        Value = GramOperations.GetWordSimilarityValue(input, similarWord),
                        Method = SimilarityMethods.TwoInsert

                    };
                 

                }
            }
        }

        public static IEnumerable<Similarity> GetTwoVowelInsert(this string input)
        {
            for (int i = 0; i <= input.Length; ++i)
            {
                foreach (var item in getTwoVowelVariations())
                {
                    var similarWord = input.InsertString(item.ToString(), i);
                    yield return new Similarity()
                    {
                        Word = similarWord,
                        Value = GramOperations.GetWordSimilarityValue(input, similarWord),
                        Method = SimilarityMethods.TwoVowelInsert

                    };

                }
            }
        }

        //OK
        public static IEnumerable<Similarity> GetTwoDelete(this string input)
        {
            for (int index = 0; index < input.Length; ++index)
            {

                string baslangic = index > 0 ? input.Substring(0, index - 1) : "";
                string son = index < input.Length-1 ? input.Substring(index+2) : "";
                string similarWord=  baslangic + son;
                yield return new Similarity()
                {
                    Word = similarWord,
                    Value = GramOperations.GetWordSimilarityValue(input, similarWord),
                    Method = SimilarityMethods.TwoDelete

                };
            }
        }

        public static  IEnumerable<Similarity> GetTwoSwap(this string input)
        {
            for (int i = 0; i < input.Length; ++i)
            {
                for (int j = (i + 1); j < input.Length-1; ++j)
                {
                   string similarWord= input.ReplaceString(input.Substring(j, 2), i).ReplaceString(input.Substring(i, 2), j);
                   yield return new Similarity()
                   {
                       Word = similarWord,
                       Value = GramOperations.GetWordSimilarityValue(input, similarWord),
                       Method = SimilarityMethods.TwoVowelInsert

                   };
                }

            }


        }

        #endregion
        public static bool IsVovel(this char input)
        {
            return vowellst.Contains(input);
        }
        private static bool HasTogetherSemiVovelChar(this string input,int charCount)
        {
            int semivowelscnt = 0;
            
            foreach (var item in input)
            {
                if (item.IsVovel())
                {
                    semivowelscnt = 0;
                }
                else
                {
                    if (++semivowelscnt >= charCount) return true;
                }
            }
            return false;
        }
        private static int SemiVovelCharNumbers(string input)
        {
            int semivowelscnt = 0;
            int temp = 0;
            foreach (var item in input)
            {
                if (item.IsVovel())
                {
                    semivowelscnt = 0;
                }
                else
                {
                    ++semivowelscnt;
                    if (semivowelscnt>temp)
                    {
                        temp = semivowelscnt;
                    }
                }
            }
            return temp;
        }

        public static IEnumerable<Similarity> GetOneCopyBoost(this string input)
        {
            //for (int i = 0; i <= input.Length; ++i)
            //{
            int i = 0;
            foreach (var item in input)
            {

                string similarWord = input.InsertString(item.ToString(), i);
                if (similarWord == "")
                    yield return null;
                yield return new Similarity()
                {
                    Word = similarWord,
                    Value = GramOperations.GetWordSimilarityValue(input, similarWord),
                    Method = SimilarityMethods.OneCopyBoost

                };
                i++;

            }
            //}
        }

        public static IEnumerable<Similarity> GetAllSimilarity(this string word)
        {
            //string capWord = word;

            word = word.ToLower();
            string[] patternItems = word.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int numberOfUnionSemiVovel = SemiVovelCharNumbers(word.ToLower().Replace(" ", string.Empty));
            string trimmedWord = string.Join(" ", patternItems).Trim();

            if (patternItems.Length > 1 && AbbreviationDictionary.GetAbbrevation(word) != null)
            {
                foreach (var item in AbbreviationDictionary.GetAbbrevation(word).GetAllSimilarity())
                {
                    yield return item;
                }
            }
            string pattern = trimmedWord.Replace(" ", "");
            
                var firstChar = pattern[0];
                foreach (var item in
                        pattern.GetOneCopyBoost()
                       .Concat(pattern.GetOneCopyBoost())
                       .Concat(pattern.GetOneDelete())
                       .Concat(pattern.GetOneRandomReplace())
                       .Concat(pattern.GetOneReplace())
                       .Concat(pattern.GetOneSemiVowelsReplace())
                       .Concat(pattern.GetOneSwap())
                       .Concat(pattern.GetOneInsert())
                       .Concat(pattern.GetOneVowelsInsert())
                       .Concat(pattern.GetOneVowelsReplace())
                       .Concat(pattern.GetTwoDelete())
                       .Concat(pattern.GetTwoInsert())
                       .Concat(pattern.GetTwoReplace())
                       .Concat(pattern.GetTwoSwap())
                       .Concat(pattern.GetTwoVowelInsert())
                       .Distinct()
                       .Where(p => 
                               !string.IsNullOrEmpty(p.Word) && p.Word[0] == firstChar && !p.Word.HasTogetherSemiVovelChar(numberOfUnionSemiVovel + 1)
                       )
                       .GroupBy(p=>p.Word).Select(p=> new Similarity()
                       {
                           Word=p.Key,
                           Method= (SimilarityMethods) (ulong) p.Sum(c=> (int)c.Method),
                           Value=p.First().Value
                       })
                       .OrderByDescending(p=>p.BoostedValue)
                     )
                {
                    yield return item;
                }
        }

        public static IEnumerable<Similarity> Normalize(this List<Similarity> list)
        {
            var diffItems = list.Select((s, i) =>
            {
                if (i != list.Count() - 1)
                {
                    var nextItem = list[i+1];

                    var diff = s.BoostedValue > 0 ? s.BoostedValue - nextItem.BoostedValue : s.ManuelBoostedValue - nextItem.ManuelBoostedValue;

                    return new { Word = s.Word, Fark = diff , Index = i};
                }
                else
                {
                    return new { Word = s.Word, Fark = 0.0 , Index = i };
                }

            });

            list = list.Select(x =>
            {
                x.ManuelBoostedValue = x.ManuelBoostedValue == 0 ? x.BoostedValue : x.ManuelBoostedValue;
                return x;
            }).ToList();

            var maxDiffItems = diffItems.Where(x => x.Fark > 1).ToList();

            foreach (var maxItem in maxDiffItems)
            {
                for (int i = 0; i < maxItem.Index + 1; i++)
                {
                    Similarity listItem = list[i];
                    listItem.ManuelBoostedValue = listItem.ManuelBoostedValue - maxItem.Fark + 0.8;
                    list[i] = listItem;
                }
            }
            

            return list;
        }

        public static IEnumerable<Similarity> Filter(this List<Similarity> list,double percentage)
        {
            var maxValue = list.GetRange(1, list.Count() - 1).Max(c => c.ManuelBoostedValue);
            var thresholdValue = maxValue * percentage / 100;
            return list.Where(x => x.ManuelBoostedValue > thresholdValue).ToList(); ;
        }
       

       

    }
}
