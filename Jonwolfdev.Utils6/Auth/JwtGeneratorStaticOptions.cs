using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jonwolfdev.Utils6.Auth
{
    public class JwtGeneratorStaticOptions
    {
        [Required, MinLength(1)]
        public string Issuer { get; set; } = "";

        [Required, MinLength(1)]
        public string Audience { get; set; } = "";

        /// <summary>
        /// TODO: Do not have clear text key in memory
        /// </summary>
        [Required, MinLength(3)]
        public string SigningKey { get; set; } = "";


        public bool AddClaimIssuedAtTime { get; set; } = true;        
        public bool ValidateIssuer { get; set; } = true;
        public bool ValidateAudience { get; set; } = true;
        public bool ValidateIssuerSigningKey { get; set; } = true;
        public bool ValidateLifeTime { get; set; } = true;


        [Range(1, int.MaxValue)]
        public int ExpirationMinutes { get; set; } = 60;
        public int ClockSkewMinutes { get; set; } = 5;
    }
}
