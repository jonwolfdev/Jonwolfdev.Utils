using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jonwolfdev.Utils6.Auth
{
    public class JwtGeneratorOptions
    {
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
        public TimeSpan Expiration { get; set; }
        public bool AddClaimIssuedAtTime { get; set; } = true;
    }
}
