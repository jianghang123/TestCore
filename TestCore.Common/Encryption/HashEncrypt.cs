using System;
using System.Security.Cryptography;
using System.Text;

namespace TestCore.Common.Encryption
{
    /// <summary>
    /// Hash加密
    /// </summary>
    public static class HashEncrypt
    {
        /// <summary>
        /// SHA1
        /// </summary>
        /// <param name="str">加密的字符串</param>
        /// <returns></returns>
        public static string GetSha1Hash(string str)
        {
            using (var hashAlgorithm = new SHA1CryptoServiceProvider())
            {
                var byteValue = Encoding.UTF8.GetBytes(str);
                var byteHash = hashAlgorithm.ComputeHash(byteValue);
                return Convert.ToBase64String(byteHash);
            }
        }

        /// <summary>
        /// SHA256
        /// </summary>
        /// <param name="str">加密的字符串</param>
        /// <returns></returns>
        public static string GetSha256Hash(string str)
        {
            using (var hashAlgorithm = new SHA256CryptoServiceProvider())
            {
                var byteValue = Encoding.UTF8.GetBytes(str);
                var byteHash = hashAlgorithm.ComputeHash(byteValue);
                return Convert.ToBase64String(byteHash);
            }
        }

        /// <summary>
        /// SHA384
        /// </summary>
        /// <param name="str">加密的字符串</param>
        /// <returns></returns>
        public static string GetSha384Hash(string str)
        {
            using (var hashAlgorithm = new SHA384CryptoServiceProvider())
            {
                var byteValue = Encoding.UTF8.GetBytes(str);
                var byteHash = hashAlgorithm.ComputeHash(byteValue);
                return Convert.ToBase64String(byteHash);
            }
        }

        /// <summary>
        /// SHA512
        /// </summary>
        /// <param name="str">加密的字符串</param>
        /// <returns></returns>
        public static string GetSha512Hash(string str)
        {
            using (var hashAlgorithm = new SHA512CryptoServiceProvider())
            {
                var byteValue = Encoding.UTF8.GetBytes(str);
                var byteHash = hashAlgorithm.ComputeHash(byteValue);
                return Convert.ToBase64String(byteHash);
            }
        }
    }
}