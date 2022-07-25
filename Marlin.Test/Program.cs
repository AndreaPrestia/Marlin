using Marlin.Core;
using System;

namespace Marlin.Test
{
    static class Program
    {
        private static bool _keepRunning = true;

        static void Main()
        {
            MarlinBuilder marlinBuilder = null;

            try
            {
                Console.CancelKeyPress += delegate (object _, ConsoleCancelEventArgs e)
                {
                    e.Cancel = true;
                    _keepRunning = false;
                };

                marlinBuilder = MarlinBuilder.Init().StartListen().Build();

                while (_keepRunning) { }

                marlinBuilder.StopListen();
            }
            catch (Exception ex)
            {
                marlinBuilder?.StopListen();

                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}
