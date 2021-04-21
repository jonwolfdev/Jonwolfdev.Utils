using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jonwolfdev.Utils.Mvc.Security
{
    public interface IIpSecretKeyExaminer
    {
        bool IsAllowed(HttpContext context);
        bool IsIpAllowed(HttpContext context);
        bool IsSecretAllowed(HttpContext context);
    }
}
