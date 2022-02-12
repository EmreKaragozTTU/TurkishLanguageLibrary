using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimilarWordsGenerationAlgorithmLibrary
{
    public static class CosineSimilarity
    {

        public static int ScalarMultiply(this IEnumerable<int> vector1, IEnumerable<int> vector2)
        {
           
            var vector1List = vector1.ToList();
            var vector2List = vector2.ToList();
             if (vector1List.Count != vector2List.Count)
                throw new Exception("Vector Boyutları Eşit Olmalıdır.");
            int toplam = 0;
            for (int i = 0; i < vector1List.Count; i++)
                toplam += vector1List[i] * vector2List[i];
            return toplam;

        }

        public static double CalculateSimilarityValue(String stringOne, String stringTwo)
        {
            var stringOneArray = stringOne.ToCharArray();
            var stringTwoArray = stringTwo.ToCharArray();
            var unionOfStrings = stringOneArray.Union(stringTwoArray).ToList();
            //var stringTwoArray = stringTwo.ToString();
            //var unionOfStrings = stringOneArray.Union(stringTwoArray);
            var stringOneOccurrenceVector = unionOfStrings.Select(p => stringOneArray.Count(k => k == p));
            var stringTwoOccurrenceVector = unionOfStrings.Select(p => stringTwoArray.Count(k => k == p));

            var dotProduct = stringOneOccurrenceVector.ScalarMultiply(stringTwoOccurrenceVector);

            double vectorOneMagnitude = stringOneOccurrenceVector.Magnitude();
            double vectorTwoMagnitude = stringTwoOccurrenceVector.Magnitude();

            return dotProduct / (vectorOneMagnitude * vectorTwoMagnitude);
        }
          
                


        public static double Magnitude(this IEnumerable<int> vector)
        {
            double magnitude = 0;
            foreach (var item in vector)
            {
                magnitude += Math.Pow(item, 2);
            }
            return Math.Sqrt(magnitude);
        }

       
       
    }
}
