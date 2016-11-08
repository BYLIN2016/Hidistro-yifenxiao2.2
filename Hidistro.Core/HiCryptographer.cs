namespace Hidistro.Core
{
    using System;
    using System.Configuration;
    using System.Security.Cryptography;
    using System.Text;

    public sealed class HiCryptographer
    {
        public static string CreateHash(string plaintext)
        {
            byte[] buffer = CreateHash(Encoding.ASCII.GetBytes(plaintext));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("x2"));
            }
            return builder.ToString();
        }

        private static byte[] CreateHash(byte[] plaintext)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            return provider.ComputeHash(plaintext);
        }

        public static string Decrypt(string text)
        {
            using (RijndaelManaged managed = new RijndaelManaged())
            {
                managed.Key = Convert.FromBase64String(ConfigurationManager.AppSettings["Key"]);
                managed.IV = Convert.FromBase64String(ConfigurationManager.AppSettings["IV"]);
                ICryptoTransform transform = managed.CreateDecryptor();
                byte[] inputBuffer = Convert.FromBase64String(text);
                byte[] bytes = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
                transform.Dispose();
                return Encoding.UTF8.GetString(bytes);
            }
        }

        public static string Encrypt(string text)
        {
            using (RijndaelManaged managed = new RijndaelManaged())
            {
                managed.Key = Convert.FromBase64String(ConfigurationManager.AppSettings["Key"]);
                managed.IV = Convert.FromBase64String(ConfigurationManager.AppSettings["IV"]);
                ICryptoTransform transform = managed.CreateEncryptor();
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                byte[] inArray = transform.TransformFinalBlock(bytes, 0, bytes.Length);
                transform.Dispose();
                return Convert.ToBase64String(inArray);
            }
        }
    }
}

