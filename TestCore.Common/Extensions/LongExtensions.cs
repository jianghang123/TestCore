
namespace TestCore.Common.Extensions
{
    /// <summary>
    /// Long���ͷ�����չ
    /// </summary>
    public static class LongExtensions
    {
        /// <summary>
        /// ��ʾ��ʽ�ļ���С
        /// </summary>
        /// <param name="value">�ļ���Сֵ</param>
        /// <returns></returns>
        public static string ToDisplayFileSize(this long value)
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
            else if (value >= 1000000000 && value < 1000000000000)
            {
                return string.Format("{0:F2} G", ((double)value) / 1073741824);
            }
            else
            {
                return string.Format("{0:F2} T", ((double)value) / 1099511627776);
            }
        }
    }
}