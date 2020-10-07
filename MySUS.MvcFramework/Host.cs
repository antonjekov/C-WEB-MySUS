
using MySUS.HTTP;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MySUS.MvcFramework
{
    public static class Host
    {
        public static async Task CreateHostAsync(IMvcApplication application, int port=80)
        {
            List<Route> routingTable = new List<Route>();
            application.ConfigureServices();
            application.Configure(routingTable);
            IHttpServer server = new HttpServer(routingTable);

            await server.StartAsync(port);
        }
    }
}
