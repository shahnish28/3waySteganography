using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageSteganographyConsole.Models
{
    public class Encryption
    {
        private static readonly int KeySize = 32; // AES-256
        private static readonly int IvSize = 16;  // AES Block Size

        public static string Encrypt(string plainText, string key)
        {
            byte[] keyBytes = GenerateKey(key);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.GenerateIV(); // Generate a random IV for each encryption
                byte[] iv = aes.IV;

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, iv))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                    // Combine IV and encrypted data
                    byte[] combinedBytes = new byte[iv.Length + encryptedBytes.Length];
                    Array.Copy(iv, 0, combinedBytes, 0, iv.Length);
                    Array.Copy(encryptedBytes, 0, combinedBytes, iv.Length, encryptedBytes.Length);

                    return Convert.ToBase64String(combinedBytes);
                }
            }
        }

        public static string Decrypt(string encryptedText, string key)
        {
            try
            {
                if (!IsValidBase64(encryptedText))
                    throw new FormatException("Invalid Base64 input!");

                byte[] keyBytes = GenerateKey(key);
                byte[] combinedBytes = Convert.FromBase64String(encryptedText);

                if (combinedBytes.Length < IvSize)
                    throw new ArgumentException("Invalid encrypted text!");

                byte[] iv = new byte[IvSize];
                byte[] cipherBytes = new byte[combinedBytes.Length - IvSize];

                Array.Copy(combinedBytes, 0, iv, 0, IvSize);
                Array.Copy(combinedBytes, IvSize, cipherBytes, 0, cipherBytes.Length);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.IV = iv;

                    using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    {
                        try
                        {
                            byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                            return Encoding.UTF8.GetString(decryptedBytes);
                        }
                        catch (CryptographicException)
                        {
                            return "Error: Incorrect decryption key!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Decryption failed: " + ex.Message;
            }
        }


        private static byte[] GenerateKey(string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(keyBytes);
            }
        }

        public static bool IsValidBase64(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return false;

            base64String = base64String.Trim();
            return (base64String.Length % 4 == 0) && Regex.IsMatch(base64String, "^[A-Za-z0-9+/]*={0,2}$");
        }
    }
}
