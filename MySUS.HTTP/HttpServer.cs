using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MySUS.HTTP
{
    public class HttpServer : IHttpServer
    {
        private List<Route> routeTable;

        //private const string NewLine = "\r\n";
        //private const int BufferSize = 4096;

        public HttpServer(List<Route> routeTable)
        {
            this.routeTable = routeTable;
        }

        public async Task StartAsync(int port)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, port);
            tcpListener.Start();
            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                ProcessClientAsync(tcpClient);
            }
        }

        private async Task ProcessClientAsync(TcpClient tcpClient)
        {
            try
            {
                using (NetworkStream stream = tcpClient.GetStream())
                {
                    int position = 0;
                    byte[] buffer = new byte[HttpConstants.BufferSize];
                    List<byte> data = new List<byte>();
                    while (true)
                    {
                        int count = await stream.ReadAsync(buffer, position, buffer.Length);
                        position += count;

                        if (count < buffer.Length)
                        {
                            var partialBuffer = new byte[count];
                            Array.Copy(buffer, partialBuffer, count);
                            data.AddRange(partialBuffer);
                            break;
                        }
                        else
                        {
                            data.AddRange(buffer);
                        }

                    }

                    string requestAsString = Encoding.UTF8.GetString(data.ToArray());
                    HttpRequest httpRequest = new HttpRequest(requestAsString);
                    Console.WriteLine($"{httpRequest.Method} {httpRequest.Path} => {httpRequest.Headers.Count} {httpRequest.Cookies.Count}");

                    //RESPONSE
                    HttpResponse response;
                    var route = routeTable.FirstOrDefault(r => string.Compare(r.Path, httpRequest.Path, true)==0&& r.Method==httpRequest.Method);
                    if (route!=null)
                    {
                        response = route.Action(httpRequest);
                    }
                    else
                    {
                        //path not found 404
                        response = new HttpResponse(new byte[0], "text/html", HttpStatusCode.NotFound);
                    }

                    response.Headers.Add(new Header("Server", "MySUSServer 1.0"));
                    var sessionCookie = httpRequest.Cookies.FirstOrDefault(cookie => cookie.Name == HttpConstants.SessionCookieName);
                    if (sessionCookie!=null)
                    {
                        var responceSessionCookie = new ResponseCookie(sessionCookie.Name, sessionCookie.Value);
                        responceSessionCookie.Path = "/";
                        response.Cookies.Add(responceSessionCookie);
                    }

                    var responseHeaderHttpBytes = Encoding.UTF8.GetBytes(response.ToString());

                    await stream.WriteAsync(responseHeaderHttpBytes, 0, responseHeaderHttpBytes.Length);
                    if (response.Body!=null)
                    {
                    await stream.WriteAsync(response.Body, 0, response.Body.Length);

                    }

                }
                tcpClient.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
