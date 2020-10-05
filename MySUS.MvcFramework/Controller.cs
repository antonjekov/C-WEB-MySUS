using MySUS.HTTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace MySUS.MvcFramework
{
    public abstract class Controller
    {
        public HttpResponse View (string viewPath)
        {
            var responseHtml = File.ReadAllText(viewPath);
            var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            var response = new HttpResponse(responseBodyBytes, "text/html");
            return response;
        }
    }
}
