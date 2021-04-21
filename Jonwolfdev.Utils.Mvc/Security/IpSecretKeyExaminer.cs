using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Jonwolfdev.Utils.Mvc.Security
{
    /// <summary>
    /// Check against http context IP and secret key
    /// </summary>
    public class IpSecretKeyExaminer : IIpSecretKeyExaminer
    {
        readonly IList<IPAddress> _allowedIps = new List<IPAddress>();
        readonly IRemoteIpAddressProvider _remoteIpAddressProvider;
        readonly string _secretKey;
        readonly string _secretKeyHeaderName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowedIps">Values separated by ;</param>
        /// <param name="remoteIpAddressProvider"></param>
        /// <param name="secretKey"></param>
        /// <param name="secretKeyHeaderName"></param>
        public IpSecretKeyExaminer(IReadOnlyList<string> allowedIps, IRemoteIpAddressProvider remoteIpAddressProvider, string secretKey, string secretKeyHeaderName)
        {
            if (allowedIps == null)
                throw new ArgumentNullException(nameof(allowedIps));

            foreach(var ip in allowedIps)
            {
                _allowedIps.Add(IPAddress.Parse(ip));
            }

            _remoteIpAddressProvider = remoteIpAddressProvider ?? throw new ArgumentNullException(nameof(remoteIpAddressProvider));
            _secretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
            _secretKeyHeaderName = secretKeyHeaderName ?? throw new ArgumentNullException(nameof(secretKeyHeaderName));
        }
        public bool IsAllowed(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (!InternalIsSecretAllowed(context))
                return false;

            if (!InternalIsIpAllowed(context))
                return false;

            return true;
        }

        public bool IsIpAllowed(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (!InternalIsIpAllowed(context))
                return false;

            return true;
        }

        public bool IsSecretAllowed(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (!InternalIsSecretAllowed(context))
                return false;

            return true;
        }

        bool InternalIsIpAllowed(HttpContext context)
        {
            var remoteIp = _remoteIpAddressProvider.GetIp(context, true);

            bool badIp = true;
            foreach (var address in _allowedIps)
            {
                if (address.Equals(remoteIp))
                {
                    badIp = false;
                    break;
                }
            }

            return !badIp;
        }

        bool InternalIsSecretAllowed(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(_secretKeyHeaderName, out var header))
            {
                return false;
            }

            var value = header.FirstOrDefault();
            if (header.Count != 1 || string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (value == _secretKey)
                return true;

            return false;
        }
    }
}
