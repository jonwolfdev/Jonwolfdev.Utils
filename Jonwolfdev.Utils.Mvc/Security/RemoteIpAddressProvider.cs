using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Jonwolfdev.Utils.Mvc.Security
{
    /// <summary>
    /// Wrapper class to get IP from firewall header or from HttpContext directly
    /// </summary>
    public class RemoteIpAddressProvider : IRemoteIpAddressProvider
    {
        readonly bool _behindFirewall;
        readonly string _clientIpHeaderName;

        /// <summary>
        /// </summary>
        /// <param name="behindCloudFlare">Get IP from header</param>
        /// <param name="clientIpHeaderName">Header name</param>
        public RemoteIpAddressProvider(bool behindCloudFlare, string clientIpHeaderName)
        {
            _behindFirewall = behindCloudFlare;
            _clientIpHeaderName = clientIpHeaderName ?? throw new ArgumentNullException(nameof(clientIpHeaderName));
        }

        /// <summary>
        /// Returns IP
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ignoreClientIpHeaderValue"></param>
        /// <returns></returns>
        public IPAddress GetIp(HttpContext context, bool ignoreClientIpHeaderValue)
        {
            try
            {
                if (context == null)
                    throw new ArgumentNullException(nameof(context));

                IPAddress remoteIp = null;
                if (_behindFirewall && !ignoreClientIpHeaderValue)
                {
                    // Ip from header
                    if (context.Request.Headers.TryGetValue(_clientIpHeaderName, out var headerValue))
                    {
                        // Always only 1 value
                        var ip = headerValue.FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(ip))
                        {
                            if (!IPAddress.TryParse(ip, out remoteIp))
                            {
                                remoteIp = null;
                            }
                        }
                    }
                }
                else
                {
                    // Development OR actual localhost
                    remoteIp = context.Connection.RemoteIpAddress;
                    if (remoteIp.IsIPv4MappedToIPv6)
                        remoteIp = remoteIp.MapToIPv4();
                }

                if (remoteIp == null)
                    throw new Exception($"Remote ip is null...");

                return remoteIp;
            }
            catch (Exception e)
            {
                throw new Exception($"Error at {nameof(GetIp)}/{nameof(RemoteIpAddressProvider)}. {nameof(ignoreClientIpHeaderValue)} = {ignoreClientIpHeaderValue}" + 
                    $", {nameof(_clientIpHeaderName)} = {_clientIpHeaderName}, {nameof(_behindFirewall)} = {_behindFirewall}", e);
            }
        }
    }
}
