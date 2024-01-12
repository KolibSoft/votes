using System.Security.Cryptography;

namespace KolibSoft.Votes.Core;

public static class CryptoUtils
{

    public static byte[] Encrypt(this byte[] data, byte[] pubkey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(pubkey, out int readed);
        var edata = rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
        return edata;
    }

    public static byte[] Decrypt(this byte[] data, byte[] privkey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(privkey, out int readed);
        var ddata = rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA256);
        return ddata;
    }

    public static byte[] SignData(this byte[] data, byte[] privkey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(privkey, out int readed);
        var signature = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return signature;
    }

    public static bool VerifyData(this byte[] data, byte[] signature, byte[] pubkey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(pubkey, out int readed);
        var valid = rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return valid;
    }

    public static byte[] ComputeHash(this byte[] data)
    {
        var hash = SHA256.HashData(data);
        return hash;
    }

    public static byte[] SignHash(this byte[] hash, byte[] privkey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(privkey, out int readed);
        var signature = rsa.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return signature;
    }

    public static bool VerifyHash(this byte[] hash, byte[] signature, byte[] pubkey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(pubkey, out int readed);
        var valid = rsa.VerifyHash(hash, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return valid;
    }

}