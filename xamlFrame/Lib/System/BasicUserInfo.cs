using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xamlFrame.Lib.System
{
    class BasicUserInfo
    {
        public string CurrUserNameFull { get; }
        public string CurrUserName { get; }
        public string CurrUserDomain { get; }

        public BasicUserInfo()
        {
            CurrUserNameFull = Environment.UserName; // format is: domain\username
            CurrUserDomain = Environment.UserDomainName;
            CurrUserName = Environment.UserName.Remove(0, (CurrUserDomain.Length + 1));
         }
    }
}
