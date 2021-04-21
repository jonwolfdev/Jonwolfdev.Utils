using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Jonwolfdev.Utils.Common.IpThrottle
{
    public interface IThrottleBlocker
    {
        public bool Block(IPAddress ip);
    }
}
