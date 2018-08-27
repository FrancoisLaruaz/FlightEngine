using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Encrypt
{
    public static class EncryptHelper
    {

        private static byte[] Key = { 147, 144, 173, 93, 196, 214, 94, 197, 114, 141, 71, 186, 131, 64, 53, 17, 11, 41, 14, 111, 79, 45, 46, 157, 174, 44, 101, 77, 242, 219, 47, 37 };
        private static byte[] Vector = { 144, 1, 189, 79, 37, 64, 111, 114, 14, 4, 117, 119, 115, 177, 231, 255 };

        private static ICryptoTransform EncryptorTransform, DecryptorTransform;
        private static System.Text.UTF8Encoding UTFEncoder;

        private static readonly object _locker_encrypt = new object();
        private static readonly object _locker_decrypt = new object();

        static EncryptHelper()
        {
            //This is our encryption method 
            RijndaelManaged rm = new RijndaelManaged();
            rm.Padding = PaddingMode.PKCS7;
            //Create an encryptor and a decryptor using our encryption method, key, and vector. 
            EncryptorTransform = rm.CreateEncryptor(Key, Vector);
            DecryptorTransform = rm.CreateDecryptor(Key, Vector);

            //Used to translate bytes to text and vice versa 
            UTFEncoder = new System.Text.UTF8Encoding();
        }



        /// <summary>
        /// Encrypt some text and return a string suitable for passing in a URL. 
        /// </summary>
        /// <param name="TextValue"></param>
        /// <returns></returns>
        public static string EncryptToString(string TextValue)
        {
            try
            {
                return ByteArrToString(Encrypt(TextValue));
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "TextValue = " + TextValue);
                return "";
            }
        }

        /// <summary>
        /// Encrypt some text and return an encrypted byte array. 
        /// </summary>
        /// <param name="TextValue"></param>
        /// <returns></returns>
        private static byte[] Encrypt(string TextValue)
        {
            byte[] encrypted = null;

            try
            {
                if (TextValue == null)
                    return null;
                lock (_locker_encrypt)
                {
                    //Translates our text value into a byte array. 
                    Byte[] bytes = UTFEncoder.GetBytes(TextValue);

                    //Used to stream the data in and out of the CryptoStream. 
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        /* 
                         * We will have to write the unencrypted bytes to the stream, 
                         * then read the encrypted result back from the stream. 
                         */
                        #region Write the decrypted value to the encryption stream
                        using (CryptoStream cs = new CryptoStream(memoryStream, EncryptorTransform, CryptoStreamMode.Write))
                        {
                            cs.Write(bytes, 0, bytes.Length);
                            cs.FlushFinalBlock();
                            #endregion

                            #region Read encrypted value back out of the stream
                            memoryStream.Position = 0;
                            encrypted = new byte[memoryStream.Length];
                            memoryStream.Read(encrypted, 0, encrypted.Length);
                            #endregion

                            //Clean up. 
                            cs.Close();
                            memoryStream.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "TextValue = " + TextValue);
            }
            return encrypted;
        }

        /// The other side: Decryption methods 
        public static string DecryptString(string EncryptedString)
        {
            try
            {
                return Decrypt(StrToByteArray(EncryptedString));
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EncryptedString = " + EncryptedString);
                return null;
            }
        }

        /// Decryption when working with byte arrays.     
        private static string Decrypt(byte[] EncryptedValue)
        {
            try
            {
                if (EncryptedValue == null)
                    return null;
                lock (_locker_decrypt)
                {
                    if (EncryptedValue != null)
                    {
                        #region Write the encrypted value to the decryption stream
                        using (MemoryStream encryptedStream = new MemoryStream())
                        {
                            using (CryptoStream decryptStream = new CryptoStream(encryptedStream, DecryptorTransform, CryptoStreamMode.Write))
                            {
                                decryptStream.Write(EncryptedValue, 0, EncryptedValue.Length);
                                decryptStream.FlushFinalBlock();
                                #endregion

                                #region Read the decrypted value from the stream.
                                encryptedStream.Position = 0;
                                Byte[] decryptedBytes = new Byte[encryptedStream.Length];
                                encryptedStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                                encryptedStream.Close();
                                #endregion
                                return UTFEncoder.GetString(decryptedBytes);
                            }
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                return null;
            }
        }

        /// <summary>
        ///  Convert a string to a byte array.  NOTE: Normally we'd create a Byte Array from a string using an ASCII encoding (like so). 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static byte[] StrToByteArray(string str)
        {
            if (String.IsNullOrEmpty(str))
                return null;


            byte[] byteArr = new byte[str.Length / 3];
            try
            {

                byte val;
                byteArr = new byte[str.Length / 3];
                int i = 0;
                int j = 0;
                do
                {
                    val = byte.Parse(str.Substring(i, 3));
                    byteArr[j++] = val;
                    i += 3;
                }
                while (i < str.Length);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "str = "+ str);
                byteArr = null;
            }
            return byteArr;
        }


        /// <summary>
        /// Same comment as above.  Normally the conversion would use an ASCII encoding in the other direction: 
        /// </summary>
        /// <param name="byteArr"></param>
        /// <returns></returns>
        private static string ByteArrToString(byte[] byteArr)
        {
            byte val;
            string tempStr = "";
            try
            {
                if (byteArr != null)
                {
                    for (int i = 0; i <= byteArr.GetUpperBound(0); i++)
                    {
                        val = byteArr[i];
                        if (val < (byte)10)
                            tempStr += "00" + val.ToString();
                        else if (val < (byte)100)
                            tempStr += "0" + val.ToString();
                        else
                            tempStr += val.ToString();
                    }
                }
                else
                {
                    tempStr = null;
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                tempStr = null;
            }

            return tempStr;
        }




    }
}
