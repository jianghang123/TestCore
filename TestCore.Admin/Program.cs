using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TestCore.Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var host = WebHost.CreateDefaultBuilder(args)
            //                .UseUrls("http://*:8001")
            //                .UseKestrel(options => options.AddServerHeader = false)
            //                .UseStartup<Startup>()
            //                .Build();
            //host.Run();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
