
using MySUS.HTTP;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MySUS.MvcFramework
{
    public static class Host
    {
        public static async Task CreateHostAsync(List<Route> routingTable, int port=80)
        {
            IHttpServer server = new HttpServer(routingTable);

            await server.StartAsync(port);
        }
    }
}
