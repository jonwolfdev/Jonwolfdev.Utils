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
        public IPAddress GetIp(HttpContext context)
        {
            try
            {
                if (context == null)
                    throw new ArgumentNullException(nameof(context));

                bool getIpDirectly = false;
                IPAddress remoteIp = null;
                if (_behindFirewall)
                {
                    // Ip from header
                    if (context.Request.Headers.TryGetValue(_clientIpHeaderName, out var headerValue))
                    {
                        // Always only 1 value
                        var ip = headerValue.FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(ip))
                        {
                            IPAddress.TryParse(ip, out remoteIp);
                        }
                    }
                    else
                    {
                        // No header, we can check if the connection is from local
                        if (context.Connection.RemoteIpAddress.Equals(context.Connection.LocalIpAddress))
                        {
                            // Remote ip = local ip address (like requesting 192.1.x.x instead of 127.0.0.1)
                            getIpDirectly = true;
                        }
                        else if (IPAddress.IsLoopback(context.Connection.RemoteIpAddress))
                        {
                            // Connection through ::1 or 127.0.0.1
                            getIpDirectly = true;
                        }
                    }
                }
                else
                {
                    // It's not behind a firewall, so get IP directly
                    getIpDirectly = true;
                }

                if (getIpDirectly)
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
                throw new Exception($"Error at {nameof(GetIp)}/{nameof(RemoteIpAddressProvider)}. " + 
                    $", {nameof(_clientIpHeaderName)} = {_clientIpHeaderName}, {nameof(_behindFirewall)} = {_behindFirewall}", e);
            }
        }
    }
}
