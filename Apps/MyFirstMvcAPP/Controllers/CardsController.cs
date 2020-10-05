using MySUS.HTTP;
using MySUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFirstMvcAPP.Controllers
{
    public class CardsController : Controller
    {
        public HttpResponse Add(HttpRequest request)
        {
            return this.View();
        }

        public HttpResponse All(HttpRequest request)
        {
            return this.View();
        }

        public HttpResponse Collection(HttpRequest request)
        {
            return this.View();
        }
    }
}
