using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TestCore.Common.Encryption
{
    /// <summary>
    /// 加解密工具类
    /// </summary>
    public class EncryptionUtils
    {

        /// <summary>
        /// Md5加密方法
        /// </summary>
        /// <param name="input">加密对象</param>
        /// <returns></returns>
        public static string Md5(string value)
        {
            byte[] bytes;
            using (var md5 = MD5.Create())
            {
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
            var result = new StringBuilder();
            foreach (byte t in bytes)
            {
                result.Append(t.ToString("X2"));
            }
            return result.ToString();
        }
        /// <summary>
        /// Md5加密方法
        /// </summary>
        /// <param name="input">加密对象</param>
        /// <returns></returns>
        public static string Md5ToBase64(string value)
        {
            byte[] bytes;
            using (var md5 = MD5.Create())
            {
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
            return Convert.ToBase64String(bytes);
        }

        public static string Md5str(string value)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }
        }


        public static string Hmac(string value, string key)
        {
            byte[] bytes;
            using (var hmac = new HMACMD5(Encoding.UTF8.GetBytes(key)))
            {
                bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
            StringBuilder result = new StringBuilder();
            foreach (byte t in bytes)
            {
                result.Append(t.ToString("X2"));
            }
            return result.ToString();
        }

        public static string EncodeBase64(Encoding encode, string source)
        {
            string enstring = "";
            byte[] bytes = encode.GetBytes(source);
            try
            {
                enstring = Convert.ToBase64String(bytes);
            }
            catch
            {
                enstring = source;
            }
            return enstring;
        }
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }


        /// <summary>
        /// HMAC-SHA1加密算法
        /// </summary>
        /// <param name="secret">密钥</param>
        /// <param name="strOrgData">源文</param>
        /// <returns></returns>
        public static string HmacSha1Sign(string secret, string strOrgData)
        {
            var hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(secret));
            var dataBuffer = Encoding.UTF8.GetBytes(strOrgData);
            var hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes);
        }


        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="data">内容</param>
        /// <param name="Appkey">秘钥</param>
        /// <returns></returns>
        public static string AesEncrypt(string data, string Appkey)
        {
            var encryptKey = Encoding.UTF8.GetBytes(Appkey);
            var v = Encoding.UTF8.GetBytes(Appkey.Substring(0, 16));
            var aesAlg = Aes.Create();
            aesAlg.BlockSize = aesAlg.LegalBlockSizes[0].MaxSize;
            aesAlg.KeySize = aesAlg.LegalKeySizes[0].MaxSize;
            aesAlg.Mode = CipherMode.ECB;
            aesAlg.Padding = PaddingMode.PKCS7;
            using (var encryptor = aesAlg.CreateEncryptor(encryptKey, v))
            {
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor,
                        CryptoStreamMode.Write))

                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(data);
                    }
                    var decryptedContent = msEncrypt.ToArray();
                    return Convert.ToBase64String(decryptedContent);

                }
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="input">内容</param>
        /// <param name="Appkey">秘钥</param>
        /// <returns></returns>
        public static string AesDecrypts(string input, string Appkey)
        {
            var fullCipher = Convert.FromBase64String(input);

            var iv = Encoding.UTF8.GetBytes(Appkey);
            var cipher = new byte[fullCipher.Length];
            //Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, 0, cipher, 0, fullCipher.Length);
            var decryptKey = Encoding.UTF8.GetBytes(Appkey);

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;
                using (var decryptor = aesAlg.CreateDecryptor(decryptKey, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(fullCipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }

        public static string AesDecrypt(string input, string Appkey)
        {
            var fullCipher = Convert.FromBase64String(input);

            var iv = new byte[16];
            var cipher = new byte[16];
            //Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var decryptKey = Encoding.UTF8.GetBytes(Appkey);
            //Buffer.BlockCopy(decryptKey, 0, iv, 0, iv.Length);
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;
                using (var decryptor = aesAlg.CreateDecryptor(decryptKey, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(fullCipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt,
                            decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
    }
}
