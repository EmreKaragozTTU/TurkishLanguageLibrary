using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurkishLanguageLibraryCore
{
    public enum ErrorType
    {
        NoError,
        NotExists,
        LimitReached,
        Blocked,
        UndefinedError,
        ConnectionError,
        AppAuthError,
        InvalidTokenValue,
        AccessTokenExpired,
        UserPasswordChanged,
        UserIsAccount,
        AccessNotAllowed
    }
}
