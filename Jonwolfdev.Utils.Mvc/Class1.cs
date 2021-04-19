using System;

namespace Jonwolfdev.Utils.Mvc
{
    public class Class1
    {
        public string Ok(Microsoft.AspNetCore.Http.HttpContext c)
        {
            return c.Connection.RemoteIpAddress.ToString();
        }
    }
}
