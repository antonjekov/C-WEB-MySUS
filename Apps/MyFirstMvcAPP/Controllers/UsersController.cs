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
        public HttpResponse Login()
        {
            return this.View();
        }

        public HttpResponse Register()
        {
            return this.View();            
        }

        [HttpPost("/users/login")]
        public HttpResponse DoLogin()
        {
            //read data
            //check user
            //log user
            //redirect
            return this.Redirect("/");
        }
    }
}
