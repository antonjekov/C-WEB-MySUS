using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MySUS.HTTP
{
    public class HttpServer : IHttpServer
    {
        //private const string NewLine = "\r\n";
        //private const int BufferSize = 4096;
        IDictionary<string, Func<HttpRequest, HttpResponse>> routeTable = new Dictionary<string, Func<HttpRequest, HttpResponse>>();

        public void AddRoute(string path, Func<HttpRequest, HttpResponse> action)
        {
            if (routeTable.ContainsKey(path))
            {
                routeTable[path] = action;
            }
            else
            {
                routeTable.Add(path, action);
            }
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
                    if (this.routeTable.ContainsKey(httpRequest.Path))
                    {
                        var action = this.routeTable[httpRequest.Path];
                        response = action(httpRequest);
                    }
                    else
                    {
                        //path not found 404
                        response = new HttpResponse(new byte[0], "text/html", HttpStatusCode.NotFound);
                    }

                    response.Headers.Add(new Header("Server", "MySUSServer 1.0"));
                    response.Cookies.Add(new ResponseCookie("sid", Guid.NewGuid().ToString()) { HttpOnly = true, MaxAge = 60 * 24 * 60 * 60 });

                    var responseHeaderHttpBytes = Encoding.UTF8.GetBytes(response.ToString());

                    await stream.WriteAsync(responseHeaderHttpBytes, 0, responseHeaderHttpBytes.Length);
                    await stream.WriteAsync(response.Body, 0, response.Body.Length);

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
