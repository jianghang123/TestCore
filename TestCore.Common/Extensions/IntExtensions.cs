
using System;

namespace TestCore.Common.Extensions
{
    /// <summary>
    /// Int类型方法扩展
    /// </summary>
    public static class IntExtensions
    {
        public static string ToDisplayFileSize(this int value)
        {
            if (value < 1000)
            {
                return string.Format("{0} Byte", value);
            }
            else if (value >= 1000 && value < 1000000)
            {
                return string.Format("{0:F2} Kb", ((double)value) / 1024);
            }
            else if (value >= 1000 && value < 1000000000)
            {
                return string.Format("{0:F2} M", ((double)value) / 1048576);
            }
            else
            {
                return string.Format("{0:F2} G", ((double)value) / 1073741824);
            }
        }
        /// <summary>
        /// 转换成等效的枚举对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="i">需要转换的数字</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T ToEnum<T>(this int i, T defaultValue) where T : struct, IComparable, IFormattable
        {
            T convertedValue;

            if (!System.Enum.TryParse(i.ToString(), true, out convertedValue))
            {
                convertedValue = defaultValue;
            } 
            return convertedValue;
        }
    }
}
