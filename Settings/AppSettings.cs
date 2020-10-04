using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserAuthentication.Settings
{
    public class AppSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string https { get; set; }
        public string Audience { get; set; }

    }
}
