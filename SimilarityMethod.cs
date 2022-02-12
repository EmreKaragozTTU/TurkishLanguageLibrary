using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimilarWordsGenerationAlgorithmLibrary
{
    [Flags]
    public enum SimilarityMethods:ulong
    {
        OneReplace=1,
        OneVowelReplace=2,
        OneSemiVowelsReplace=4,
        OneRandomReplace=8,
        OneInsert=16,
        OneVowelsInsert=32,
        OneDelete=64,
        OneSwap=128,
        OneCopyBoost=256,
        TwoReplace=512,
        TwoInsert=1024,
        TwoVowelInsert=2048,
        TwoDelete=4096,
        TwoSwap=8192,
        Abbrevation=16384

    }
}
