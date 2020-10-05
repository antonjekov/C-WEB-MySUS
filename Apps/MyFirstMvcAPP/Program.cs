using MyFirstMvcAPP.Controllers;
using MySUS.HTTP;
using System.Threading.Tasks;

namespace MyFirstMvcAPP
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IHttpServer server = new HttpServer();

            server.AddRoute("/", new HomeController().Index);
            server.AddRoute("/about", new HomeController().About);

            server.AddRoute("/favicon.ico", new StaticFilesController().Favicon);

            server.AddRoute("/users/login", new UsersController().Login);
            server.AddRoute("/users/register", new UsersController().Register);

            await server.StartAsync(80);
        }
    }
}
