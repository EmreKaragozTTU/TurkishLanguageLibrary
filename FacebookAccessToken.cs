using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TurkishLanguageLibraryCore
{
    [DataContract]
    public class FacebookAccessToken : IAccessToken
    {
        public SocialMediaType TokenType { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string TokenValue { get; set; }

        public FacebookAccessToken(SocialMediaType tokenType, string value, DateTime expirationDate)
        {
            this.TokenValue = value;
            this.ExpirationDate = expirationDate;
            this.TokenType = tokenType;
        }
    }
}
