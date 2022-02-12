using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimilarWordsGenerationAlgorithmLibrary
{
    public class Similarity
    {
        private static readonly Dictionary<SimilarityMethods, double> _methodBoostValues;
        static Similarity()
        {
            _methodBoostValues = new Dictionary<SimilarityMethods, double>();
            _methodBoostValues.Add(SimilarityMethods.Abbrevation, 0);
            _methodBoostValues.Add(SimilarityMethods.OneCopyBoost, 11);
            _methodBoostValues.Add(SimilarityMethods.OneDelete, 8);
            _methodBoostValues.Add(SimilarityMethods.OneInsert, 7);
            _methodBoostValues.Add(SimilarityMethods.OneRandomReplace, 10);
            _methodBoostValues.Add(SimilarityMethods.OneReplace, 10);
            _methodBoostValues.Add(SimilarityMethods.OneSemiVowelsReplace, 10);
            _methodBoostValues.Add(SimilarityMethods.OneSwap, 9);
            _methodBoostValues.Add(SimilarityMethods.OneVowelReplace, 10);
            _methodBoostValues.Add(SimilarityMethods.OneVowelsInsert, 10);
            _methodBoostValues.Add(SimilarityMethods.TwoDelete, 0);
            _methodBoostValues.Add(SimilarityMethods.TwoInsert, 0);
            _methodBoostValues.Add(SimilarityMethods.TwoReplace, 0);
            _methodBoostValues.Add(SimilarityMethods.TwoSwap, 0);
            _methodBoostValues.Add(SimilarityMethods.TwoVowelInsert, 0);
          

        }
        public string Word { get; set; }
        public double Value { get; set; }
        public SimilarityMethods Method { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Similarity item = (Similarity)obj;
            return (item.Word == this.Word && item.Method == this.Method);
        }

        public override int GetHashCode()
        {

            //Get hash code for the Name field if it is not null.
            int hashProductName = Word == null ? 0 : Word.GetHashCode();

            //Get hash code for the Code field.
            int hashProductCode = Method.GetHashCode();

            //Calculate the hash code for the product.
            return hashProductName ^ hashProductCode;
        }
        public double BoostedValue
        {
            get
            {
                double boost=0;
                //if (this.Word == "mtmustafa")
                //    Debugger.Break();
                foreach(SimilarityMethods item in Enum.GetValues(typeof(SimilarityMethods)))
                {
                    if (this.Method.HasFlag(item))
                        boost += _methodBoostValues[item];
                }
                return this.Value + boost;
            }
        }

        public double ManuelBoostedValue { get; set; }


        public string SocialID { get; set; }

        public override string ToString()
        {
            return this.Word + " , " + this.BoostedValue;
        }
    }
}
