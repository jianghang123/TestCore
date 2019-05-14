using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TestCore.Common.Security
{
    public class Signature
    {
        /** 默认编码字符集 */
        private static string DEFAULT_CHARSET = "GBK";
        public static string GetSignContent(IDictionary<string, string> parameters)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append("=").Append(value).Append("&");
                }
            }
            string content = query.ToString().Substring(0, query.Length - 1);

            return content;
        }

        public static string RSASign(IDictionary<string, string> parameters, string privateKeyPem, string charset, string signType)
        {
            string signContent = GetSignContent(parameters);

            return RSASignCharSet(signContent, privateKeyPem, charset, signType);
        }

        public static string RSASignCharSet(string data, string privateKeyPem, string charset, string signType)
        {
            RSA rsa = RSAUtil.CreateRsaFromPrivateKey(privateKeyPem);
            byte[] dataBytes = null;
            if (string.IsNullOrEmpty(charset))
            {
                dataBytes = Encoding.UTF8.GetBytes(data);
            }
            else
            {
                dataBytes = Encoding.GetEncoding(charset).GetBytes(data);
            }
            var cipherBytes = rsa.Encrypt(dataBytes, RSAEncryptionPadding.Pkcs1);

            if ("RSA2".Equals(signType))
            {
                cipherBytes = rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
            else
            {
                cipherBytes = rsa.SignData(dataBytes, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            }
            return Convert.ToBase64String(cipherBytes);
        }

        public static bool RSACheckV2(IDictionary<string, string> parameters, string publicKeyPem, string charset, string signType, bool keyFromFile)
        {
            string sign = parameters["sign"];

            parameters.Remove("sign");
            string signContent = GetSignContent(parameters);

            return RSACheckContent(signContent, sign, publicKeyPem, charset, signType, keyFromFile);
        }

        public static bool RSACheckContent(string signContent, string sign, string publicKeyPem, string charset, string signType, bool keyFromFile)
        {

            try
            {
                byte[] dataBytes;
                if (string.IsNullOrEmpty(charset))
                {
                    dataBytes = Encoding.UTF8.GetBytes(signContent);
                }
                else
                {
                    dataBytes = Encoding.GetEncoding(charset).GetBytes(signContent);
                }
                var rsa = RSAUtil.CreateRsaFromPublicKey(publicKeyPem);
                //var cipherBytes = System.Convert.FromBase64String(signContent);
                //var plainTextBytes = rsa.Decrypt(cipherBytes, RSAEncryptionPadding.Pkcs1);

                if ("RSA2".Equals(signType))
                {
                    return rsa.VerifyData(dataBytes, Convert.FromBase64String(sign), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                }
                else
                {
                    return rsa.VerifyData(dataBytes, Convert.FromBase64String(sign), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
                }
            }
            catch
            {
                return false;
            }

        }

    }
}
