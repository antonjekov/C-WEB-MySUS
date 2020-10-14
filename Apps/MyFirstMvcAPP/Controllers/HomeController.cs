using BattleCards.ViewModels;
using MySUS.HTTP;
using MySUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleCards.Controllers
{
    public class HomeController: Controller
    {
        [HttpGet("/")]
        public HttpResponse Index()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/cards/all");
            }
            return this.View();
        }

        public HttpResponse About()
        {
            this.SignIn("niki");
            
           return this.View();
        }
    }
}
