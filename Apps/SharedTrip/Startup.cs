using Microsoft.EntityFrameworkCore;
using MySUS.HTTP;
using MySUS.MvcFramework;
using SharedTrip.Data;
using SharedTrip.Services;
using System.Collections.Generic;

namespace SharedTrip
{
    public class Startup : IMvcApplication
    {
        public void Configure(List<Route> routeTable)
        {
           new ApplicationDbContext().Database.Migrate();
        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Add<IUsersService, UsersService>();
            serviceCollection.Add<ITripsService, TripsService>();
            serviceCollection.Add<IUserTripService, UserTripService>();
        }
    }
}
