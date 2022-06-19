using Marlin.Core;
using System;

namespace Marlin.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var marlinBuilder = MarlinBuilder.Init().UseHttps().StartListen().HandleRequests().Build();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}
