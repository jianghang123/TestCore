
using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace TestCore.Common.Extensions
{
    /// <summary>
    /// String类型方法扩展
    /// </summary>
    public static class StringExtensions
    {
        #region 正则表达式
        private static readonly Regex acsiiExpression = new Regex(@"^[\x00-\xFF]+$");
        private static readonly Regex base64Expression = new Regex(@"[A-Za-z0-9\+\/\=]");
        private static readonly Regex chineseExpression = new Regex(@"^[\u4E00-\u9FA5\uF900-\uFA2D]+$");
        private static readonly Regex decimalExpression = new Regex(@"^[0-9]+(\.[0-9]+)?$"); 
        private static readonly Regex decimalSignExpression = new Regex(@"^[+-]?[0-9]+(\.[0-9]+)?$");
        private static readonly Regex emailExpression = new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly Regex idcardExpression = new Regex(@"(^\d{15}$)|(^\d{17}[0-9]|X|x)$");
        private static readonly Regex ip4Expression = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
        private static readonly Regex letterExpression = new Regex("^[A-Za-z]+$");
        private static readonly Regex lletterExpression = new Regex("^[a-z]+$");
        private static readonly Regex uletterExpression = new Regex("^[A-Z]+$");
        private static readonly Regex mobileExpression = new Regex(@"^((13|15|17|18)[0-9]|14[57])\d{8}$");
        private static readonly Regex numberExpression = new Regex("^[0-9]+$");
        private static readonly Regex numberSignExpression = new Regex("^[+-]?[0-9]+$");
        private static readonly Regex postCodeExpression = new Regex(@"^[0-9]\d{5}$");
        private static readonly Regex priceExpression = new Regex(@"^(0|[1-9][0-9]{0,10})(\.[0-9]{1,2}){0,1}$");
        private static readonly Regex qqExpression = new Regex("^[1-9]*[1-9][0-9]*$");
        private static readonly Regex telExpression = new Regex(@"^(([0\+]\d{2,3}-)?(0\d{2,3})-)?(\d{7,8})(-(\d{3,}))?$");
        private static readonly Regex timeExpression = new Regex("^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        private static readonly Regex webUrlExpression = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly Regex stripHTMLExpression = new Regex("<\\S[^><]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        # endregion

        /// <summary>
        /// 是否为空白字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
        /// <summary>
        /// 是否为非空白字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotNullAndWhiteSpace(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }
        /// <summary>
        /// 是否为空字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        /// <summary>
        /// 是否为非空字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        } 
        /// <summary>
        /// 是否是ACSII码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsACSII(this string str)
        {
            return str.IsNotNullOrEmpty() && acsiiExpression.IsMatch(str);
        }
        /// <summary>
        /// 是否是Base64字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBase64String(this string str)
        {
            return str.IsNotNullOrEmpty() && base64Expression.IsMatch(str);
        }
        /// <summary>
        /// 是否是中文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsChinese(this string str)
        {
            return str.IsNotNullOrEmpty() && chineseExpression.IsMatch(str);
        }
        /// <summary>
        /// 是否是无符号小数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDecimal(this string str)
        {
            return str.IsNotNullAndWhiteSpace() && decimalExpression.IsMatch(str);
        }
        /// <summary>
        /// 是否是有符号小数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDecimalSign(this string str)
        {
            return str.IsNotNullAndWhiteSpace() && decimalSignExpression.IsMatch(str);
        }
        /// <summary>
        /// 是否是邮箱地址
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmail(this string str)
        {
            return str.IsNotNullAndWhiteSpace() && emailExpression.IsMatch(str);
        }
        /// <summary>
        /// 是否是身份证号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsIdCard(this string str)
        {
            return str.IsNotNullAndWhiteSpace() && idcardExpression.IsMatch(str);
        }
        /// <summary>
        /// 是否是IP4
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsIP4(this string str)
        {
            return str.IsNotNullAndWhiteSpace() && ip4Expression.IsMatch(str);
        }
        /// <summary>
        /// 是否是英文字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsLetter(this string str)
        {
            return str.IsNotNullAndWhiteSpace() && letterExpression.IsMatch(str);
        }
        /// <summary>
        /// 是否是小写英文字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsLLetter(this string str)
        {
            return str.IsNotNullAndWhiteSpace() && lletterExpression.IsMatch(str);
        }
        /// <summary>
        /// 是否是大写英文字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsULetter(this string str)
        {
            return str.IsNotNullAndWhiteSpace() && uletterExpression.IsMatch(str);
        }
        /// <summary>
        /// 是否是手机号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsMobile(this string str)
        {
            return str.IsNotNullAndWhiteSpace() && mobileExpression.IsMatch(str);
        }
        /// <summary>
        /// 是否是正数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(this string str)
        { 
            return str.IsNotNullAndWhiteSpace() && numberExpression.IsMatch(str);
        }
        /// <summary>
        /// 是否是有符号的整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumberSign(this string str)
        {
            return str.IsNotNullAndWhiteSpace() && numberSignExpression.IsMatch(str);
        }
        /// <summary>
        /// 是否是中国邮政编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsPostCode(this string str)
        { 
            return str.IsNotNullAndWhiteSpace() && postCodeExpression.IsMatch(str);
        }
        /// <summary>
        /// 是否是价格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsPrice(this string str)
        { 
            return str.IsNotNullAndWhiteSpace() && priceExpression.IsMatch(str);
        }
        /// <summary>
        /// 是否是QQ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsQQ(this string str)
        { 
            return str.IsNotNullAndWhiteSpace() && qqExpression.IsMatch(str);
        }
        /// <summary>
        /// 是否是中国电话
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsTel(this string str)
        {
            return str.IsNotNullAndWhiteSpace() && telExpression.IsMatch(str);
        }
        /// <summary>
        /// 是否是时间
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsTime(this string str)
        { 
            return str.IsNotNullAndWhiteSpace() && timeExpression.IsMatch(str);
        }
        /// <summary>
        /// 是否是网址
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsWebUrl(this string str)
        {
            return !string.IsNullOrWhiteSpace(str) && webUrlExpression.IsMatch(str);
        } 
        /// <summary>
        /// 获取字符串长度（按字节）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetStringLength(this string str)
        {
            if (str.IsNullOrEmpty())
            {
                return 0;
            }
            return Encoding.Default.GetBytes(str).Length;
        }
        /// <summary>
        /// 清理SQL语句危险字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FilterSql(this string str)
        {
            if (str.IsNullOrEmpty())
            {
                return string.Empty;
            }
            str = str.ToLower();
            return str.Replace("'", "")
                .Replace(",", "")
                .Replace(";", "")
                .Replace("=", "")
                .Replace(" or ", "")
                .Replace("select", "")
                .Replace("update", "")
                .Replace("insert", "")
                .Replace("delete", "")
                .Replace("declare", "")
                .Replace("exec", "")
                .Replace("drop", "")
                .Replace("create ", "")
                .Replace("%", "")
                .Replace("--", "—");
        }
        /// <summary>
        /// 去掉最后一个字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CutLastChar(this string str)
        {
            if (str.IsNullOrEmpty())
            {
                return string.Empty;
            }
            return str.Substring(0, str.Length - 1);
        }
        /// <summary>
        /// 去掉HTML代码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string NoHtml(this string str)
        {
            if (str.IsNullOrEmpty())
            {
                return string.Empty;
            }
            //删除脚本
            str = str.Replace("\r\n", "");
            str = Regex.Replace(str, @"<script.*?</script>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"<style.*?</style>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"<.*?>", "", RegexOptions.IgnoreCase);
            //删除HTML
            str = Regex.Replace(str, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"-->", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"<!--.*", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(nbsp|#160);", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "\\s{2,}", "", RegexOptions.IgnoreCase);
            str = str.Replace("<", "");
            str = str.Replace(">", "");
            str = str.Replace("\r\n", "");
            str = str.Replace("\n", ""); 
            return str;
        } 
        /// <summary>
        /// 转换为字节系列
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ToByte(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
        /// <summary>
        /// 转换为已解码的字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string str)
        {
            return WebUtility.HtmlDecode(str);
        }
        /// <summary>
        /// 转换为HTML编码字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string str)
        {
            return WebUtility.HtmlEncode(str);
        }
        /// <summary>
        /// 转换为URL编码字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(this string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = Encoding.UTF8.GetBytes(str);
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }
            return (sb.ToString());
        }
        /// <summary>
        /// 转换为Unicode编号字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUnicode(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                builder.Append("\\u" + ((int)str[i]).ToString("x"));
            }
            return builder.ToString();
        } 
        /// <summary>
        /// 将参数值格式化到字符中
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string str, params object[] args)
        { 
            return string.Format(str, args);
        } 
        /// <summary>
        /// 去除脚本和HTML
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StripHtml(this string str)
        {
            return stripHTMLExpression.Replace(str, string.Empty);
        }
        /// <summary>
        /// 第一个字符转为小写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FirstCharToLowerCase(this string str)
        {
            if (str.IsNotNullAndWhiteSpace() && str.Length > 1 && char.IsUpper(str[0]))
            {
                return char.ToLower(str[0]) + str.Substring(1);
            }
            return str;
        }
        /// <summary>
        /// 第一个字符转为大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FirstCharToUpperCase(this string str)
        {
            if (str.IsNotNullAndWhiteSpace() && str.Length > 1 && char.IsLower(str[0]))
            {
                return char.ToUpper(str[0]) + str.Substring(1);
            }
            return str;
        }
        /// <summary>
        /// 转换成等效的枚举对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="str">需要转换的字符</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T ToEnum<T>(this string str, T defaultValue) where T : struct, IComparable, IFormattable
        {
            T convertedValue = defaultValue;

            if (!string.IsNullOrWhiteSpace(str) && !System.Enum.TryParse(str.Trim(), true, out convertedValue))
            {
                convertedValue = defaultValue;
            } 
            return convertedValue;
        }
        /// <summary>
        /// 是否是Byte类型字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsByte(this string str)
        {
            byte result;
            return byte.TryParse(str, out result);
        }
        /// <summary>
        /// 是否是INT类型字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt(this string str)
        {
            int result;
            return int.TryParse(str, out result);
        } 
        /// <summary>
        /// 是否是Float类型字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsFloat(this string str)
        {
            float result;
            return float.TryParse(str, out result);
        }
        /// <summary>
        /// 是否是Double类型字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDouble(this string str)
        {
            double result;
            return double.TryParse(str, out result);
        }
        /// <summary>
        /// 是否是DateTime类型字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string str)
        {
            DateTime result;
            return DateTime.TryParse(str, out result);
        }
        /// <summary>
        /// 转换为Bool类型值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool AsBool(this string str)
        {
            bool result = false;
            bool.TryParse(str, out result);
            return result;
        }
        /// <summary>
        /// 转换为日期时间值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime AsDateTime(this string str)
        {
            DateTime result = DateTime.MinValue;
            DateTime.TryParse(str, out result);
            return result;
        }
        /// <summary>
        /// 转换为实数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Decimal AsDecimal(this string str)
        {
            decimal result = 0.0M;
            Decimal.TryParse(str, out result);
            return result;
        }
        /// <summary>
        /// 转换为Byte类型值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int AsByte(this string str)
        {
            byte result = 0;
            byte.TryParse(str, out result);
            return result;
        }
        /// <summary>
        /// 转换为INT类型值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int AsInt(this string str)
        {
            int result = 0;
            int.TryParse(str, out result);
            return result;
        }
        /// <summary>
        /// 转换为Float类型值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float AsFloat(this string str)
        {
            float result = 0f;
            float.TryParse(str, out result);
            return result;
        }
        /// <summary>
        /// 转换为Double类型值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double AsDouble(this string str)
        {
            double result = 0d;
            double.TryParse(str, out result);
            return result;
        }

        /// <summary>
        /// 自动转换为正则表达式内可直接使用的字符串（自动插入转义字符）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EscapeForRegex(this string str)
        {
            return str.IsNullOrWhiteSpace() ? str : Regex.Escape(str);
        }

        /// <summary>
        /// 大小写敏感比较
        /// </summary>
        /// <param name="str"></param>
        /// <param name="comparing"></param>
        /// <returns></returns>
        public static bool CaseSensitiveEquals(this string str, string comparing)
        {
            if (str == null && comparing == null)
            {
                return true;
            }
            if (str != null && comparing == null || str == null)
            {
                return false;
            }
            return string.CompareOrdinal(str, comparing) == 0;
        }

        /// <summary>
        /// 大小写忽略比较
        /// </summary>
        /// <param name="str"></param>
        /// <param name="comparing"></param>
        /// <returns></returns>
        public static bool CaseInsensitiveEquals(this string str, string comparing)
        {
            if (str == null && comparing == null)
            {
                return true;
            }
            if (str != null && comparing == null || str == null)
            {
                return false;
            }
            return str.Equals(comparing, StringComparison.OrdinalIgnoreCase);
        }
         
        public static string Fill(this string str, params object[] arguments)
        {
            Guard.ArgumentNotNullOrWhiteSpace(str, nameof(str));
            return string.Format(CultureInfo.CurrentCulture, str, arguments);
        }
    }
}
