namespace Docker.DotNet.X509;

public class CertificateCredentials : Credentials
{
    private readonly X509Certificate2 _certificate;

    public CertificateCredentials(X509Certificate2 clientCertificate)
    {
        _certificate = clientCertificate;
    }

    public RemoteCertificateValidationCallback ServerCertificateValidationCallback { get; set; }

    public override HttpMessageHandler GetHandler(HttpMessageHandler innerHandler)
    {
        var handler = (ManagedHandler)innerHandler;
        handler.ClientCertificates = new X509CertificateCollection
        {
            _certificate
        };

        handler.ServerCertificateValidationCallback = ServerCertificateValidationCallback;

        if (handler.ServerCertificateValidationCallback == null)
        {
            handler.ServerCertificateValidationCallback = ServicePointManager.ServerCertificateValidationCallback;
        }

        return handler;
    }

    public override bool IsTlsCredentials()
    {
        return true;
    }

    public override void Dispose()
    {
    }
}