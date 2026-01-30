namespace Docker.DotNet.X509;

[Obsolete("RSAUtil is obsolete. Use DockerTlsCertificates instead.")]
public static class RSAUtil
{
    public static X509Certificate2 GetCertFromPFX(string pfxFilePath, string password)
    {
#if NET9_0_OR_GREATER
        return X509CertificateLoader.LoadPkcs12FromFile(pfxFilePath, password);
#else
        return new X509Certificate2(pfxFilePath, password);
#endif
    }

    public static X509Certificate2 GetCertFromPEM(string certFilePath, string keyFilePath)
    {
#if NETSTANDARD
        return Polyfills.X509Certificate2.CreateFromPemFile(certFilePath, keyFilePath);
#elif NET9_0_OR_GREATER
        var certificate = X509Certificate2.CreateFromPemFile(certFilePath, keyFilePath);
        return OperatingSystem.IsWindows() ? X509CertificateLoader.LoadPkcs12(certificate.Export(X509ContentType.Pfx), null) : certificate;
#elif NET6_0_OR_GREATER
        var certificate = X509Certificate2.CreateFromPemFile(certFilePath, keyFilePath);
        return OperatingSystem.IsWindows() ? new X509Certificate2(certificate.Export(X509ContentType.Pfx)) : certificate;
#endif
    }
}