using TestCore.Common.Configuration;
using System;
using System.Security.Cryptography;
using System.Text;
using TestCore.Common.Helper;

namespace TestCore.Common.Encryption
{ 
    /// <summary>
    /// AES加密/解密
    /// </summary>
    public sealed class AESEncrypt
    {
        #region Fields

        private byte[] Keys = new byte[] { 0x41, 0x72, 0x65, 0x79, 0x6f, 0x75, 0x6d, 0x79, 0x53, 110, 0x6f, 0x77, 0x6d, 0x61, 110, 0x3f };

        private readonly TestCoreConfig config;

        #endregion

        #region Ctor

        public AESEncrypt(TestCoreConfig config)
        {
            this.config = config;
        }

        #endregion

        #region Methods

        public string Decrypt(string Text)
        {
            return Decrypt(Text, config.AESEncryptKey);
        }

        public string Decrypt(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = CommonHelper.EnsureMaximumLength(decryptKey, 0x20, string.Empty);
                decryptKey = decryptKey.PadRight(0x20, ' ');
                ICryptoTransform transform = new RijndaelManaged { Key = Encoding.UTF8.GetBytes(decryptKey), IV = Keys }.CreateDecryptor();
                byte[] inputBuffer = Convert.FromBase64String(decryptString);
                byte[] bytes = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        public string Encrypt(string Text)
        {
            return Encrypt(Text, config.AESEncryptKey);
        }

        public string Encrypt(string encryptString, string encryptKey)
        {
            encryptKey = CommonHelper.EnsureMaximumLength(encryptKey, 0x20, string.Empty);
            encryptKey = encryptKey.PadRight(0x20, ' ');
            ICryptoTransform transform = new RijndaelManaged { Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 0x20)), IV = Keys }.CreateEncryptor();
            byte[] bytes = Encoding.UTF8.GetBytes(encryptString);
            return Convert.ToBase64String(transform.TransformFinalBlock(bytes, 0, bytes.Length));
        }

        #endregion

    }
}

