﻿using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Commons
{
    public class RijndaelHelper
    {
        // Example usage: EncryptBytes(someFileBytes, "SensitivePhrase", "SodiumChloride");
        public static byte[] EncryptBytes(byte[] inputBytes, string passPhrase, string saltValue)
        {
            byte[] CipherBytes = null;
            try
            {
                RijndaelManaged RijndaelCipher = new RijndaelManaged();

                RijndaelCipher.Mode = CipherMode.CBC;
                byte[] salt = Encoding.ASCII.GetBytes(saltValue);
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, salt, "SHA1", 2);

                ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(password.GetBytes(32), password.GetBytes(16));

                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(inputBytes, 0, inputBytes.Length);
                cryptoStream.FlushFinalBlock();
                CipherBytes = memoryStream.ToArray();

                memoryStream.Close();
                cryptoStream.Close();
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "saltValue = " + saltValue);
            }
            return CipherBytes;
        }

        // Example usage: DecryptBytes(encryptedBytes, "SensitivePhrase", "SodiumChloride");
        public static byte[] DecryptBytes(byte[] encryptedBytes, string passPhrase, string saltValue)
        {
            byte[] plainBytes = null;
            try
            {
                RijndaelManaged RijndaelCipher = new RijndaelManaged();

                RijndaelCipher.Mode = CipherMode.CBC;
                byte[] salt = Encoding.ASCII.GetBytes(saltValue);
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, salt, "SHA1", 2);

                ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(password.GetBytes(32), password.GetBytes(16));

                MemoryStream memoryStream = new MemoryStream(encryptedBytes);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
                plainBytes = new byte[encryptedBytes.Length];

                int DecryptedCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

                memoryStream.Close();
                cryptoStream.Close();
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "saltValue = " + saltValue);
            }
            return plainBytes;
        }
    }
}