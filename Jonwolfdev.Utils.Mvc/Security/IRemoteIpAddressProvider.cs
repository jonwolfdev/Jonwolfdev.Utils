using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Jonwolfdev.Utils.Mvc.Security
{
    /// <summary>
    /// Interface for RemoteIpAddressProvider
    /// </summary>
    public interface IRemoteIpAddressProvider
    {
        /// <summary>
        /// Returns IP based on firewall header or from HttpContext directly
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ignoreClientIpHeaderValue"></param>
        /// <returns></returns>
        IPAddress GetIp(HttpContext context, bool ignoreClientIpHeaderValue);
    }
}
