using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Marlin.Core.Entities;
using Marlin.Core.Interfaces;

namespace Marlin.Core;

public sealed class SslTcpServer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly MarlinConfiguration _configuration;
    private readonly IEventHandler _eventHandler;
    private X509Certificate2 _serverCertificate;
    private readonly string _certificateName;
    private readonly string _safePassword;

    public SslTcpServer([NotNull] IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        _configuration = (MarlinConfiguration)_serviceProvider.GetService(typeof(MarlinConfiguration));

        _certificateName = _configuration?.CertificateName ?? "Marlin";
        _safePassword = _configuration?.CertificateKey ?? "Marlin666%$";

        if (_configuration?.JwtConfiguration == null)
        {
            throw new ArgumentNullException(nameof(_configuration.JwtConfiguration));
        }

        if (_configuration.EventLoggerEnabled)
        {
            _eventHandler = (IEventHandler)serviceProvider.GetService(typeof(IEventHandler));
        }
    }


    // The certificate parameter specifies the name of the file
    // containing the machine certificate.
    public void RunServer()
    {
        if (!HasCert())
        {
            _serverCertificate = GenerateCert();
            AddCertToStore(_serverCertificate, StoreName.Root, StoreLocation.CurrentUser);
        }
        else
        {
            _serverCertificate = GetCert();
        }
        
        // Create a TCP/IP (IPv4) socket and listen for incoming connections.
        var listener = new TcpListener(IPAddress.Any, 8080);
        listener.Start();

        while (true)
        {
            Console.WriteLine("Waiting for a client to connect...");
            // Application blocks while waiting for an incoming connection.
            // Type CNTL-C to terminate the server.
            var client = listener.AcceptTcpClient();
            ProcessClient(client);
        }
    }

    private X509Certificate2 GenerateCert()
    {
        var sanBuilder = new SubjectAlternativeNameBuilder();
        sanBuilder.AddIpAddress(IPAddress.Loopback);
        sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
        sanBuilder.AddDnsName("localhost");
        sanBuilder.AddDnsName(Environment.MachineName);

        var distinguishedName = new X500DistinguishedName($"CN={_certificateName}");

        using var rsa = RSA.Create(2048);
        var request =
            new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        request.CertificateExtensions.Add(
            new X509KeyUsageExtension(
                X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment |
                X509KeyUsageFlags.DigitalSignature, false));


        request.CertificateExtensions.Add(
            new X509EnhancedKeyUsageExtension(
                new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));

        request.CertificateExtensions.Add(sanBuilder.Build());

        var certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)),
            new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));

        return new X509Certificate2(certificate.Export(X509ContentType.Pfx, _safePassword), _safePassword,
            X509KeyStorageFlags.MachineKeySet);
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
            _certificateName,
            false);

        return certificates.Count > 0;
    }
    
    private X509Certificate2 GetCert()
    {
        var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);

        store.Open(OpenFlags.ReadOnly);

        var certificates = store.Certificates.Find(
            X509FindType.FindBySubjectName,
            _certificateName,
            false);

        return certificates.MaxBy(x => x.NotAfter);
    }
    
    void ProcessClient(TcpClient client)
    {
        // A client has connected. Create the
        // SslStream using the client's network stream.
        SslStream sslStream = new SslStream(
            client.GetStream(), false);
        // Authenticate the server but don't require the client to authenticate.
        try
        {
            sslStream.AuthenticateAsServer(_serverCertificate, clientCertificateRequired: false,
                checkCertificateRevocation: true);

            // Set timeouts for the read and write to 5 seconds.
            sslStream.ReadTimeout = 5000;
            sslStream.WriteTimeout = 5000;
            // Read a message from the client.
            Console.WriteLine("Waiting for client message...");
            string messageData = ReadMessage(sslStream);
            Console.WriteLine("Received: {0}", messageData);

            // Write a message to the client.
            byte[] message = Encoding.UTF8.GetBytes("Hello from the server.<EOF>");
            Console.WriteLine("Sending hello message.");
            sslStream.Write(message);
        }
        catch (AuthenticationException e)
        {
            Console.WriteLine("Exception: {0}", e.Message);
            if (e.InnerException != null)
            {
                Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
            }

            Console.WriteLine("Authentication failed - closing the connection.");
            sslStream.Close();
            client.Close();
            return;
        }
        finally
        {
            // The client stream will be closed with the sslStream
            // because we specified this behavior when creating
            // the sslStream.
            sslStream.Close();
            client.Close();
        }
    }

    static string ReadMessage(SslStream sslStream)
    {
        // Read the  message sent by the client.
        // The client signals the end of the message using the
        // "<EOF>" marker.
        byte[] buffer = new byte[2048];
        StringBuilder messageData = new StringBuilder();
        int bytes = -1;
        do
        {
            // Read the client's test message.
            bytes = sslStream.Read(buffer, 0, buffer.Length);

            // Use Decoder class to convert from bytes to UTF8
            // in case a character spans two buffers.
            Decoder decoder = Encoding.UTF8.GetDecoder();
            char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
            decoder.GetChars(buffer, 0, bytes, chars, 0);
            messageData.Append(chars);
            // Check for EOF or an empty message.
            if (messageData.ToString().IndexOf("<EOF>") != -1)
            {
                break;
            }
        } while (bytes != 0);

        return messageData.ToString();
    }
}