using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Marlin.Core
{
    public class MarlinBuilder 
    {
        private readonly MarlinServer _marlinServer;

        private MarlinBuilder([NotNull] IServiceCollection serviceDescriptors)
        {
            var serviceProvider = serviceDescriptors.AddMarlinServices();

            _marlinServer = new MarlinServer(serviceProvider);
        }

        public static MarlinBuilder Init()
        {
            var serviceCollection = new ServiceCollection();

            return new MarlinBuilder(serviceCollection);
        }

        public MarlinBuilder Build() => this;

        public MarlinBuilder StartListen()
        {
            _marlinServer.RunServer();

            return this;
        }

        public MarlinBuilder StopListen()
        {
            _marlinServer.StopServer();
            return this;
        }
    }
}
