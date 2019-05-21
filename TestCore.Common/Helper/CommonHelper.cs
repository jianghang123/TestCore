using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TestCore.Common.Helper
{
    /// <summary>
    /// ���ֲ��һ������������
    /// </summary>
    public partial class CommonHelper
    {
        #region Fields

        private static readonly Regex _emailRegex;
        //FluentValidation
        private const string _emailExpression = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$";

        #endregion

        #region Ctor

        static CommonHelper()
        {
            _emailRegex = new Regex(_emailExpression, RegexOptions.IgnoreCase);
        }

        #endregion

        #region Methods

        /// <summary>
        /// ȷ���û��ĵ����ʼ���Ͷ��
        /// </summary>
        /// <param name="email">��������</param>
        /// <returns></returns>
        public static string EnsureSubscriberEmailOrThrow(string email)
        {
            var output = EnsureNotNull(email);
            output = output.Trim();
            output = EnsureMaximumLength(output, 255);

            if (!IsValidEmail(output))
            {
                throw new TestCoreException("Email is not valid.");
            }

            return output;
        }

        /// <summary>
        /// ��֤�ַ����Ƿ�����Ч�����ʼ���ʽ
        /// </summary>
        /// <param name="email">��������</param>
        /// <returns>����ַ�������Ч�ĵ����ʼ���ַ����Ϊtrue��������ǣ���Ϊfalse</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            email = email.Trim();

           return _emailRegex.IsMatch(email);
        }

        /// <summary>
        /// ��֤�ַ����Ƿ�����Ч��IP��ַ
        /// </summary>
        /// <param name="ipAddress">ip��ַ</param>
        /// <returns>����ַ�������Ч��IP��ַ����Ϊtrue��������ǣ���Ϊfalse</returns>
        public static bool IsValidIpAddress(string ipAddress)
        {
            return IPAddress.TryParse(ipAddress, out IPAddress _);
        }

        /// <summary>
        /// ȷ���ַ������������������
        /// </summary>
        /// <param name="str">�����ַ�</param>
        /// <param name="maxLength">��󳤶�</param>
        /// <param name="postfix">���ԭʼ�ַ��������̣���Ҫ��ӵ���β���ַ���</param>
        /// <returns>��������ַ����ĳ��ȴ�����󳤶���������ַ����ض�</returns>
        public static string EnsureMaximumLength(string str, int maxLength, string postfix = null)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            if (str.Length > maxLength)
            {
                var pLen = postfix == null ? 0 : postfix.Length;

                var result = str.Substring(0, maxLength - pLen);
                if (!string.IsNullOrEmpty(postfix))
                {
                    result += postfix;
                }
                return result;
            }

            return str;
        }

        /// <summary>
        /// ȷ���ַ�����������ֵ
        /// </summary>
        /// <param name="str">�����ַ�</param>
        /// <returns>�����ַ���ֻ����ֵ�����ַ����������ΪNull���</returns>
        public static string EnsureNumericOnly(string str)
        {
            return string.IsNullOrEmpty(str) ? string.Empty : new string(str.Where(p => char.IsDigit(p)).ToArray());
        }

        /// <summary>
        /// ȷ���ַ������ǿյ�
        /// </summary>
        /// <param name="str">�����ַ�</param>
        /// <returns>string</returns>
        public static string EnsureNotNull(string str)
        {
            return str ?? string.Empty;
        }

        /// <summary>
        /// ָʾָ�����ַ����Ƿ�Ϊ�ջ���ַ���
        /// </summary>
        /// <param name="stringsToValidate">Ҫ��֤���ַ�������</param>
        /// <returns>Boolean</returns>
        public static bool AreNullOrEmpty(params string[] stringsToValidate)
        {
            return stringsToValidate.Any(p => string.IsNullOrEmpty(p));
        }

        /// <summary>
        /// �Ƚ���������
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="a1">Array 1</param>
        /// <param name="a2">Array 2</param>
        /// <returns>bool</returns>
        public static bool ArraysEqual<T>(T[] a1, T[] a2)
        { 
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            var comparer = EqualityComparer<T>.Default;
            for (var i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i])) return false;
            }
            return true;
        }


        /// <summary>
        /// ����һ���������Ե�ֵ
        /// </summary>
        /// <param name="instance">Ҫ�������ԵĶ���</param>
        /// <param name="propertyName">Ҫ���õ����Ե�����</param>
        /// <param name="value">��������Ϊ��ֵ</param>
        public static void SetProperty(object instance, string propertyName, object value)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            var instanceType = instance.GetType();
            var pi = instanceType.GetProperty(propertyName);
            if (pi == null)
                throw new TestCoreException("No property '{0}' found on the instance of type '{1}'.", propertyName, instanceType);
            if (!pi.CanWrite)
                throw new TestCoreException("The property '{0}' on the instance of type '{1}' does not have a setter.", propertyName, instanceType);
            if (value != null && !value.GetType().IsAssignableFrom(pi.PropertyType))
                value = To(value, pi.PropertyType);
            pi.SetValue(instance, value, new object[0]);
        }

        /// <summary>
        /// ��ֵת��ΪĿ������
        /// </summary>
        /// <param name="value">ת����ֵ</param>
        /// <param name="destinationType">��ֵת��Ϊ</param>
        /// <returns>ת��ֵ</returns>
        public static object To(object value, Type destinationType)
        {
            return To(value, destinationType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// ��ֵת��ΪĿ������
        /// </summary>
        /// <param name="value">ת����ֵ</param>
        /// <param name="destinationType">��ֵת��Ϊ</param>
        /// <param name="culture">Culture</param>
        /// <returns>ת��ֵ</returns>
        public static object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value != null)
            {
                var sourceType = value.GetType();

                var destinationConverter = TypeDescriptor.GetConverter(destinationType);
                if (destinationConverter != null && destinationConverter.CanConvertFrom(value.GetType()))
                    return destinationConverter.ConvertFrom(null, culture, value);

                var sourceConverter = TypeDescriptor.GetConverter(sourceType);
                if (sourceConverter != null && sourceConverter.CanConvertTo(destinationType))
                    return sourceConverter.ConvertTo(null, culture, value, destinationType);

                if (destinationType.IsEnum && value is int)
                    return System.Enum.ToObject(destinationType, (int)value);

                if (!destinationType.IsInstanceOfType(value))
                    return Convert.ChangeType(value, destinationType, culture);
            }
            return value;
        }

        /// <summary>
        /// ��ֵת��ΪĿ������
        /// </summary>
        /// <param name="value">ת����ֵ</param>
        /// <typeparam name="T">��ֵת��Ϊ</typeparam>
        /// <returns>ת��ֵ</returns>
        public static T To<T>(object value)
        { 
            return (T)To(value, typeof(T));
        }

        /// <summary>
        /// ת��Ϊö��
        /// </summary>
        /// <param name="str">�����ַ�</param>
        /// <returns>Converted string</returns>
        public static string ConvertEnum(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            var result = string.Empty;
            foreach (var c in str)
                if (c.ToString() != c.ToString().ToLower())
                    result += " " + c.ToString();
                else
                    result += c.ToString();
             
            result = result.TrimStart();
            return result;
        }

        /// <summary>
        /// ������������Ļ�
        /// </summary>
        public static void SetTelerikCulture()
        { 
            var culture = new CultureInfo("zh_CN");
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int GetDifferenceInYears(DateTime startDate, DateTime endDate)
        { 
            var age = endDate.Year - startDate.Year;
            if (startDate > endDate.AddYears(-age))
                age--;
            return age;
        }

        /// <summary>
        /// ��ȡ˽���ֶ�����ֵ
        /// </summary>
        /// <param name="target">Ŀ�����</param>
        /// <param name="fieldName">�ֶ�����</param>
        /// <returns>Value</returns>
        public static object GetPrivateFieldValue(object target, string fieldName)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target", "The assignment target cannot be null.");
            }

            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentException("fieldName", "The field name cannot be null or empty.");
            }

            var t = target.GetType();
            FieldInfo fi = null;

            while (t != null)
            {
                fi = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

                if (fi != null) break;

                t = t.BaseType;
            }

            if (fi == null)
            {
                throw new TestCoreException($"Field '{fieldName}' not found in type hierarchy.");
            }

            return fi.GetValue(target);
        }

        #endregion

    }
}
