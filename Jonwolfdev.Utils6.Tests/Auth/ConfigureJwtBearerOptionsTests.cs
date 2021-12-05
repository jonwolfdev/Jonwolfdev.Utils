using Jonwolfdev.Utils6.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Jonwolfdev.Utils6.Tests.Auth
{
    public class ConfigureJwtBearerOptionsTests
    {
        [Fact]
        public void ConfigureJetBearerOptions_Should_Configure()
        {
            // arrange
            var options = new JwtGeneratorStaticOptions()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifeTime = true,
                
                Issuer = "issuer_unit_test",
                Audience = "aud_unit_test",
                SigningKey = "sk_secret_sk_secret_000"
            };
            var mockOptions = new Mock<IOptions<JwtGeneratorStaticOptions>>();
            mockOptions.SetupGet(x => x.Value).Returns(options);
            var jwtOptions = new JwtBearerOptions();

            // act
            var configureOptions = new ConfigureJwtBearerOptions(mockOptions.Object);
            configureOptions.Configure(JwtBearerDefaults.AuthenticationScheme, jwtOptions);

            // assert
            Assert.Equal(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey)).ToString(), jwtOptions.TokenValidationParameters.IssuerSigningKey.ToString());
            Assert.Equal(options.ValidateAudience, jwtOptions.TokenValidationParameters.ValidateIssuer);
            Assert.Equal(options.ValidateIssuer, jwtOptions.TokenValidationParameters.ValidateIssuer);
            Assert.Equal(options.ValidateIssuerSigningKey, jwtOptions.TokenValidationParameters.ValidateIssuer);
            Assert.Equal(options.ValidateLifeTime, jwtOptions.TokenValidationParameters.ValidateLifetime);

            Assert.Equal(options.Issuer, jwtOptions.TokenValidationParameters.ValidIssuer);
            Assert.Equal(options.Audience, jwtOptions.TokenValidationParameters.ValidAudience);
        }
    }
}
