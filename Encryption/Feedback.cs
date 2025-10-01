using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TuvVision.Encryption
{
    public class Feedback
    {
        public class Encryption
        {
            //static byte[] key = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            //static byte[] iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            public static string Encrypt(string plainText)
            {
                byte[] key = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
                byte[] iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

                Aes aes = Aes.Create();
                aes.Key = key;
                aes.IV = iv;

                byte[] encrypted;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);
                        cs.Write(plaintextBytes, 0, plaintextBytes.Length);
                        cs.Close();
                    }
                    encrypted = ms.ToArray();
                }

                return Convert.ToBase64String(encrypted);
            }

            public static string Decrypt(string cipherText)
            {
                byte[] key = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
                byte[] iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

                Aes aes = Aes.Create();
                aes.Key = key;
                aes.IV = iv;

                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                string decrypted;

                using (MemoryStream ms = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            decrypted = sr.ReadToEnd();
                        }
                    }
                }

                return decrypted;
            }
        }
        
    }
}