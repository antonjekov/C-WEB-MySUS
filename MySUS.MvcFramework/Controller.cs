using MySUS.HTTP;
using System.Runtime.CompilerServices;
using System.Text;

namespace MySUS.MvcFramework
{
    public abstract class Controller
    {
        public HttpResponse View ([CallerMemberName]string viewPath=null)
        {
            var layout = System.IO.File.ReadAllText("Views/Shared/_Layout.cshtml");

            var viewContent =System.IO.File.ReadAllText($"Views/{this.GetType().Name.Replace("Controller",string.Empty)}/{viewPath}.cshtml");
            var responseHtml = layout.Replace("@RenderBody()", viewContent);

            var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            var response = new HttpResponse(responseBodyBytes, "text/html");
            return response;
        }

        public HttpResponse File(string filePath, string contentType)
        {
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var response = new HttpResponse(fileBytes, contentType);
            return response;
        }

        public HttpResponse Redirect(string path)
        {
            var response = new HttpResponse(HttpStatusCode.Found);
            response.Headers.Add(new Header("Location", path));
            return response;
        }
    }
}
