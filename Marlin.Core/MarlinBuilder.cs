using Marlin.Core.Entities;
using Marlin.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Marlin.Core
{
    public class MarlinBuilder
    {
        private bool _isSecure = false;
        private const string CertificateName = "Heyzon";
        private const string SafePassword = "HeyzonServerCert666%$";

        private readonly HttpListener _httpListener;
        private readonly MarlinServer _httpServer;

        private MarlinBuilder()
        {
            string environment = Environment.GetEnvironmentVariable("MARLIN_ENVIRONMENT");

            var configBuilder = new ConfigurationBuilder().AddJsonFile(!string.IsNullOrEmpty(environment) ? $"appsettings.{environment}.json" : "appsettings.json");
            var configuration = configBuilder.Build();

            var marlinConfiguration = configuration.GetSection("Marlin").Get<MarlinConfiguration>();

            if (marlinConfiguration == null || marlinConfiguration.JwtConfiguration == null)
            {
                throw new ArgumentNullException(nameof(marlinConfiguration));
            }

            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(marlinConfiguration);

            if (marlinConfiguration.EventLoggerEnabled)
            {
                services.AddSingleton<IEventHandler>();
            }

            var serviceProvider = services.BuildServiceProvider();

            _httpServer = new MarlinServer(serviceProvider);
            _httpListener = new HttpListener();
        }

        public static MarlinBuilder Init()
        {
            if (!HttpListener.IsSupported)
            {
                Console.Write(Messages.CurrentMachineDoesNotSupportsHttpListener);
            }

            return new MarlinBuilder();
        }

        public MarlinBuilder Build() => this;

        public MarlinBuilder StartListen()
        {
            _httpListener.Prefixes.Add($"http://localhost:8080/");

            if (_isSecure)
                _httpListener.Prefixes.Add($"https://localhost:8443/");

            _httpListener.Start();
            return this;
        }

        public MarlinBuilder HandleRequests()
        {
            _httpServer.HandleIncomingConnections(_httpListener).ConfigureAwait(false).GetAwaiter().GetResult();
            return this;
        }

        public MarlinBuilder StopListen()
        {
            _httpListener.Close();
            return this;
        }

        public MarlinBuilder UseHttps()
        {
            if (!this.HasCert())
            {
                var certificate = this.GenerateCert();

                AddCertToStore(certificate, StoreName.Root, StoreLocation.CurrentUser);
            }

            _isSecure = true;

            return this;
        }

        private X509Certificate2 GenerateCert()
        {
            var sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddIpAddress(IPAddress.Loopback);
            sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
            sanBuilder.AddDnsName("localhost");
            sanBuilder.AddDnsName(Environment.MachineName);


            var distinguishedName = new X500DistinguishedName($"CN={CertificateName}");

            using var rsa = RSA.Create(2048);
            var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            request.CertificateExtensions.Add(
                new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, false));


            request.CertificateExtensions.Add(
                new X509EnhancedKeyUsageExtension(
                    new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));

            request.CertificateExtensions.Add(sanBuilder.Build());

            var certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));
            certificate.FriendlyName = CertificateName;

            return new X509Certificate2(certificate.Export(X509ContentType.Pfx, SafePassword), SafePassword, X509KeyStorageFlags.MachineKeySet);
        }

        private static void AddCertToStore(X509Certificate2 cert, StoreName st, StoreLocation sl)
        {
            var store = new X509Store(st, sl);
            store.Open(OpenFlags.ReadWrite);
            store.Add(cert);

            store.Close();
        }

        private bool HasCert()
        {
            var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);

            store.Open(OpenFlags.ReadOnly);

            var certificates = store.Certificates.Find(
                X509FindType.FindBySubjectName,
                CertificateName,
                false);

            return certificates.Count > 0;
        }

    }
}
