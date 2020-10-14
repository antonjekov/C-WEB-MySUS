using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MySUS.HTTP
{
//GET /assets/css/money.css? _ = 1549445024 HTTP/1.1
//Host: webnews.bg
//Connection: keep-alive
//Pragma: no-cache
//Cache-Control: no-cache
//User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36
//Accept: text/css,*/*;q=0.1
//Sec-Fetch-Site: cross-site
//Sec-Fetch-Mode: no-cors
//Sec-Fetch-Dest: style
//Referer: https://money.bg/
//Accept-Encoding: gzip, deflate, br
//Accept-Language: bg,en;q=0.9,es;q=0.8
//
//some body here

    public class HttpRequest
    {
        public static IDictionary<string, Dictionary<string, string>> Sessions = new Dictionary<string, Dictionary<string, string>>();

        public HttpRequest(string requestString)
        {
            this.Headers = new List<Header>();
            this.Cookies = new List<Cookie>();
            this.FormData = new Dictionary<string, string>();

            var lines = requestString.Split(new string[] { HttpConstants.NewLine }, StringSplitOptions.None);

            //GET /assets/css/money.css? _ = 1549445024 HTTP/1.1
            var headerLine = lines[0];
            var headerLineParts = headerLine.Split(' ');
            this.Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), headerLineParts[0], true);
            this.Path = headerLineParts[1];

            bool isInHeaders = true;
            int lineIndex = 1;
            StringBuilder bodyLines = new StringBuilder();

            while (lineIndex<lines.Length)
            {
                var currentLine = lines[lineIndex];
                lineIndex++;

                if (string.IsNullOrWhiteSpace(currentLine))//After two new lines starts body 
                {
                    isInHeaders = false;
                    continue;
                }

                if (isInHeaders)
                {
                    var newHeader = new Header(currentLine);                    
                    this.Headers.Add(newHeader);
                }
                else
                {
                    bodyLines.AppendLine(currentLine);
                }                
            }
            if (this.Headers.Any(h=> h.Name.Equals(HttpConstants.RequestCookieHeader,StringComparison.OrdinalIgnoreCase)))
            {
                var cookiesAsString = this.Headers
                    .FirstOrDefault(h => h.Name.Equals(HttpConstants.RequestCookieHeader, StringComparison.OrdinalIgnoreCase))
                    .Value;
                var cookies = cookiesAsString.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var cookie in cookies)
                {
                    
                    this.Cookies.Add(new Cookie(cookie));
                }
            }

            var sessionCookie = this.Cookies.FirstOrDefault(cookie => cookie.Name == HttpConstants.SessionCookieName);
            if (sessionCookie==null)
            {
                var sessionId = Guid.NewGuid().ToString();
                this.Session = new Dictionary<string, string>();
                Sessions.Add(sessionId, this.Session);
                this.Cookies.Add(new Cookie(HttpConstants.SessionCookieName, sessionId));
            }
            else if (!Sessions.ContainsKey(sessionCookie.Value))
            {
                this.Session = new Dictionary<string, string>();
                Sessions.Add(sessionCookie.Value, this.Session);
            }
            else
            {
                this.Session = Sessions[sessionCookie.Value];
            }


            this.Body = bodyLines.ToString().TrimEnd('\n', '\r');
            var parameters = this.Body.Split(new string[] { "&"},StringSplitOptions.RemoveEmptyEntries);
            foreach (var parameter in parameters)
            {
                var parameterParts = parameter.Split(new[] { '=' },2);
                var name = parameterParts[0];
                var value = WebUtility.UrlDecode(parameterParts[1]);
                if (this.FormData.ContainsKey(name))
                {
                    this.FormData[name] = value;
                    continue;
                }
                this.FormData.Add(name, value);
            }
        }

        public string Path { get; set; }

        public HttpMethod Method { get; set; }

        public string Body { get; set; }

        public Dictionary<string,string> Session { get; set; }

        public ICollection<Header> Headers { get; set; }

        public ICollection<Cookie> Cookies { get; set; }

        public IDictionary<string, string> FormData { get; set; }
    }
}
