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
            return this.View();
        }

        public HttpResponse Register(HttpRequest request)
        {
            return this.View();            
        }

        public HttpResponse DoLogin(HttpRequest request)
        {
            //read data
            //check user
            //log user
            //redirect
            return this.Redirect("/");
        }
    }
}
