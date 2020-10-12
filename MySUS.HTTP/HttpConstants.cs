using System;
using System.Collections.Generic;
using System.Text;

namespace MySUS.HTTP
{
    public static class HttpConstants
    {
        public const string NewLine = "\r\n";
        public const int BufferSize = 4096;
        public const string RequestCookieHeader = "Cookie";
        public const string SessionCookieName = "SUS_SID";
    }
}
