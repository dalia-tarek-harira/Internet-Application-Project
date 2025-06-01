namespace BookSwap.Models;
using System.Security.Cryptography;
using System.Text;

public static class EncryptionHelper
{
    private static readonly string Key = "ThisIsA32ByteLongSecretKey123456";
  

    public static string Encrypt(string plainText)
    {
        byte[] iv = new byte[16]; // Initialization Vector
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(Key);
        aes.IV = iv;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(string cipherText)
    {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(cipherText);

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(Key);
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(buffer);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }
}


