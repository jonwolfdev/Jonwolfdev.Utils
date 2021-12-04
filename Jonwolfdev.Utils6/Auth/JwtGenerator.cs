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
        readonly JwtGeneratorStaticOptions _options;
        readonly JwtSecurityTokenHandler _tokenHandler = new();
        readonly SigningCredentials _signingCredentials;

        public JwtGenerator(IOptions<JwtGeneratorStaticOptions> options)
        {
            _options = options.Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
            _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        public JwtSecurityToken GenerateJwtSecurityToken(IReadOnlyList<Claim> claims)
        {
            var claimsList = new List<Claim>();
            claimsList.AddRange(claims);

            if (_options.AddClaimIssuedAtTime)
                claimsList.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));

            return new JwtSecurityToken(
                _options.Issuer, _options.Audience, claimsList,
                DateTime.UtcNow, DateTime.UtcNow.Add(TimeSpan.FromMinutes(_options.ExpirationMinutes)),
                _signingCredentials);
        }

        public string SerializeToken(JwtSecurityToken token)
        {
            return _tokenHandler.WriteToken(token);
        }

    }
}
