using MySUS.HTTP;
using MySUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyFirstMvcAPP.Controllers
{
    public class StaticFilesController: Controller
    {
        public HttpResponse Favicon(HttpRequest request)
        {
            return this.File("wwwroot/favicon.ico", "image/x-icon");
        }

        internal HttpResponse BootstrapCss(HttpRequest arg)
        {
            return this.File("wwwroot/css/bootstrap.min.css", "text/css");
        }

        internal HttpResponse CustomCss(HttpRequest arg)
        {
            return this.File("wwwroot/css/custom.css", "text/css");
        }

        internal HttpResponse CustomJs(HttpRequest arg)
        {
            return this.File("wwwroot/js/custom.js", "text/javascript");
        }

        internal HttpResponse BootstrapJs(HttpRequest arg)
        {
            return this.File("wwwroot/js/bootstrap.bundle.min.js", "text/javascript");
        }
    }
}
