using MySUS.HTTP;
using MySUS.MvcFramework.ViewEngine;
using System.Runtime.CompilerServices;
using System.Text;

namespace MySUS.MvcFramework
{
    public abstract class Controller
    {
        private SusViewEngine viewEngine;

        public Controller()
        {
            this.viewEngine = new SusViewEngine();
        }

        public HttpRequest Request { get; set; }

        public HttpResponse View (object viewModel=null, [CallerMemberName]string viewPath=null )
        {
            var layout = System.IO.File.ReadAllText("Views/Shared/_Layout.cshtml");
            layout = layout.Replace("@RenderBody()", "__VIEW_GOES_HERE__");
            layout = this.viewEngine.GetHtml(layout,viewModel);

            var viewContent =System.IO.File.ReadAllText($"Views/{this.GetType().Name.Replace("Controller",string.Empty)}/{viewPath}.cshtml");
            viewContent = this.viewEngine.GetHtml(viewContent,viewModel);
            var responseHtml = layout.Replace("__VIEW_GOES_HERE__", viewContent);

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
