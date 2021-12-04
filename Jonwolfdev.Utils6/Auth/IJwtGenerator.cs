using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Jonwolfdev.Utils6.Auth
{
    public interface IJwtGenerator
    {
        JwtSecurityToken GenerateJwtSecurityToken(IReadOnlyList<Claim> claims);
        string SerializeToken(JwtSecurityToken token);
    }
}
