using BattleCards.Data;
using Microsoft.EntityFrameworkCore;
using BattleCards.Controllers;
using MySUS.HTTP;
using MySUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;
using BattleCards.Services;

namespace BattleCards
{
    public class Startup : IMvcApplication
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            //AddSingleton - service that have only one instance
            //AddTrancient - every time create new instance
            //AddScoped
            serviceCollection.Add<IUsersService, UsersService>();
            serviceCollection.Add<ICardsService, CardsService>();
        }

        public void Configure(List<Route> routeTable)
        {
            new ApplicationDbContext().Database.Migrate();
        }
    }
}
