using System;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new HttpSelfHostConfiguration("http://localhost:15300");
            config.Routes.MapHttpRoute("API Default", "api/{controller}/{id}", new {id = RouteParameter.Optional});
            using (var server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Press Enter to stop service and quit...");
                Console.ReadKey();
            }
        }
    }
}
