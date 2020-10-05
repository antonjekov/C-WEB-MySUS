using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MySUS.HTTP
{
    public interface IHttpServer
    {
        void AddRoute(string path, Func<HttpRequest, HttpResponse> action);

        Task StartAsync(int port);
    }
}
