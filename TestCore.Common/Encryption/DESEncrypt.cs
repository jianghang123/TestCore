using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TestCore.Common.Encryption
{

    /// <summary>
    /// DES加密/解密
    /// </summary>
    public sealed class DESEncrypt
    {
        #region Fields

        private byte[] Keys = new byte[] { 0x12, 0x34, 0x56, 120, 0x90, 0xab, 0xcd, 0xef };

        #endregion

        public string Decrypt(string Text)
        {
            return Decrypt(Text);
        }

        public string Decrypt(string decryptString, string decryptKey)
        {
            if (decryptString == null)
                throw new ArgumentNullException(nameof(decryptString));

            if (decryptKey == null)
                throw new ArgumentNullException(nameof(decryptKey));

            try
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                int num = decryptString.Length / 2;
                byte[] buffer = new byte[num];
                for (int i = 0; i < num; i++)
                {
                    int num3 = Convert.ToInt32(decryptString.Substring(i * 2, 2), 0x10);
                    buffer[i] = (byte) num3;
                }
                provider.Key = ASCIIEncoding.ASCII.GetBytes(MD5Encrypt.Encrypt(decryptKey).Substring(0, 8));
                provider.IV = ASCIIEncoding.ASCII.GetBytes(MD5Encrypt.Encrypt(decryptKey).Substring(0, 8));
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                return Encoding.Default.GetString(stream.ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }

        public string Encrypt(string Text)
        {
            return Encrypt(Text);
        }

        public string Encrypt(string encryptString, string encryptKey)
        {
            if (encryptString == null)
                throw new ArgumentNullException(nameof(encryptString));

            if(encryptKey==null)
                throw new ArgumentNullException(nameof(encryptKey));

            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(encryptString);
            provider.Key = ASCIIEncoding.ASCII.GetBytes(MD5Encrypt.Encrypt(encryptKey).Substring(0, 8));
            provider.IV = ASCIIEncoding.ASCII.GetBytes(MD5Encrypt.Encrypt(encryptKey).Substring(0, 8));
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            StringBuilder builder = new StringBuilder();
            foreach (byte num in stream.ToArray())
            {
                builder.AppendFormat("{0:X2}", num);
            }
            return builder.ToString();
        }
    }
}

