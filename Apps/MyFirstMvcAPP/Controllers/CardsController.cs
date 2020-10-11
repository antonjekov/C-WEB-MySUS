using MySUS.HTTP;
using MySUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFirstMvcAPP.Controllers
{
    public class CardsController : Controller
    {
        public HttpResponse Add()
        {
            return this.View();
        }

        public HttpResponse All()
        {
            return this.View();
        }

        public HttpResponse Collection()
        {
            return this.View();
        }
    }
}
