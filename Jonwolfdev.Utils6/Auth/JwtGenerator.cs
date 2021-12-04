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
        readonly JwtGeneratorOptions _options;
        readonly SigningCredentials _signingCredentials;
        readonly SymmetricSecurityKey _key;
        readonly JwtSecurityTokenHandler _tokenHandler = new();

        public JwtGenerator(JwtGeneratorOptions options, string signingKey)
        {
            _options = options;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            _signingCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
        }

        public JwtSecurityToken GenerateJwtSecurityToken(IReadOnlyDictionary<string, string> dictClaims)
        {
            var claims = new List<Claim>();
            foreach (var item in dictClaims)
            {
                claims.Add(new Claim(item.Key, item.Value));
            }

            if (_options.AddClaimIssuedAtTime)
                claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));

            return new JwtSecurityToken(
                _options.Issuer, _options.Audience, claims,
                DateTime.UtcNow, DateTime.UtcNow.Add(_options.Expiration),
                _signingCredentials);
        }

        public string SerializeToken(JwtSecurityToken token)
        {
            return _tokenHandler.WriteToken(token);
        }
    }
}
