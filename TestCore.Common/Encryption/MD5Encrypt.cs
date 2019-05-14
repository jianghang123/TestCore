using System;
using System.Security.Cryptography;
using System.Text;

namespace TestCore.Common.Encryption
{ 
    /// <summary>
    /// MD5加密/解密
    /// </summary>
    public sealed class MD5Encrypt
    {
        /// <summary>
        /// Encrypts the specified convert string.
        /// </summary>
        /// <param name="convertString">The convert string.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string MD5By16(string convertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(convertString)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }

        /// <summary>
        /// 对字符串str加密后再增加混淆串字符key一起进行加密后得到的字符串，两次都是采用MD5(string str)方法
        /// </summary>
        /// <param name="pass">字符串</param>
        /// <param name="key">混淆串字符</param>
        /// <returns>加密后的字符串</returns>
        public static string GetPass(string pass, string key)
        {
            //调用MD5生成密码
            return MD5(MD5(pass) + key);
        }
        /// <summary>
        /// MD5加密（去除“-”）得到字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(str)));
            t2 = t2.Replace("-", "").ToLower();
            return t2;
        }
        public static string getMd5Sign(string txt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(txt);
            MD5 mD = new MD5CryptoServiceProvider();
            byte[] value = mD.ComputeHash(bytes);
            return BitConverter.ToString(value).Replace("-", "");
        }
        /// <summary>
        /// 对字符串str加密后再增加混淆串字符key一起进行加密后得到的字符串，两次都是采用Encrypt(string str)方法
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="key">混淆字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptParty(string str, string key)
        {
            return Encrypt(Encrypt(str) + key);
        }
        /// <summary>
        /// 对字符串str进行加密后得到的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            bytes = new MD5CryptoServiceProvider().ComputeHash(bytes);
            string str2 = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str2 = str2 + bytes[i].ToString("x").PadLeft(2, '0');
            }
            return str2;
        }
        /// <summary>
        /// 字符串加密后转为Base64形式的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5ToBase64(string str)
        {
            MD5 md = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(str);
            return Convert.ToBase64String(md.ComputeHash(bytes));
        }
    }
}

