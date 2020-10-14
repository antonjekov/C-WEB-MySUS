using MySUS.HTTP;
using MySUS.MvcFramework.ViewEngine;
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace MySUS.MvcFramework
{
    public abstract class Controller
    {
        private const string UserIdSessionName = "UserId";
        private SusViewEngine viewEngine;

        public Controller()
        {
            this.viewEngine = new SusViewEngine();
        }

        public HttpRequest Request { get; set; }

        protected HttpResponse Error(string errorText)
        {
            var viewContent = $"<div class=\"alert alert-danger\" role=\"alert\">{errorText}</div>";
            string responseHtml = PutViewInLayout( viewContent);

            var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            var response = new HttpResponse(responseBodyBytes, "text/html", HttpStatusCode.UnprocessableEntity);
            return response;
        }

        protected HttpResponse View (object viewModel=null, [CallerMemberName]string viewPath=null )
        {
            var viewContent = System.IO.File.ReadAllText($"Views/{this.GetType().Name.Replace("Controller", string.Empty)}/{viewPath}.cshtml");
            viewContent = this.viewEngine.GetHtml(viewContent, viewModel, this.GetUserId());

            string responseHtml = PutViewInLayout(viewContent, viewModel);

            var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            var response = new HttpResponse(responseBodyBytes, "text/html");
            return response;
        }

        protected string PutViewInLayout(string viewContent, object viewModel = null)
        {
            var layout = System.IO.File.ReadAllText("Views/Shared/_Layout.cshtml");
            layout = layout.Replace("@RenderBody()", "__VIEW_GOES_HERE__");
            layout = this.viewEngine.GetHtml(layout, viewModel, this.GetUserId());
            var responseHtml = layout.Replace("__VIEW_GOES_HERE__", viewContent);
            return responseHtml;
        }

        protected HttpResponse File(string filePath, string contentType)
        {
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var response = new HttpResponse(fileBytes, contentType);
            return response;
        }

        protected HttpResponse Redirect(string path)
        {
            var response = new HttpResponse(HttpStatusCode.Found);
            response.Headers.Add(new Header("Location", path));
            return response;
        }

        protected void SignIn(string userId)
        {
            this.Request.Session[UserIdSessionName] = userId;
        }

        protected void SignOut()
        {
            this.Request.Session[UserIdSessionName] = null;
        }

        protected bool IsUserSignedIn()=> this.Request.Session.ContainsKey(UserIdSessionName)&& this.Request.Session[UserIdSessionName] != null;
       

        protected string GetUserId()=> this.Request.Session.ContainsKey(UserIdSessionName)? this.Request.Session[UserIdSessionName]:null;
        
    }
}
