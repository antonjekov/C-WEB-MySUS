using MySUS.HTTP;
using MySUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyFirstMvcAPP.Controllers
{
    public class UsersController: Controller
    {
        public HttpResponse Login(HttpRequest request)
        {
            return this.View("Views/Users/Login.html");
        }

        public HttpResponse Register(HttpRequest request)
        {
            return this.View("Views/Users/Register.html");            
        }
    }
}
