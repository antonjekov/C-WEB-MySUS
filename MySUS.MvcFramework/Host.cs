﻿
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MySUS.HTTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MySUS.MvcFramework
{
    public static class Host
    {
        public static async Task CreateHostAsync(IMvcApplication application, int port=80)
        {
            List<Route> routingTable = new List<Route>();
            IServiceCollection serviceCollection = new ServiceCollection();

            application.ConfigureServices(serviceCollection);
            application.Configure(routingTable);

            AutoRegisterStaticFiles(routingTable);
            AutoRegisterRoutes(routingTable, application,serviceCollection);

            Console.WriteLine("Registered routes:");
            foreach (var route in routingTable)
            {
                Console.WriteLine($"{route.Method} {route.Path}");
            }
            Console.WriteLine();
            Console.WriteLine("Requests:");
            IHttpServer server = new HttpServer(routingTable);

            await server.StartAsync(port);
        }

        private static void AutoRegisterRoutes(List<Route> routingTable, IMvcApplication application, IServiceCollection serviceCollection)
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
                    
                    routingTable.Add(new Route(url, httpMethod, (request) => ExecuteAction(request,controller, serviceCollection,method)));                    
                }
            }
        }

        private static HttpResponse ExecuteAction(HttpRequest request, Type controller, IServiceCollection serviceCollection, MethodInfo action)
        {
            var instance = serviceCollection.CreateInstance(controller) as Controller;
            instance.Request = request;
            var arguments = new List<object>();
            var parameters = action.GetParameters();
            foreach (var parameter in parameters)
            {
                var httpParameterValue = GetParameterFromRequest(request, parameter.Name);
                var parameterValue = Convert.ChangeType(httpParameterValue, parameter.ParameterType);
                if (parameterValue == null &&
                    parameter.ParameterType != typeof(string)
                    && parameter.ParameterType != typeof(int?))
                {
                    // complex type
                    parameterValue = Activator.CreateInstance(parameter.ParameterType);
                    var properties = parameter.ParameterType.GetProperties();
                    foreach (var property in properties)
                    {
                        var propertyHttpParamerValue = GetParameterFromRequest(request, property.Name);
                        var propertyParameterValue = Convert.ChangeType(propertyHttpParamerValue, property.PropertyType);
                        property.SetValue(parameterValue, propertyParameterValue);
                    }
                }
                arguments.Add(parameterValue);
            }

            var responce = action.Invoke(instance, arguments.ToArray()) as HttpResponse;
            return responce;
        }

        private static string GetParameterFromRequest(HttpRequest request, string parameterName)
        {
            parameterName = parameterName.ToLower();
            if (request.FormData.Any(x=>x.Key.ToLower()==parameterName))
            {
                return request.FormData.FirstOrDefault(x => x.Key.ToLower() == parameterName).Value;
            }
            if (request.QueryData.Any(x=>x.Key.ToLower()==parameterName))
            {
                return request.QueryData.FirstOrDefault(x => x.Key.ToLower() == parameterName).Value;
            }
            return null;
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
