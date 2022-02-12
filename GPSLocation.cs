using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurkishLanguageLibraryCore
{
    [Serializable]
    public class GPSLocation
    {
        public double Latitude
        {
            get;
        
            set;
          
        }

        public double Longitude
        {
            get;
       
            set;
         
        }

        public string LocationWellKnowName
        {
            get;
         
            set;
         
        }

        public List<string> LocationTags
        {
            get;

            set;

        }
    }
}
