using System;
using System.Security.Cryptography;
using System.Text;

namespace Utils.String
{
    public static class StringExtensions
    {
        public static string Decrypt(this string data, string key)
        {
            var rijndaelCipher = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 0x80,
                BlockSize = 0x80
            };

            var encryptedData = Convert.FromBase64String(data);
            var pwdBytes = Encoding.UTF8.GetBytes(key);
            var keyBytes = new byte[0x10];
            var len = pwdBytes.Length;

            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }

            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            var plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);

            return Encoding.UTF8.GetString(plainText);
        }

        public static string Encrypt(this string data, string key)
        {
            var rijndaelCipher = new RijndaelManaged
            {
                Mode = CipherMode.CBC,          //remember this parameter
                Padding = PaddingMode.PKCS7,    //remember this parameter
                KeySize = 0x80,
                BlockSize = 0x80
            };

            var pwdBytes = Encoding.UTF8.GetBytes(key);
            var keyBytes = new byte[0x10];
            var len = pwdBytes.Length;

            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }

            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            var transform = rijndaelCipher.CreateEncryptor();
            var plainText = Encoding.UTF8.GetBytes(data);

            return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
        }
    }
}