using MySUS.HTTP;
using MySUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BattleCards.Controllers
{
    public class UsersController: Controller
    {
        public HttpResponse Login()
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

        public HttpResponse Register()
        {
            return this.View();            
        }

        [HttpPost("/users/register")]
        public HttpResponse DoRegister()
        {
            //read data
            //check user
            //log user
            //redirect
            return this.Redirect("/");
        }

        // /users/logout
        public HttpResponse Logout()
        {
            if (this.IsUserSignedIn())
            {
            this.SignOut();
            return this.Redirect("/");
            }
            else
            {
                return this.Error("Only signed in users can logout.");
            }
        }
    }
}
