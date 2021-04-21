using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Jonwolfdev.Utils.Common.IpThrottle
{
    public class IpThrottleBlocker : IIpThrottleBlocker
    {
        readonly Dictionary<IPAddress, DateTimeOffset> _db = new Dictionary<IPAddress, DateTimeOffset>();

        /// <summary>
        /// 500,000 ips in dictionary (IpAddress, DateTimeOffset) = 100mb
        /// 1 million = 200 mb
        /// 1 item = 210 bytes~
        /// </summary>
        readonly int _maxIpsInDictionary = 100000;
        readonly int _throttleWaitMS = 2000;

        readonly object _lock = new object();

        public IpThrottleBlocker(int maxIpsInDict, int throttleWaitMS)
        {
            _maxIpsInDictionary = maxIpsInDict;
            _throttleWaitMS = throttleWaitMS;
        }

        public bool Block(IPAddress ip)
        {
            if (ip == null)
                throw new ArgumentNullException(nameof(ip));

            DateTimeOffset lastAccess;
            DateTimeOffset utcnow = DateTimeOffset.UtcNow;

            lock (_lock)
            {
                if (!_db.TryGetValue(ip, out lastAccess))
                {
                    lastAccess = utcnow;

                    CleanDictionary();

                    _db[ip] = lastAccess;

                    return false;
                }

                if (lastAccess.AddMilliseconds(_throttleWaitMS) > utcnow)
                {
                    // Need to wait
                    return true;
                }

                // Update the value
                _db[ip] = utcnow;
                return false;
            }
        }

        void CleanDictionary()
        {
            if (_db.Count > _maxIpsInDictionary)
            {
                _db.Clear();
            }
        }
    }
}
