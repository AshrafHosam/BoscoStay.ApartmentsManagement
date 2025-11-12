using Application.Contracts.Helpers;
using System.Security.Cryptography;
using System.Text;

namespace Persistence.Implementation.Helpers
{
    internal class EncryptionHelper : IEncryptionHelper
    {
        public string Decrypt(string input, string key)
        {
            try
            {
                var fullCipher = Convert.FromBase64String(input);

                using var aes = Aes.Create();
                aes.Key = GetAesKey(key);

                var iv = new byte[aes.BlockSize / 8];
                Array.Copy(fullCipher, iv, iv.Length);
                aes.IV = iv;

                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using var ms = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length);
                using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using var sr = new StreamReader(cs);

                return sr.ReadToEnd();
            }
            catch
            {
                return input;
            }
        }

        public string Encrypt(string input, string key)
        {
            try
            {
                using var aes = Aes.Create();
                aes.Key = GetAesKey(key);
                aes.GenerateIV();

                using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using var ms = new MemoryStream();
                ms.Write(aes.IV, 0, aes.IV.Length); // prepend IV

                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(input);
                }

                return Convert.ToBase64String(ms.ToArray());
            }
            catch
            {
                return input;
            }
        }

        private static byte[] GetAesKey(string key)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
        }

    }
}
