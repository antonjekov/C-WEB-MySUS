using BattleCards.Data;
using BattleCards.Services;
using BattleCards.ViewModels.Cards;
using MySUS.HTTP;
using MySUS.MvcFramework;
using System;

namespace BattleCards.Controllers
{
    public class CardsController : Controller
    {
        private ICardsService cardsService;

        public CardsController(ICardsService cardsService)
        {
           this.cardsService = cardsService;
        }

        public HttpResponse Add()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(AddCardInputModel model)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }

            if (string.IsNullOrEmpty(model.Name)|| model.Name.Length < 5||model.Name.Length>15)
            {
                return this.Error("Name should be between 5 and 15 characters long.");
            }

            if (string.IsNullOrWhiteSpace(model.Image))
            {
                return this.Error("ImageUrl is required.");
            }

            if (!Uri.TryCreate(model.Image,UriKind.Absolute,out _))
            {
                return this.Error("Url is not valid");
            }

            if (string.IsNullOrWhiteSpace(model.Keyword))
            {
                return this.Error("Keyword is required.");
            }

            if (model.Attack<0)
            {
                return this.Error("Attack should be non negative integer");
            }

            if (model.Health < 0)
            {
                return this.Error("Health should be non negative integer");
            }

            if (string.IsNullOrWhiteSpace(model.Description)|| model.Description.Length>200)
            {
                return this.Error("Description should be between 1 and 200 characters long.");
            }

            var cardId = this.cardsService.AddCard(model);

            this.cardsService.AddCardToUserCollection(this.GetUserId(), cardId);

            return this.Redirect("/cards/all");
        }

        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            var cardsViewModel = this.cardsService.GetAll();

            return this.View(cardsViewModel);
        }

        public HttpResponse Collection()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            var cards =this.cardsService.GetAllByUserId(this.GetUserId());
            return this.View(cards);
        }

        public HttpResponse AddToCollection(int cardId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            var userId = this.GetUserId();
            
            this.cardsService.AddCardToUserCollection(userId, cardId);
            return Redirect("/cards/collection");
        }

        public HttpResponse RemoveFromCollection(int cardId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            var userId = this.GetUserId();

            this.cardsService.RemoveCardFromUserCollection(userId, cardId);
            return Redirect("/cards/all");
        }
    }
}
