using System;
using System.Security.Cryptography;
using System.Text;

namespace Go.Helpers
{
    public class Encription
    {
        public static string Encrypt(string text)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));

            return Convert.ToBase64String(bytes);
        }
    }
}