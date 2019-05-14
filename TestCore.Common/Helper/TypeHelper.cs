using TestCore.Common.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestCore.Common.Helper
{
    public static class TypeHelper
    {


        static readonly Type[] predefinedTypes = {
            typeof(Object),
            typeof(Boolean),
            typeof(Char),
            typeof(String),
            typeof(SByte),
            typeof(Byte),
            typeof(Int16),
            typeof(UInt16),
            typeof(Int32),
            typeof(UInt32),
            typeof(Int64),
            typeof(UInt64),
            typeof(Single),
            typeof(Double),
            typeof(Decimal),
            typeof(DateTime),
            typeof(TimeSpan),
            typeof(Guid),
            typeof(Math),
            typeof(Convert)
        };


        /// <summary>
        /// 是否预定义类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsPredefinedType(this Type type)
        {
            foreach (Type t in predefinedTypes) if (t == type) return true;
            return false;
        }

        /// <summary>
        /// 名称转换成枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumName"></param>
        /// <returns></returns>
        public static T TryParse<T>(string enumName) where T: struct
        {
            return (T)(Enum.Parse(typeof(T), enumName)); 
        }

        /// <summary>
        /// 获取主键,自增长,不允许编辑的成员名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Tuple<List<string>, List<string>, List<string>>  GetKeyNames(this Type type)
        {
            var pkFields = new List<string>();
            var idetFields = new List<string>();
            var exFields = new List<string>();

            var metaPros = GetFlogProperties(type);
            if (metaPros != null && metaPros.Any())
            {
                FieldInfoAttribute attr = null;
                foreach (var p in metaPros)
                {
                    attr = p.GetCustomAttribute<FieldInfoAttribute>();

                    if(attr != null)
                    {
                        if(attr.IsPK)
                        {
                            pkFields.Add(p.Name);
                        }
                        if(attr.IsIdEntity)
                        {
                            idetFields.Add(p.Name);
                        }
                        if(attr.IsAllowEdit == false)
                        {
                            exFields.Add(p.Name);
                        }
                    }
                }
            }
            return Tuple.Create(pkFields, idetFields, exFields);
        }


        /// <summary>
        /// 获取包含自定义标记的成员信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetFlogProperties(this Type type)
        {
            var pros = type.GetProperties().Where(p => p.GetCustomAttribute<FieldInfoAttribute>() != null);
            var metaType = type.GetTypeInfo().GetCustomAttribute<ModelMetadataTypeAttribute>();
            if (metaType != null)
            {
                var metaPros  = metaType.MetadataType.GetProperties().Where(p => p.GetCustomAttribute<FieldInfoAttribute>() != null)
                   .ToList();
                if (pros.Count() == 0) return metaPros;

                var metaNames = metaPros.Select(c => c.Name);
                foreach (var p in pros)
                {
                    if (!metaNames.IsContains(p.Name))
                    {
                        metaPros.Add(p);
                    }
                 }
                return metaPros;
            }
            return pros;
        }



        public static IEnumerable<string> GetSelectPropertyNames(this Type type)
        {
            var pros = type.GetProperties();
 
            var metaType = type.GetTypeInfo().GetCustomAttribute<ModelMetadataTypeAttribute>();

            List<string> metapNames = new List<string>();
            if (metaType != null )
            {
               var metaPros = metaType.MetadataType.GetProperties().Where(p => p.GetCustomAttribute<FieldInfoAttribute>() != null)
                   .ToList();
                foreach (var p in metaPros)
                {
                    var attr = p.GetCustomAttribute<FieldInfoAttribute>();
                    if (attr != null && attr.IsAllowEdit == false)
                    {
                        metapNames.Add(p.Name);
                    }
                }
            }
            var pNames = new List<string>();

            foreach(var p in pros)
            {
                var attr = p.GetCustomAttribute<FieldInfoAttribute>();
                if( attr != null &&  attr.IsAllowEdit == false )
                {
                    continue;
                }
                if(metapNames.IsContains(p.Name))
                {
                    continue;
                }
                pNames.Add(p.Name);
            }
            return pNames;
        }


        /// <summary>
        /// 获取单一主键名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string  GetPkName(this Type type)
        {
            var metaType = type.GetTypeInfo().GetCustomAttribute<ModelMetadataTypeAttribute>();
            FieldInfoAttribute temp = null;
            PropertyInfo[] pros = null;
            if (metaType != null)
            {
                pros = metaType.MetadataType.GetProperties().Where(p => (temp = p.GetCustomAttribute<FieldInfoAttribute>()) != null && temp.IsPK )
                   .ToArray();
                if (pros.Any())
                {
                    return pros.Select(c => c.Name).FirstOrDefault();
                }
            }
            pros = type.GetProperties().Where(p => (temp = p.GetCustomAttribute<FieldInfoAttribute>()) != null && temp.IsPK).ToArray();
            if (pros.Any())
            {
                return pros.Select(c => c.Name).FirstOrDefault();
            }
            return string.Empty;
        }

        /// <summary>
        /// 是否有成员
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasProperty(this Type type,string name = null)
        {
            if(string.IsNullOrEmpty(name))
            {
                return type.GetProperties().Any();
            }
            return type.GetProperties().Where(p => p.Name.IsEquals(name)).Any();
        }


        public static object GetPropertyValue(this Type type, object obj, string name)
        {
            var pro = type.GetProperties().Where(p => p.Name.IsEquals(name)).FirstOrDefault();

            if (pro == null) return null;

            return pro.GetValue(obj);
        }



        #region TryParse
        /// <summary>
        /// string ==&gt; int
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static int TryParse(string inputText, int defaultValue)
        {
            if (string.IsNullOrEmpty(inputText))
                return defaultValue;
            int result;
            return int.TryParse(inputText, out result) ? result : defaultValue;
        }

        /// <summary>
        /// string ==&gt; decimal
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static decimal TryParse(string inputText, decimal defaultValue)
        {
            if (string.IsNullOrEmpty(inputText))
                return defaultValue;
            decimal result;
            return decimal.TryParse(inputText, out result) ? result : defaultValue;
        }


        /// <summary>
        /// string ==&gt; long
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static long TryParse(string inputText, long defaultValue)
        {
            if (string.IsNullOrEmpty(inputText))
                return defaultValue;
            long result;
            return long.TryParse(inputText, out result) ? result : defaultValue;
        }

        /// <summary>
        /// string ==&gt; bool
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        public static bool TryParse(string inputText, bool defaultValue)
        {
            if (string.IsNullOrEmpty(inputText))
                return defaultValue;
            bool result;
            return bool.TryParse(inputText, out result) ? result : defaultValue;
        }

        /// <summary>
        /// string ==&gt; DateTime
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static DateTime TryParse(string inputText, DateTime defaultValue)
        {
            if (string.IsNullOrEmpty(inputText))
                return defaultValue;
            DateTime result;
            return DateTime.TryParse(inputText, out result) ? result : defaultValue;
        }

        /// <summary>
        /// string ==&gt; float
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static double TryParse(string inputText, double defaultValue)
        {
            if (string.IsNullOrEmpty(inputText))
                return defaultValue;
            double result;
            return double.TryParse(inputText, out result) ? result : defaultValue;
        }

        /// <summary>
        /// string ==&gt; int[]
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static int[] TryParse(string inputText, int[] defaultValue)
        {
            if (string.IsNullOrEmpty(inputText))
                return defaultValue;
            try
            {
                return inputText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToArray();
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// string ==&gt; Guid
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static Guid TryParse(string inputText, Guid defaultValue)
        {
            if (string.IsNullOrEmpty(inputText))
                return defaultValue;
            try
            {
                return new Guid(inputText);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// string ==&gt; Enum
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static Enum TryParse(string inputText, Enum defaultValue)
        {
            if (string.IsNullOrEmpty(inputText))
                return defaultValue;
            try
            {
                return (Enum)Enum.Parse(defaultValue.GetType(), inputText, true);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// string ==&gt; string
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string TryParse(string inputText, string defaultValue)
        {
            if (string.IsNullOrEmpty(inputText))
                return defaultValue;
            else
                return inputText;
        }

        /// <summary>
        /// TryParse指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T TryParse<T>(Func<T> func, T defaultValue)
        {
            try
            {
                return func();
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// TryParse指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T TryParse<T>(Func<T> func)
        {
            return TryParse<T>(func, default(T));
        }
        #endregion

        /// <summary>
        /// 扩展忽略大小写的Equals
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool IsEquals(this string str1, string str2)
        {
            return string.Equals(str1, str2, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 扩展忽略大小写的Contains
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool IsContains(this string str1, string str2)
        {
            if (string.IsNullOrEmpty(str1))
                return string.IsNullOrEmpty(str2);

            return (str1.IndexOf(str2, StringComparison.CurrentCultureIgnoreCase) >= 0);
        }

        /// <summary>
        /// 是否包含字符串 忽略大小写
        /// </summary>
        /// <param name="strList"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsContains(this IEnumerable<string> strList, string str)
        {
            if (strList == null || !strList.Any()) return false;

            return strList.Any(c => c.Equals(str, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="list"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsContains(this IEnumerable<int> list, int obj)
        {
            if (list == null || !list.Any()) return false;

            return list.Any(c => c == obj );
        }

        /// <summary>
        /// 判断字符串列表中是否包含子字符串
        /// </summary>
        /// <param name="strList"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsContainsSubString(this IEnumerable<string> strList, string str)
        {
            if (strList.Count() == 0) return false;

            return strList.Any(c => str.IndexOf(c, StringComparison.CurrentCultureIgnoreCase) >= 0);
        }

        /// <summary>
        /// GetSubString
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxLength">Length of the max.</param>
        /// <returns></returns>
        public static string GetSubString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            else if (value.Length > maxLength)
                return value.Substring(0, maxLength);
            else
                return value;
        }
        
        
        /// <summary>
        /// 获取要查询的属性名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetPropertyNames(this Type type)
        {
            var pros = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.GetProperty);

            if (pros == null || pros.Length == 0)
            {
                return "*";
            }
            return string.Join(",", type.GetSelectPropertyNames());
        }


    }
}
