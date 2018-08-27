using System;
using System.Globalization;
using System.Text;
using System.Security.Cryptography;

namespace Commons
{
    public static class HashHelpers
    {
        public static string HashEncode(string code)
        {
            return BitConverter.ToString(SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(code)));
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            char[] text = new char[length];
            for (int i = 0; i < length; i++)
            {
                text[i] = characters[random.Next(characters.Length)];
            }
            return new string(text);
        }

    }
}
