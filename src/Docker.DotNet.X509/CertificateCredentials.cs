namespace Docker.DotNet.X509;

public class CertificateCredentials : Credentials
{
    private readonly X509Certificate2 _certificate;

    private bool _disposed;

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
        if (handler is not ManagedHandler managedHandler)
        {
            return handler;
        }

        if (!managedHandler.ClientCertificates.Contains(_certificate))
        {
            managedHandler.ClientCertificates.Add(_certificate);
        }

        managedHandler.ServerCertificateValidationCallback = ServerCertificateValidationCallback;

        return handler;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _certificate.Dispose();
        }

        _disposed = true;
    }
}