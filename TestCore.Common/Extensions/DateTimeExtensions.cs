
using System;
using System.Text;

namespace TestCore.Common.Extensions
{
    /// <summary>
    /// DateTime类型方法扩展
    /// </summary>
    public static class DateTimeExtensions
    {
        #region Fields

        private static readonly DateTime Jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        #endregion

        #region Utilities

        /// <summary>
        /// 获取美东时间
        /// </summary>
        /// <returns></returns>
        public static DateTime UtcNowToEasternTime()
        {
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToDateString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        #endregion

        #region Methods

        /// <summary>
        /// utc转美东时间
        /// </summary>
        /// <param name="utcDateTime"></param>
        /// <returns></returns>
        public static DateTime UtcNowToEasternTime(this DateTime utcDateTime)
        {
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, easternZone);
        }

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy-MM-dd HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="isRemoveSecond">是否移除秒</param>
        public static string ToDateTimeString(this DateTime dateTime, bool isRemoveSecond = false)
        {
            if (isRemoveSecond)
                return dateTime.ToString("yyyy-MM-dd HH:mm");
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy-MM-dd HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="isRemoveSecond">是否移除秒</param>
        public static string ToDateTimeString(this DateTime? dateTime, bool isRemoveSecond = false)
        {
            if (dateTime == null)
                return string.Empty;
            return ToDateTimeString(dateTime.Value, isRemoveSecond);
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToDateString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToDateString(dateTime.Value);
        }

        /// <summary>
        /// 获取格式化字符串，不带年月日，格式："HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 获取格式化字符串，不带年月日，格式："HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToTimeString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToTimeString(dateTime.Value);
        }

        /// <summary>
        /// 获取格式化字符串，带毫秒，格式："yyyy-MM-dd HH:mm:ss.fff"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToMillisecondString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// 获取格式化字符串，带毫秒，格式："yyyy-MM-dd HH:mm:ss.fff"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToMillisecondString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToMillisecondString(dateTime.Value);
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy年MM月dd日"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToChineseDateString(this DateTime dateTime)
        {
            return string.Format("{0}年{1}月{2}日", dateTime.Year, dateTime.Month, dateTime.Day);
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy年MM月dd日"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToChineseDateString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToChineseDateString(dateTime.SafeValue());
        }

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy年MM月dd日 HH时mm分"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="isRemoveSecond">是否移除秒</param>
        public static string ToChineseDateTimeString(this DateTime dateTime, bool isRemoveSecond = false)
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("{0}年{1}月{2}日", dateTime.Year, dateTime.Month, dateTime.Day);
            result.AppendFormat(" {0}时{1}分", dateTime.Hour, dateTime.Minute);
            if (isRemoveSecond == false)
                result.AppendFormat("{0}秒", dateTime.Second);
            return result.ToString();
        }

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy年MM月dd日 HH时mm分"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="isRemoveSecond">是否移除秒</param>
        public static string ToChineseDateTimeString(this DateTime? dateTime, bool isRemoveSecond = false)
        {
            if (dateTime == null)
                return string.Empty;
            return ToChineseDateTimeString(dateTime.Value);
        }
        public static string ToEasyString(this DateTime value)
        {
            DateTime now = DateTime.Now;
            if (now < value) return value.ToString("yyyy/MM/dd");
            TimeSpan dep = now - value;
            if (dep.TotalMinutes < 10)
            {
                return "刚刚";
            }
            else if (dep.TotalMinutes >= 10 && dep.TotalMinutes < 60)
            {
                return (int)dep.TotalMinutes + " 分钟前";
            }
            else if (dep.TotalHours < 24)
            {
                return (int)dep.TotalHours + " 小时前";
            }
            else if (dep.TotalDays < 5)
            {
                return (int)dep.TotalDays + " 天前";
            }
            else return value.ToString("yyyy/MM/dd");
        }
        public static string ToEasyString(this DateTime? value)
        {
            if (value.HasValue) return value.Value.ToEasyString();
            else return string.Empty;
        }

        /// <summary>
        /// 获取日期的最小时间表示形式(00:00:00)
        /// </summary>
        public static DateTimeOffset ToMinTimeDate(this DateTimeOffset date)
        {
            var result = new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, 0, date.Offset);
            return result;
        }

        /// <summary>
        /// 获取日期的最小时间表示形式(00:00:00)
        /// </summary>
        public static DateTime ToMinTimeDate(this DateTime date)
        {
            var result = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0, date.Kind);
            return result;
        }

        /// <summary>
        /// 获取日期的最大时间表示形式(23:59:59)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToMaxTimeDate(this DateTime date)
        {
            var result = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999, date.Kind);

            return result;
        }

        /// <summary>
        /// 返回自 1970 年 1 月 1 日 00:00:00 GMT 以来此 <see cref="DateTime"/> 对象表示的毫秒数。 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long GetUnixTime(this DateTime date)
        {
            var diff = date.ToUniversalTime() - Jan1St1970;
            return (long)Math.Floor(diff.TotalMilliseconds);
        }

        /// <summary>
        /// 获取日期的最大时间表示形式(23:59:59)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTimeOffset ToMaxTimeDate(this DateTimeOffset date)
        {
            var result = new DateTimeOffset(date.Year, date.Month, date.Day, 23, 59, 59, 999, date.Offset);

            return result;
        }

        /// <summary>
        /// 获取当前时间所在周的日期范围。
        /// </summary>
        public static RangeOfDateTime CurrentWeek(this DateTime dateTime)
        {
            var start = dateTime.AddDays(1 - int.Parse(dateTime.DayOfWeek.ToString("d"))).ToMinTimeDate();
            var end = start.AddDays(6).ToMaxTimeDate();
            return new RangeOfDateTime(start, end);
        }

        /// <summary>
        /// 获取当前时间所在月的日期范围。
        /// </summary>
        public static RangeOfDateTime CurrentMonth(this DateTime dateTime)
        {
            var start = dateTime.AddDays(1 - dateTime.Day).ToMinTimeDate();
            var end = start.AddMonths(1).AddDays(-1).ToMaxTimeDate();
            return new RangeOfDateTime(start, end);
        }

        /// <summary>
        /// 获取当前时间所在季度的日期范围。
        /// </summary>
        public static RangeOfDateTime CurrentQuarter(this DateTime dateTime)
        {
            var start = dateTime.AddMonths(0 - (dateTime.Month - 1) % 3).AddDays(1 - dateTime.Day).ToMinTimeDate();
            var end = start.AddMonths(3).AddDays(-1).ToMaxTimeDate();
            return new RangeOfDateTime(start, end);
        }

        /// <summary>
        /// 获取当前日期处于一年之中的哪个季度。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int QuarterOfYear(this DateTime dateTime)
        {
            return dateTime.Month / 4 + 1;
        }

        public static DateTime ChangeKind(this DateTime date, DateTimeKind kind)
        {
            var result = new DateTime(date.Ticks, kind);
            return result;
        }

        public static DateTime? ChangeKind(this DateTime? date, DateTimeKind kind)
        {
            return date.HasValue ? new DateTime?(date.Value.ChangeKind(kind)) : null;
        }

        public static DateTime? ToUniversalTime(this DateTime? date)
        {
            return date.HasValue ? new DateTime?(date.Value.ToUniversalTime()) : null;
        }

        public static DateTime WithoutMillis(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Kind);
        }

        #endregion

    }
}
