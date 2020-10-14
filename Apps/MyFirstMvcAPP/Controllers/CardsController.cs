﻿using BattleCards.Data;
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
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
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
          
            dbContext.SaveChanges();

            return this.Redirect("/cards/all");
        }

        public HttpResponse All()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
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

            return this.View(cardsViewModel);
        }

        public HttpResponse Collection()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            return this.View();
        }
    }
}
