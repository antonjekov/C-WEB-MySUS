using MySUS.HTTP;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MySUS.MvcFramework
{
    public static class Host
    {
        public static async Task CreateHostAsync(List<Route> routingTable, int port=80)
        {
            IHttpServer server = new HttpServer();

            foreach (var route in routingTable)
            {
                server.AddRoute(route.Path, route.Action);
            }
            await server.StartAsync(port);
        }
    }
}
