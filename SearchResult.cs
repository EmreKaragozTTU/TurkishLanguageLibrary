using TurkishLanguageLibraryCore;
using TurkishLanguageLibraryCore.Facebook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahteHesapSearchConsole
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class SearchResult
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]

        public string SocialID { get; set; }

        public SocialMediaAccountBase Account { get; set; }

        public SocialMediaSearchState State { get; set; }

        public SocialMediaType MediaType { get; set; }

        public int MediaTypeValue { get { return (int)MediaType; } set { MediaType = (SocialMediaType)value ;} }

        public int StateNumber { get { return (int)this.State; } set { State = (SocialMediaSearchState)value; } }

        public string Word { get; set; }

        public string Error { get; set; }

        public bool? IsFake
        {
            get
            {
                if (this.Account == null || !this.Account.IsLoaded) return null;
                return this.RealityValue < Program.ThresholdValue;
            }

        }

        public string StateDescription
        {
            get
            {
                switch (this.State)
                {
                    case SocialMediaSearchState.Searching:
                       return  "Hesap aranıyor...";
                    case SocialMediaSearchState.Found:
                       return "Hesap Bulundu.";
                    case SocialMediaSearchState.NotFound:
                       return "Hesap Bulunamadı!";
                    case SocialMediaSearchState.Error:
                       return "HATA!";
                    default:
                       return "";
                }

            }
        }

        public int? RealityValue { get; set; }

        public double Similarity { get; set; }

        public string IsReal { get; set; }

        public TestState TestState { get; set; }

        public Opinion Opinion { get; set; }
    }
}
