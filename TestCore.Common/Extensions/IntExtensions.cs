
using System;

namespace TestCore.Common.Extensions
{
    /// <summary>
    /// Int���ͷ�����չ
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
        /// ת���ɵ�Ч��ö�ٶ���
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="i">��Ҫת��������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
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
