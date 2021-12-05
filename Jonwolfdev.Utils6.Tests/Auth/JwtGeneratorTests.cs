using Jonwolfdev.Utils6.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Jonwolfdev.Utils6.Tests.Auth
{
    public class JwtGeneratorTests
    {
        [Fact]
        public void GenerateJwtSecurityToken_Should_Return_Valid_Object()
        {
            // arrange
            var options = new JwtGeneratorStaticOptions()
            {
                Issuer = "issuer_unit_test",
                Audience = "aud_unit_test",
                SigningKey = "sk_secret_sk_secret_000"
            };
            var mockOptions = new Mock<IOptions<JwtGeneratorStaticOptions>>();
            mockOptions.SetupGet(x => x.Value).Returns(options);

            var tokenHandlerParams = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = options.Issuer,
                ValidAudience = options.Audience,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(options.ClockSkewMinutes),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey)),
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            // act
            var generator = new JwtGenerator(mockOptions.Object);
            var token = generator.GenerateJwtSecurityToken(new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, "sub1"),
                new Claim(JwtRegisteredClaimNames.Email, "email1")
            });
            var tokenStr = generator.SerializeToken(token);

            // assert
            Assert.NotNull(token);
            Assert.NotEmpty(tokenStr);

            Assert.True(tokenHandler.CanReadToken(tokenStr));
            var principal = tokenHandler.ValidateToken(tokenStr, tokenHandlerParams, out var newToken);

            var subClaim = principal.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).First();
            var emailClaim = principal.Claims.Where(x => x.Type == ClaimTypes.Email).First();
            Assert.NotNull(subClaim);
            Assert.Equal("sub1", subClaim.Value);
            Assert.Equal("email1", emailClaim.Value);
        }

        [Fact]
        public void GenerateJwtSecurityToken_Should_Return_Valid_Object_InvalidKey()
        {
            // arrange
            var options = new JwtGeneratorStaticOptions()
            {
                Issuer = "issuer_unit_test",
                Audience = "aud_unit_test",
                SigningKey = "sk_secret_sk_secret_000"
            };
            var mockOptions = new Mock<IOptions<JwtGeneratorStaticOptions>>();
            mockOptions.SetupGet(x => x.Value).Returns(options);

            var tokenHandlerParams = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = options.Issuer,
                ValidAudience = options.Audience,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(options.ClockSkewMinutes),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey + "incorrect")),
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            // act
            var generator = new JwtGenerator(mockOptions.Object);
            var token = generator.GenerateJwtSecurityToken(new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, "sub1"),
                new Claim(JwtRegisteredClaimNames.Email, "email1")
            });
            var tokenStr = generator.SerializeToken(token);

            // assert
            Assert.NotNull(token);
            Assert.NotEmpty(tokenStr);

            Assert.True(tokenHandler.CanReadToken(tokenStr));
            Assert.Throws<SecurityTokenSignatureKeyNotFoundException>(() => { tokenHandler.ValidateToken(tokenStr, tokenHandlerParams, out var newToken); });
        }
    }
}