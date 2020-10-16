using BattleCards.Data;
using BattleCards.ViewModels.Cards;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleCards.Services
{
    public class CardsService : ICardsService
    {
        private readonly ApplicationDbContext db;

        public CardsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public int AddCard(AddCardInputModel input)
        {
            var card = new Card
            {
                Attack = input.Attack,
                Health = input.Health,
                Description = input.Description,
                Name = input.Name,
                ImageUrl = input.Image,
                Keyword = input.Keyword
            };
            this.db.Cards.Add(card);
            this.db.SaveChanges();
            return card.Id;
        }

        public void AddCardToUserCollection(string userId, int cardId)
        {
            if (this.db.UserCards.Any(x=>x.UserId==userId&&x.CardId==cardId))
            {
                return;
            }
            this.db.UserCards.Add(new UserCard(userId, cardId));
            db.SaveChanges();
        }

        public void RemoveCardFromUserCollection(string userId, int cardId)
        {
            var card = this.db.UserCards.FirstOrDefault(x => x.UserId == userId && x.CardId == cardId);
            if (card==null)
            {
                return;
            }
            this.db.UserCards.Remove(card);
            db.SaveChanges();
        }

        public IEnumerable<CardViewModel> GetAll()
        {
           return  this.db.Cards.Select(x => new CardViewModel
            {
                Attack = x.Attack,
                Description = x.Description,
                Health = x.Health,
                ImageUrl = x.ImageUrl,
                Keyword = x.Keyword,
                Name = x.Name,
                Id = x.Id
            }).ToList();
        }

        public IEnumerable<CardViewModel> GetAllByUserId(string userId)
        {
            return this.db.UserCards.Where(x => x.UserId == userId)
                            .Select(x => new CardViewModel()
                            {
                                Attack = x.Card.Attack,
                                Description = x.Card.Description,
                                Health = x.Card.Health,
                                ImageUrl = x.Card.ImageUrl,
                                Keyword = x.Card.Keyword,
                                Name = x.Card.Name,
                                Id = x.CardId
                            }).ToList();
        }

    }
}
