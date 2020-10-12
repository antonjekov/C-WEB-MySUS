using BattleCards.Data;
using BattleCards.ViewModels;
using MySUS.HTTP;
using MySUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleCards.Controllers
{
    public class CardsController : Controller
    {
        public HttpResponse Add()
        {
            return this.View();
        }

        [HttpPost("/Cards/Add")]
        public HttpResponse DoAdd()
        {
            var dbContext = new ApplicationDbContext();

            if (this.Request.FormData["name"].Length < 5)
            {
                return this.Error("Name should be at least 5 characters long.");
            }
            

            dbContext.Cards.Add(new Card
            {
                Attack = int.Parse(this.Request.FormData["attack"]),
                Health = int.Parse(this.Request.FormData["health"]),
                Description = this.Request.FormData["description"],
                Name = this.Request.FormData["name"],
                ImageUrl = this.Request.FormData["image"],
                Keyword = this.Request.FormData["keyword"]                
            });

            //var request = this.Request;
            //var viewModel = new DoAddViewModel()
            //{
            //    Attack = int.Parse(this.Request.FormData["attack"]),
            //    Health = int.Parse(this.Request.FormData["health"])
            //};

            dbContext.SaveChanges();

            return this.Redirect("/");
        }

        public HttpResponse All()
        {
            var db = new ApplicationDbContext();
            var cardsViewModel = db.Cards.Select(x => new CardViewModel
            {
                Attack = x.Attack,
                Description=x.Description,
                Health=x.Health,
                ImageUrl=x.ImageUrl,
                Keyword=x.Keyword,
                Name=x.Name
            }).ToList() ;

            return this.View(new AllCardsViewModel { Cards= cardsViewModel });
        }

        public HttpResponse Collection()
        {
            return this.View();
        }
    }
}
