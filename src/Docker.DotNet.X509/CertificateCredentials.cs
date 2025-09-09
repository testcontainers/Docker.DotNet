using System;
using System.Collections.Generic;
using System.Security.Authentication;

namespace Docker.DotNet.X509;

public class CertificateCredentials : Credentials
{
    private readonly X509Certificate2 _certificate;

    public CertificateCredentials(X509Certificate2 certificate)
    {
        _certificate = certificate;
    }

    public RemoteCertificateValidationCallback ServerCertificateValidationCallback { get; set; }

    public override void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public override bool IsTlsCredentials()
    {
        return true;
    }

    public override HttpMessageHandler GetHandler(HttpMessageHandler handler)
    {
        if (handler is ManagedHandler managedHandler)
        {
            if (!managedHandler.ClientCertificates.Contains(_certificate))
            {
                managedHandler.ClientCertificates.Add(_certificate);
            }

            managedHandler.ServerCertificateValidationCallback = ServerCertificateValidationCallback;

            return managedHandler;
        }

#if NET6_0_OR_GREATER
        if (handler is SocketsHttpHandler nativeHandler)
#else
        if (handler is HttpClientHandler nativeHandler)
#endif
        {
            if (!nativeHandler.ClientCertificates.Contains(_certificate))
            {
                nativeHandler.ClientCertificates.Add(_certificate);
            }

            nativeHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            nativeHandler.CheckCertificateRevocationList = false;
            nativeHandler.UseProxy = true;
            nativeHandler.AllowAutoRedirect = true;
            nativeHandler.MaxAutomaticRedirections = 20;
            nativeHandler.Proxy = WebRequest.DefaultWebProxy;
            nativeHandler.SslProtocols = SslProtocols.Tls12;
            nativeHandler.ServerCertificateCustomValidationCallback += (message, certificate, chain, errors) => ServerCertificateValidationCallback?.Invoke(message, certificate, chain, errors) ?? false;

            return nativeHandler;
        }

        return handler;
    }

    protected virtual void Dispose(bool disposing)
    {
    }
}