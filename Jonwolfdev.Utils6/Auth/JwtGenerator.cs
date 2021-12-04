using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Jonwolfdev.Utils6.Auth
{
    public class JwtGenerator : IJwtGenerator
    {
        readonly IOptionsMonitor<JwtGeneratorOptions> _options;
        readonly SigningCredentials _signingCredentials;
        readonly JwtSecurityTokenHandler _tokenHandler = new();

        public JwtGenerator(IOptionsMonitor<JwtGeneratorOptions> options, string signingKey)
        {
            _options = options;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        public JwtSecurityToken GenerateJwtSecurityToken(IReadOnlyList<Claim> claims)
        {
            var claimsList = new List<Claim>();
            claimsList.AddRange(claims);

            var options = _options.CurrentValue;
            if (options.AddClaimIssuedAtTime)
                claimsList.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));

            return new JwtSecurityToken(
                options.Issuer, options.Audience, claimsList,
                DateTime.UtcNow, DateTime.UtcNow.Add(options.Expiration),
                _signingCredentials);
        }

        public string SerializeToken(JwtSecurityToken token)
        {
            return _tokenHandler.WriteToken(token);
        }
    }
}
