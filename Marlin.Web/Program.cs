using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marlin.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
          var builder = WebApplication.CreateBuilder(args);

          var app = builder.Build();
           
          app.MapGet("/", () => "Hello World!");
          
          app.Run();
        }
    }
}
