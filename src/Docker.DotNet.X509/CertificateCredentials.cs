namespace Docker.DotNet.X509;

public class CertificateCredentials : Credentials
{
    private readonly X509Certificate2 _certificate;

    private bool _disposed;

    public CertificateCredentials(X509Certificate2 certificate)
    {
        _certificate = certificate;
    }

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
        if (handler is HttpClientHandler httpClientHandler)
        {
            httpClientHandler.ClientCertificates.Add(_certificate);
            httpClientHandler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
        }

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