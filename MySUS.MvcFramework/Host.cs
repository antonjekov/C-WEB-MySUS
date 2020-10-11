
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MySUS.HTTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MySUS.MvcFramework
{
    public static class Host
    {
        public static async Task CreateHostAsync(IMvcApplication application, int port=80)
        {
            List<Route> routingTable = new List<Route>();
            AutoRegisterStaticFiles(routingTable);
            AutoRegisterRoutes(routingTable, application);

            application.ConfigureServices();
            application.Configure(routingTable);

            Console.WriteLine("All registered routes:");
            foreach (var route in routingTable)
            {
                Console.WriteLine($"{route.Method} {route.Path}");
            }

            IHttpServer server = new HttpServer(routingTable);

            await server.StartAsync(port);
        }

        private static void AutoRegisterRoutes(List<Route> routingTable, IMvcApplication application)
        {
            //routeTable.Add(new Route("/users/login", HttpMethod.GET, new UsersController().Login));
            var controllerTypes = application.GetType().Assembly.GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(Controller)));
            foreach (var controller in controllerTypes)
            {
                var methods = controller.GetMethods()
                    .Where(x=>x.IsPublic && !x.IsStatic && x.DeclaringType==controller && !x.IsAbstract && !x.IsConstructor && !x.IsSpecialName);
                foreach (var method in methods)
                {
                    var url = "/" + controller.Name.Replace("Controller", string.Empty) + "/" + method.Name;

                    var attribute = method.GetCustomAttributes(false)
                        .Where(x=>x.GetType().IsSubclassOf(typeof(BaseHttpAttribute)))
                        .FirstOrDefault() as BaseHttpAttribute;

                    var httpMethod = HttpMethod.GET;

                    if (!string.IsNullOrEmpty(attribute?.Url))
                    {
                        url = attribute.Url;
                    }
                    if (attribute!=null)
                    {
                        httpMethod = attribute.Method;
                    }
                    
                    routingTable.Add(new Route(url, httpMethod, (request) => 
                    {
                        var instance = Activator.CreateInstance(controller) as Controller;
                        instance.Request = request;
                        var responce = method.Invoke(instance, new object[] { } ) as HttpResponse;
                        return responce;
                    }));
                    
                }
            }

        }

        private static void AutoRegisterStaticFiles(List<Route> routingTable)
        {
            var staticFiles = Directory.GetFiles("wwwroot", "*", SearchOption.AllDirectories);
            foreach (var staticFile in staticFiles)
            {
                var url = staticFile.Replace("wwwroot", string.Empty).Replace("\\", "/");
                routingTable.Add(new Route(url, HttpMethod.GET, (request) =>
                {
                    var fileContent = File.ReadAllBytes(staticFile);
                    var fileExt = new FileInfo(staticFile).Extension;
                    var contentType = fileExt switch
                    {
                        ".txt" => "text/plain",
                        ".js" => "text/javascript",
                        ".css" => "text/css",
                        ".jpg" => "image/jpeg",
                        ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        ".gif" => "image/gif",
                        ".ico" => "image/x-icon",
                        ".html" => "text/html",
                        _ => "text/plain"

                    };

                    var httpResponse = new HttpResponse(fileContent, contentType, HttpStatusCode.Ok);
                    return httpResponse;
                }));
            }
        }
    }
}
