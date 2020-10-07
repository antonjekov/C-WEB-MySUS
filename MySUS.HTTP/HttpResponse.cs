using System;
using System.Collections.Generic;
using System.Text;

namespace MySUS.HTTP
{
    //HTTP/1.1 200 OK
    //Server: MySUSServer 1.0
    //Content-Length: 17
    //Content-Type: text/html
    //
    //body

    public class HttpResponse
    {
        public HttpResponse(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;
            this.Headers = new List<Header>();
            this.Cookies = new List<Cookie>();
            this.Body = new byte[0];
        }

        public HttpResponse(byte[] body,string contentType, HttpStatusCode statusCode=HttpStatusCode.Ok)
        {
            if (body==null)
            {
                throw new ArgumentNullException(nameof(body));
            }
            this.StatusCode = statusCode;
            this.Body = body;
            this.Headers = new List<Header>() 
            {
                {new Header("Content-Type",contentType) },
                {new Header("Content-Length", this.Body.Length.ToString()) }
            };
            this.Cookies = new List<Cookie>();
        }

        public HttpStatusCode StatusCode { get; set; }

        public byte[] Body { get; set; }

        public ICollection<Header> Headers { get; set; }

        public ICollection<Cookie> Cookies { get; set; }

        public override string ToString()
        {
            StringBuilder responseBuilder = new StringBuilder();
            responseBuilder.Append($"HTTP/1.1 {(int)this.StatusCode} {this.StatusCode}"+HttpConstants.NewLine);
            foreach (var header in this.Headers)
            {
                responseBuilder.Append($"{header.ToString()}"+HttpConstants.NewLine);
            }
            foreach (var cookie in this.Cookies)
            {
                responseBuilder.Append($"Set-Cookie: {cookie.ToString()}" + HttpConstants.NewLine);
            }
            responseBuilder.Append(HttpConstants.NewLine);

            //body
            return responseBuilder.ToString();
        }
    }
}
