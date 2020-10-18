using Microsoft.EntityFrameworkCore;
using MySUS.HTTP;
using MySUS.MvcFramework;
using Suls.Data;
using Suls.Services;
using System.Collections.Generic;

namespace Suls
{
    public class StartUp : IMvcApplication
    {
        public void Configure(List<Route> routeTable)
        {
            new ApplicationDbContext().Database.Migrate();
        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Add<IProblemService, ProblemService>();
            serviceCollection.Add<IUserService, UserService>();
            serviceCollection.Add<ISubmissionServices, SubmissionServices>();
        }
    }
}
