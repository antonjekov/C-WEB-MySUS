using Microsoft.EntityFrameworkCore;
using MySUS.HTTP;
using MySUS.MvcFramework;
using SharedTrip.Data;
using System;
using System.Collections.Generic;
using System.Text;

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
            
        }
    }
}
