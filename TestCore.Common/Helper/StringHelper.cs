using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace TestCore.Common.Helper
{
    /// <summary>
    /// 字符串帮助类
    /// </summary>
    public sealed class StringHelper
    {
        /// <summary>
        /// 将集合类型数据转换为采用分隔符形式的字符串
        /// </summary>
        /// <param name="items">集合类型数据</param>
        /// <param name="delimiter">分隔符</param>
        /// <returns></returns>
        public static string Concat(ICollection items, string delimiter)
        {
            if (items == null)
            {
                return string.Empty;
            }
            if (items.Count == 0)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            foreach (object obj2 in items)
            {
                if (obj2 != null)
                {
                    builder.Append(obj2);
                    builder.Append(delimiter);
                }
            }
            return builder.ToString().TrimEnd(delimiter.ToCharArray());
        }

        /// <summary>
        /// 获取一个字符在字符数组的索引
        /// </summary>
        /// <param name="strSearch">查询的字符</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写</param>
        /// <returns></returns>
        public static int GetInArrayID(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            if (strSearch != null)
            {
                if (stringArray == null)
                {
                    return -1;
                }
                for (int i = 0; i < stringArray.Length; i++)
                {
                    if (caseInsensetive)
                    {
                        if (strSearch.ToLower() == stringArray[i].ToLower())
                        {
                            return i;
                        }
                    }
                    else if (strSearch == stringArray[i])
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// 截断字符(一个汉字占2个字节计算）
        /// </summary>
        /// <param name="srcString">字符串</param>
        /// <param name="length">截取长度</param>
        /// <param name="tailString">尾巴字符</param>
        /// <returns></returns>
        public static string GetSubString(string srcString, int length, string tailString)
        {
            return GetSubString(srcString, 0, length, tailString);
        }

        /// <summary>
        /// 截断字符(一个汉字占2个字节计算）
        /// </summary>
        /// <param name="srcString">字符串</param>
        /// <param name="startIndex">截取开始位置</param>
        /// <param name="length">截取长度</param>
        /// <param name="tailString">尾巴字符</param>
        /// <returns></returns>
        public static string GetSubString(string srcString, int startIndex, int length, string tailString)
        {
            if (srcString == null)
            {
                return string.Empty;
            }
            if (srcString.Length == 0)
            {
                return string.Empty;
            }
            string str = srcString;
            byte[] bytes = Encoding.UTF8.GetBytes(srcString);
            foreach (char ch in Encoding.UTF8.GetChars(bytes))
            {
                if (((ch > 'ࠀ') && (ch < '一')) || ((ch > 0xac00) && (ch < 0xd7a3)))
                {
                    if (startIndex >= srcString.Length)
                    {
                        return "";
                    }
                    return srcString.Substring(startIndex, ((length + startIndex) > srcString.Length) ? (srcString.Length - startIndex) : length);
                }
            }
            if (length < 0)
            {
                return str;
            }
            byte[] sourceArray = Encoding.Default.GetBytes(srcString);
            if (sourceArray.Length <= startIndex)
            {
                return str;
            }
            int num = sourceArray.Length;
            if (sourceArray.Length > (startIndex + length))
            {
                num = length + startIndex;
            }
            else
            {
                length = sourceArray.Length - startIndex;
                tailString = "";
            }
            int num2 = length;
            int[] numArray = new int[length];
            byte[] destinationArray = null;
            int num3 = 0;
            for (int i = startIndex; i < num; i++)
            {
                if (sourceArray[i] > 0x7f)
                {
                    num3++;
                    if (num3 == 3)
                    {
                        num3 = 1;
                    }
                }
                else
                {
                    num3 = 0;
                }
                numArray[i] = num3;
            }
            if ((sourceArray[num - 1] > 0x7f) && (numArray[length - 1] == 1))
            {
                num2 = length + 1;
            }
            destinationArray = new byte[num2];
            Array.Copy(sourceArray, startIndex, destinationArray, 0, num2);
            return (Encoding.Default.GetString(destinationArray) + tailString);
        } 


        public static string GetUnicodeSubString(string str, int length, string tailString)
        {
            if (str == null)
            {
                return string.Empty;
            }
            if (str.Length == 0)
            {
                return string.Empty;
            }
            string str2 = string.Empty;
            int byteCount = Encoding.Default.GetByteCount(str);
            int num2 = str.Length;
            int num3 = 0;
            int num4 = 0;
            if (byteCount <= length)
            {
                return str;
            }
            for (int i = 0; i < num2; i++)
            {
                if (Convert.ToInt32(str.ToCharArray()[i]) > 0xff)
                {
                    num3 += 2;
                }
                else
                {
                    num3++;
                }
                if (num3 > length)
                {
                    num4 = i;
                    break;
                }
                if (num3 == length)
                {
                    num4 = i + 1;
                    break;
                }
            }
            if (num4 >= 0)
            {
                str2 = str.Substring(0, num4) + tailString;
            }
            return str2;
        } 

        public static bool InArray(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            return (GetInArrayID(strSearch, stringArray, caseInsensetive) > -1);
        }

        public static bool InArray(string str, string stringarray, string strSplit, bool caseInsensetive)
        {
            return InArray(str, SplitString(stringarray, strSplit), caseInsensetive);
        }

        public static bool InIPArray(string ip, string[] iparray)
        {
            if (ip != null)
            {
                if (ip.Length == 0)
                {
                    return false;
                }
                string[] strArray = SplitString(ip, ".");
                for (int i = 0; i < iparray.Length; i++)
                {
                    string[] strArray2 = SplitString(iparray[i], ".");
                    int num2 = 0;
                    for (int j = 0; j < strArray2.Length; j++)
                    {
                        if (strArray2[j] == "*")
                        {
                            return true;
                        }
                        if ((strArray.Length <= j) || !(strArray2[j] == strArray[j]))
                        {
                            break;
                        }
                        num2++;
                    }
                    if (num2 == 4)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 采用英文逗号分割字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <returns></returns>
        public static string[] SplitString(string sourceStr)
        {
            return SplitString(sourceStr, ",");
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="strContent">源字符串</param>
        /// <param name="strSplit">分割符</param>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (strContent == null)
            {
                return new string[0];
            }
            if (strContent.Length == 0)
            {
                return new string[0];
            }
            if (strContent.IndexOf(strSplit) < 0)
            {
                return new string[] { strContent };
            }
            return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
        } 
    }
}

