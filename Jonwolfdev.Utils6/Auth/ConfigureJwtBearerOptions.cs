using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jonwolfdev.Utils6.Auth
{
    public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        readonly JwtGeneratorStaticOptions _options;
        public ConfigureJwtBearerOptions(IOptions<JwtGeneratorStaticOptions> options)
        {
            _options = options.Value;
        }

        public void Configure(string name, JwtBearerOptions options)
        {
            if (name == JwtBearerDefaults.AuthenticationScheme)
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = _options.ValidateIssuer,
                    ValidateAudience = _options.ValidateAudience,
                    ValidateIssuerSigningKey = _options.ValidateIssuerSigningKey,
                    ValidateLifetime = _options.ValidateLifeTime,

                    ValidAudience = _options.Audience,
                    ValidIssuer = _options.Issuer,
                    ClockSkew = TimeSpan.FromMinutes(_options.ClockSkewMinutes),

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey))
                };
                return;
            }
            throw new NotImplementedException("JWD.U6: Unkown name for authentication scheme");
        }

        public void Configure(JwtBearerOptions options)
        {
            throw new NotImplementedException("JWD.U6: This method is not implemented");
        }
    }
}
