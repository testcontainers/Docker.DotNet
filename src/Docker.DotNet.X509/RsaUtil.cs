namespace Docker.DotNet.X509;

public static class RsaUtil
{
    public static X509Certificate2 GetCertFromPfx(string pfxFilePath, string password)
    {
#if NET9_0_OR_GREATER
        return X509CertificateLoader.LoadPkcs12FromFile(pfxFilePath, password);
#else
        return new X509Certificate2(pfxFilePath, password);
#endif
    }

    public static X509Certificate2 GetCertFromPem(string certFilePath, string keyFilePath)
    {
#if NETSTANDARD
        return Polyfills.X509Certificate2.CreateFromPemFile(certFilePath, keyFilePath);
#else
        return OperatingSystem.IsWindows() ? CreateWindowsCert(certFilePath, keyFilePath) : X509Certificate2.CreateFromPemFile(certFilePath, keyFilePath);
#endif
    }

#if !NETSTANDARD
    private static X509Certificate2 CreateWindowsCert(string certFilePath, string keyFilePath)
    {
        using var cert = X509Certificate2.CreateFromPemFile(certFilePath, keyFilePath);

#if NET9_0_OR_GREATER
        return X509CertificateLoader.LoadPkcs12(cert.Export(X509ContentType.Pfx), null);
#else
        return new X509Certificate2(cert.Export(X509ContentType.Pfx));
#endif
    }
#endif
}