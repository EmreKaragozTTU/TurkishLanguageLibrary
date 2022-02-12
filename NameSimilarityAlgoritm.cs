using SimilarWordsGenerationAlgorithmLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateSimilarityNameAlgoritm
{
    public class NameSimilarityAlgoritm
    {
        public static List<char> vowels = new List<char>() { 'A', 'a', 'E', 'e', 'I', 'ı', 'İ', 'i', 'O', 'o', 'Ö', 'ö', 'U', 'u', 'Ü', 'ü' };

        public static List<Similarity> GenerateNameSimilarities(string name,int eşikDeğeri)
        {
          

            List<List<Similarity>> combinedList = new List<List<Similarity>>();
            List<Similarity> oneletterList = new List<Similarity>();
            List<String> splitList = new List<String>();

            string[] patternItems = name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            List<Similarity> lastList = new List<Similarity>();

            for (int i = 0; i < patternItems.Count(); i++)
            {
                var foundList = patternItems[i].GetAllSimilarity().ToList().Normalize().ToList() ;
                foundList = foundList.Filter(eşikDeğeri).ToList();

                if (foundList.Where(x => x.Word.ToLower() == patternItems[i].ToLower()).FirstOrDefault() == null)
                    foundList.Add(new Similarity() { Word = patternItems[i], ManuelBoostedValue = foundList.Max(x => x.ManuelBoostedValue) + 1 });
                combinedList.Add(foundList);

                combinedList[i].AddRange(GetFirstLetters(patternItems[i], foundList));

            }

            lastList.AddRange(GetAllCombinationsLists(combinedList.ToArray()));

            var allCombinedList = GetCombinations(name, lastList);
            lastList.AddRange(allCombinedList);

            lastList = lastList.Distinct().ToList();
            lastList = lastList.OrderByDescending(x => x.ManuelBoostedValue).ToList();


            for (int i = 0; i < lastList.Count; i++)
            {
                splitList = lastList[i].Word.Split(' ').ToList();
                int letterCount = 0;
                foreach (var item in splitList)
                {
                    if (item.Length == 1)
                    {
                        letterCount++;
                    }
                }

                if (letterCount >= 2)
                {
                    lastList[i].ManuelBoostedValue -= lastList[i].ManuelBoostedValue * 35 / 100;
                }
            }
            lastList.Where(x => x.Word.ToLower() == name.ToLower()).First().ManuelBoostedValue = lastList.Max(x => x.ManuelBoostedValue) + 1;
            lastList = lastList.OrderByDescending(x => x.ManuelBoostedValue).ToList();

            return lastList;
        }

        private static List<Similarity> GetAllCombinationsLists(params List<Similarity>[] listeler)
        {
            List<Similarity> foundList = new List<Similarity>();

            var allCombinations = AllCombinationsOf(listeler);

            for (int i = 0; i < allCombinations.Count(); i++)
            {
                Similarity value = new Similarity();
                value.Word = string.Join(" ", allCombinations[i].Select(x => x.Word).ToList());
                for (int j = 0; j < allCombinations[i].Count(); j++)
                {
                    if (allCombinations[i][j].ManuelBoostedValue == 0)
                    {
                        value.ManuelBoostedValue += allCombinations[i][j].BoostedValue;
                    }
                    else
                    {
                        value.ManuelBoostedValue += allCombinations[i][j].ManuelBoostedValue;
                    }
                }
                //value.ManuelBoostedValue = allCombinations[i][i].BoostedValue
                int tekliHarfSayısı = allCombinations[i].Where(x=>x.Word.Length==1).Count();
                int kelimeSayısı = allCombinations[i].Count();

                if (tekliHarfSayısı != kelimeSayısı)
                    foundList.Add(value);
            }
            return foundList;
        }
        private static List<List<T>> AllCombinationsOf<T>(params List<T>[] sets)
        {
            // need array bounds checking etc for production
            var combinations = new List<List<T>>();

            // prime the data
            foreach (var value in sets[0])
                combinations.Add(new List<T> { value });

            foreach (var set in sets.Skip(1))
                combinations = AddExtraSet(combinations, set);

            return combinations;
        }
        private static List<List<T>> AddExtraSet<T>(List<List<T>> combinations, List<T> set)
        {
            var newCombinations = from value in set
                                  from combination in combinations
                                  select new List<T>(combination) { value };

            return newCombinations.ToList();
        }

        private static List<Similarity> GetCombinations(string input, List<Similarity> foundList)
        {
            List<int> indexler = new List<int>();

            for (int i = 0; i < input.Length; i++)
            {
                if (vowels.Contains(input[i]))
                {
                    indexler.Add(i);
                }
            }
            var resultList = new List<Similarity>();

            var AllCombinations = GetPermutations(indexler).ToList();

            for (int i = 0; i < AllCombinations.Count(); i++)
            {
                var combination = AllCombinations[i];
                string geçiciKelime = input;
                for (int j = 0; j < combination.Count(); j++)
                {
                    var karakterDizisi = geçiciKelime.ToList();
                    if (combination[j] != 0)
                    {
                        if (input[combination[j] - 1] != ' ')
                        {
                            karakterDizisi[combination[j]] = '?';
                            geçiciKelime = string.Join("", karakterDizisi);
                            if (resultList.Where(x => x.Word.Contains(geçiciKelime.Replace("?", ""))).Count() == 0)
                                resultList.Add(new Similarity() { Word = geçiciKelime.Replace("?", "") });

                        }
                    }
                }
            }

            List<Similarity> finalList = new List<Similarity>();
            finalList = resultList.OrderByDescending(x => x.Word.ToString().Length).ToList();

            for (int i = finalList.Count - 1, j = 1; i >= 0; i--, j++)
            {


                finalList[i].ManuelBoostedValue = foundList.Max(x => x.ManuelBoostedValue) + 0.1 * j;

            }

            return finalList;
        }
        private static IEnumerable<IEnumerable<T>> GetPowerSet<T>(IList<T> list)
        {
            return from m in Enumerable.Range(0, 1 << list.Count)
                   select
                       from i in Enumerable.Range(0, list.Count)
                       where (m & (1 << i)) != 0
                       select list[i];
        }
        private static IEnumerable<List<int>> GetPermutations(IList<int> strings)
        {
            List<List<int>> foundList = new List<List<int>>();
            foreach (var s in GetPowerSet(strings))
            {
                foundList.Add(s.ToList());
            }

            return foundList;
        }

        private static IEnumerable<Similarity> GetFirstLetters(string word, List<Similarity> BoostList)
        {
            List<Similarity> foundList = new List<Similarity>();
            Similarity newWord = new Similarity();
            Similarity newWordWithPoint = new Similarity();

            newWord.Word = word.Substring(0, 1);
            newWord.ManuelBoostedValue = BoostList.Max(x => x.BoostedValue) * 2;
            foundList.Add(newWord);

            return foundList;
        }

    }
}
