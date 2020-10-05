using MySUS.HTTP;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySUS.MvcFramework
{
    public class Route
    {
        public Route(string path, Func<HttpRequest, HttpResponse> action)
        {
            Path = path;
            Action = action;
        }

        public string Path { get; set; }

        public Func<HttpRequest, HttpResponse> Action { get; set; }
    }
}
