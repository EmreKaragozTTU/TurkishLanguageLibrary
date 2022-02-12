using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimilarWordsGenerationAlgorithmLibrary
{
  
    public static class GramOperations
    {
        [ThreadStatic]
        private static Dictionary<string, double> _wordFrequences;
        private static void ensureBiGramsLoaded()
        {
            if (_wordFrequences != null) return;
            _wordFrequences = new Dictionary<string, double>();

            var biGrams = File.ReadAllLines("biGram.txt");
            foreach (var line in biGrams)
            {
                var parts = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                string name = parts[0];
                int cnt = Convert.ToInt32(parts[1]);
                double freq = Convert.ToDouble(parts[2]);
                if (!_wordFrequences.ContainsKey(name))
                    _wordFrequences.Add(name, freq);
            }
        }
       


        public static double GetWordSimilarityValue(string keyword, string similarWord)
        {
                ensureBiGramsLoaded();
           
                int biGrmMtchCnt = 0;
                double totalFrequency=0;
              
                for (int i = 0; i < similarWord.Length-1; ++i)
                {
                    string w=similarWord.Substring(i,2);
                    if(!_wordFrequences.ContainsKey(w)) continue;
                    biGrmMtchCnt++;
                    totalFrequency+=_wordFrequences[w];
                }

              return  (totalFrequency +  biGrmMtchCnt) * 
                  CosineSimilarity.CalculateSimilarityValue(similarWord, keyword) ;
                 
              
         }

        

    }
}
