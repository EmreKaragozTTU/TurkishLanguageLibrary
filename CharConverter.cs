using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurkishLanguageLibraryCore
{
    public static class CharConverter
    {
        public static string ToEnglishChars(this string Value)
        {
            Value = Value.Replace("İ", "I");
            Value = Value.Replace("ı", "i");
            Value = Value.Replace("Ü", "U");
            Value = Value.Replace("ü", "u");
            Value = Value.Replace("Ö", "O");
            Value = Value.Replace("ö", "o");
            Value = Value.Replace("Ş", "S");
            Value = Value.Replace("ş", "s");
            Value = Value.Replace("Ç", "C");
            Value = Value.Replace("ç", "c");
            Value = Value.Replace("Ğ", "G");
            Value = Value.Replace("ğ", "g");
            return Value;
        }

    }
}
