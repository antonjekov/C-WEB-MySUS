using BattleCards.Data;
using BattleCards.ViewModels;
using Microsoft.EntityFrameworkCore;
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
        private ApplicationDbContext db;

        public CardsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public HttpResponse Add()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            return this.View();
        }

        [HttpPost("/Cards/Add")]
        public HttpResponse DoAdd(string attack, string health, string description, string name, string image, string keyword)
        {
            

            if (this.Request.FormData["name"].Length < 5)
            {
                return this.Error("Name should be at least 5 characters long.");
            }
            

            this.db.Cards.Add(new Card
            {
                Attack = int.Parse(attack),
                Health = int.Parse(health),
                Description = description,
                Name = name,
                ImageUrl = image,
                Keyword = keyword               
            });
          
            this.db.SaveChanges();

            return this.Redirect("/cards/all");
        }

        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            var cardsViewModel = this.db.Cards.Select(x => new CardViewModel
            {
                Attack = x.Attack,
                Description=x.Description,
                Health=x.Health,
                ImageUrl=x.ImageUrl,
                Keyword=x.Keyword,
                Name=x.Name
            }).ToList() ;

            return this.View(cardsViewModel);
        }

        public HttpResponse Collection()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            return this.View();
        }
    }
}
