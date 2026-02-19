namespace Docker.DotNet.X509;

public sealed class DockerTlsCertificates
{
    private const string DefaultCaPemFileName = "ca.pem";

    private const string DefaultCertPemFileName = "cert.pem";

    private const string DefaultKeyPemFileName = "key.pem";

    public DockerTlsCertificates(
        X509Certificate2 certificate,
        X509Certificate2 certificateAuthorityCertificate = null)
    {
        Certificate = certificate ?? throw new ArgumentNullException(nameof(certificate));
        CertificateAuthorityCertificate = certificateAuthorityCertificate;
    }

    private X509Certificate2 Certificate { get; }

    private X509Certificate2 CertificateAuthorityCertificate { get; }

    public static DockerTlsCertificates LoadFromDirectory(
        string directoryPath,
        bool loadCertificateAuthority = true,
        string caPemFileName = DefaultCaPemFileName,
        string certPemFileName = DefaultCertPemFileName,
        string keyPemFileName = DefaultKeyPemFileName)
    {
        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException(directoryPath);
        }

        X509Certificate2 caCertificate = null;

        var certPemPath = Path.Combine(directoryPath, certPemFileName);
        var keyPemPath = Path.Combine(directoryPath, keyPemFileName);
        var caPemPath = Path.Combine(directoryPath, caPemFileName);

        var certificate = LoadCertificateFromPemFiles(certPemPath, keyPemPath);

        if (loadCertificateAuthority && File.Exists(caPemPath))
        {
            caCertificate = LoadCertificateAuthorityFromPemFile(caPemPath);
        }

        return new DockerTlsCertificates(certificate, caCertificate);
    }

    public static X509Certificate2 LoadCertificateFromPemFiles(string certPemPath, string keyPemPath)
    {
        EnsureFileExists(certPemPath);
        EnsureFileExists(keyPemPath);

#if NET9_0_OR_GREATER
        var certificate = X509Certificate2.CreateFromPemFile(certPemPath, keyPemPath);

        if (OperatingSystem.IsWindows())
        {
            var pfxBytes = certificate.Export(X509ContentType.Pfx);
            certificate.Dispose();
            return X509CertificateLoader.LoadPkcs12(pfxBytes, password: null);
        }

        return certificate;
#elif NET6_0_OR_GREATER
        var certificate = X509Certificate2.CreateFromPemFile(certPemPath, keyPemPath);

        if (OperatingSystem.IsWindows())
        {
            var pfxBytes = certificate.Export(X509ContentType.Pfx);
            certificate.Dispose();
            return new X509Certificate2(pfxBytes);
        }

        return certificate;
#elif NETSTANDARD
        return Polyfills.X509Certificate2.CreateFromPemFile(certPemPath, keyPemPath);
#else
        return X509Certificate2.CreateFromPemFile(certPemPath, keyPemPath);
#endif
    }

    public static X509Certificate2 LoadCertificateFromPfxFile(string pfxPath, string password)
    {
        EnsureFileExists(pfxPath);

        password ??= string.Empty;

#if NET9_0_OR_GREATER
        return X509CertificateLoader.LoadPkcs12FromFile(pfxPath, password);
#elif NETSTANDARD
        return new X509Certificate2(File.ReadAllBytes(pfxPath), password);
#else
        return new X509Certificate2(pfxPath, password, X509KeyStorageFlags.EphemeralKeySet);
#endif
    }

    public static X509Certificate2 LoadCertificateAuthorityFromPemFile(string caPemPath)
    {
        EnsureFileExists(caPemPath);

#if NET9_0_OR_GREATER
        return X509CertificateLoader.LoadCertificateFromFile(caPemPath);
#elif NETSTANDARD
        return Polyfills.X509Certificate2.CreateFromPemFile(caPemPath);
#else
        return new X509Certificate2(caPemPath);
#endif
    }

    public IAuthProvider CreateCredentials()
    {
        var credentials = new CertificateCredentials(Certificate);

        if (CertificateAuthorityCertificate is not null)
        {
            credentials.ServerCertificateValidationCallback = CreateCertificateAuthorityValidationCallback(CertificateAuthorityCertificate);
        }

        return credentials;
    }

    public static RemoteCertificateValidationCallback CreateCertificateAuthorityValidationCallback(X509Certificate2 certificateAuthorityCertificate)
    {
        if (certificateAuthorityCertificate is null)
        {
            throw new ArgumentNullException(nameof(certificateAuthorityCertificate));
        }

        return (_, certificate, _, _) =>
        {
            if (certificate is null)
            {
                return false;
            }

            var serverCertificate2 = certificate as X509Certificate2 ?? new X509Certificate2(certificate);

            using var chain = new X509Chain();
            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;

#if NET5_0_OR_GREATER
            chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
            chain.ChainPolicy.CustomTrustStore.Add(certificateAuthorityCertificate);
            return chain.Build(serverCertificate2);
#else
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
            chain.ChainPolicy.ExtraStore.Add(certificateAuthorityCertificate);

            if (!chain.Build(serverCertificate2))
            {
                return false;
            }

            foreach (var chainElement in chain.ChainElements)
            {
                if (string.Equals(chainElement.Certificate.Thumbprint, certificateAuthorityCertificate.Thumbprint, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
#endif
        };
    }

    private static void EnsureFileExists(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(path);
        }
    }
}