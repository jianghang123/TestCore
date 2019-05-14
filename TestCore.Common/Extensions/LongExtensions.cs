
namespace TestCore.Common.Extensions
{
    /// <summary>
    /// Long类型方法扩展
    /// </summary>
    public static class LongExtensions
    {
        /// <summary>
        /// 显示格式文件大小
        /// </summary>
        /// <param name="value">文件大小值</param>
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
