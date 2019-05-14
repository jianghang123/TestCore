using TestCore.Common.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace TestCore.Common.Helper
{
    public static class TimeHelper
    {
        public static readonly DateTime InitUnixDateTime = new DateTime(1970, 1, 1);

        public static string ConvertToDate(DateTime? time, string fomat = "yyyy-MM-dd")
        {
            return string.Format("{0:" + fomat + "}", time);
        }

        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        /// <summary>  
        /// Unix时间戳转为C#格式时间  
        /// </summary>  
        /// <param name=timeStamp></param>  
        /// <returns></returns>  
        public static DateTime ParseUnixDateTimeStamp(string timeStamp)
        {
            long seconds = TypeHelper.TryParse(timeStamp, (long)0);
            return InitUnixDateTime.AddSeconds((double)seconds);
        }

        /// <summary>  
        /// DateTime时间格式转换为Unix时间戳格式  
        /// 注：如果参数需要和php交互，请传入utc时间
        /// </summary>  
        /// <param name=time></param>  
        /// <returns></returns>  
        public static long ConvertToUnixDateTimeStamp(System.DateTime time)
        {
            return Convert.ToInt64((time - InitUnixDateTime).TotalSeconds);
        }

        public static DateTime ParseMillsTimeStamp(string timeStamp)
        {
            long seconds = TypeHelper.TryParse(timeStamp, (long)0);
            return InitUnixDateTime.AddMilliseconds(seconds);
        }


        /// <summary>
        /// 获取某一日期是该年中的第几周
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns>该日期在该年中的周数</returns>
        public static int GetWeekOfYear(DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();
            return gc.GetWeekOfYear(dt, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
        }

        /// <summary>
        /// 获取某一年有多少周
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns>该年周数</returns>
        public static int GetWeekAmount(int year)
        {
            DateTime end = new DateTime(year, 12, 31);  //该年最后一天
            System.Globalization.GregorianCalendar gc = new GregorianCalendar();
            return gc.GetWeekOfYear(end, CalendarWeekRule.FirstDay, DayOfWeek.Monday);  //该年星期数
        }

        public static Tuple<DateTime, DateTime> GetWeekRange(int Year, int Week)
        {
            DateTime YearStart = new DateTime(Year, 1, 1);
            //一年的第一天为星期几
            int a = (int)YearStart.Date.DayOfWeek;
            //星期起始減第一天的星期加上第一天即得一年第一個星期的第一天
            YearStart = YearStart.AddDays(1 - a);
            //加上(星期數-1)*7
            DateTime dStart = YearStart.AddDays(Convert.ToDouble(Week * 7 - 7));
            //當週結尾
            DateTime dEnd = dStart.AddDays(6);
            return new Tuple<DateTime, DateTime>(dStart, dEnd);
        }


        #region 得到一周的周一和周日的日期
        /// <summary> 
        /// 计算本周的周一日期 
        /// </summary> 
        /// <returns></returns> 
        public static DateTime GetMondayDate()
        {
            return GetMondayDate(DateTime.Now);
        }
        /// <summary> 
        /// 计算本周周日的日期 
        /// </summary> 
        /// <returns></returns> 
        public static DateTime GetSundayDate()
        {
            return GetSundayDate(DateTime.Now);
        }
        /// <summary> 
        /// 计算某日起始日期（礼拜一的日期） 
        /// </summary> 
        /// <param name="someDate">该周中任意一天</param> 
        /// <returns>返回礼拜一日期，后面的具体时、分、秒和传入值相等</returns> 
        public static DateTime GetMondayDate(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Monday;
            if (i == -1) i = 6;// i值 > = 0 ，因为枚举原因，Sunday排在最前，此时Sunday-Monday=-1，必须+7=6。 
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Subtract(ts);
        }
        /// <summary> 
        /// 计算某日结束日期（礼拜日的日期） 
        /// </summary> 
        /// <param name="someDate">该周中任意一天</param> 
        /// <returns>返回礼拜日日期，后面的具体时、分、秒和传入值相等</returns> 
        public static DateTime GetSundayDate(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Sunday;
            if (i != 0) i = 7 - i;// 因为枚举原因，Sunday排在最前，相减间隔要被7减。 
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Add(ts);
        }
        #endregion


        #region 根据当前日期确定当前是星期几  
        public static string GetWeekDay(string strDate)
        {
            try
            {
                //需要判断的时间
                DateTime dTime = Convert.ToDateTime(strDate);

                return GetWeekDay(dTime);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public static string GetWeekDay(DateTime dTime)
        {
            try
            {
                //确定星期几
                int index = (int)dTime.DayOfWeek;
                return GetWeekDay(index);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion

        //*******************************************************************//
        #region 转换星期的表示方法

        private static string GetWeekDay(int index)
        {
            string retVal = string.Empty;
            switch (index)
            {
                case 0:
                    {
                        retVal = "星期日";
                        break;
                    }
                case 1:
                    {
                        retVal = "星期一";
                        break;
                    }
                case 2:
                    {
                        retVal = "星期二";
                        break;
                    }
                case 3:
                    {
                        retVal = "星期三";
                        break;
                    }
                case 4:
                    {
                        retVal = "星期四";
                        break;
                    }
                case 5:
                    {
                        retVal = "星期五";
                        break;
                    }
                case 6:
                    {
                        retVal = "星期六";
                        break;
                    }
            }

            return retVal;
        }
        #endregion
    }
}

namespace System
{
    public static class TimeHelperExtension
    {
        #region 时区

        //本地时间 
        //世界统一时间：协调世界时(UTC)
        //本地时间 = UTC + 时区差


        /// <summary>
        /// 转为美东时间：夏令时-12h 冬令时-13h
        /// </summary>
        /// <param name="dateTime">转换时间</param>
        /// <returns>美东时间</returns>
        [Obsolete("请使用ConvertToEstTimeSummer", true)]
        public static DateTime ConvertToEstTime(this DateTime dateTime)
        {
            var estZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return TimeZoneInfo.ConvertTime(dateTime, estZoneInfo);
        }

        /// <summary>
        /// 转为美东夏令时 北京-12小時
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ConvertToEstTimeSummer(this DateTime dateTime)
        {
            var estZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            if (estZoneInfo.IsDaylightSavingTime(dateTime))
            {
                return TimeZoneInfo.ConvertTime(dateTime, estZoneInfo);
            }
            else
            {
                return TimeZoneInfo.ConvertTime(dateTime, estZoneInfo).AddHours(1);
            }
        }

        // <summary>
        /// 美东时间转为本地时间  夏令时+12h 冬令时+13h
        /// </summary>
        /// <param name="dateTime">转换时间</param>
        /// <returns>美东时间</returns>
        public static DateTime EstToLocalTime(this DateTime dateTime)
        {
            var estZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            if (estZoneInfo.IsDaylightSavingTime(dateTime))
            {
                return dateTime.AddHours(12);
            }
            else
            {
                return dateTime.AddHours(13);
            }
        }
        #endregion

        #region 起始时间-结束时间

        /// <summary>
        /// 当天开始时间：00:00:00
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime TodayStartTime(this DateTime dateTime, int? day = null, int? month = null, int? year = null)
        {
            return new DateTime(year ?? dateTime.Year, month ?? dateTime.Month, day ?? dateTime.Day);
        }

        /// <summary>
        /// 当天结束时间：23:59:59.999
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime TodayEndTime(this DateTime dateTime, int? day = null, int? month = null, int? year = null)
        {
            return new DateTime(year ?? dateTime.Year, month ?? dateTime.Month, day ?? dateTime.Day, 23, 59, 59, 999);
        }

        /// <summary>
        /// 当月开始时间 2017-09-01：00:00:00
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime MonthStartTime(this DateTime dateTime, int? month = null, int? year = null)
        {
            return new DateTime(year ?? dateTime.Year, month ?? dateTime.Month, 1);
        }

        /// <summary>
        /// 当月结束时间 2017-09-30：23:59:59
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime MonthEndTime(this DateTime dateTime, int? month = null, int? year = null)
        {
            return new DateTime(year ?? dateTime.Year, month ?? dateTime.Month, 1).AddMonths(1).AddSeconds(-1);
        }

        /// <summary>
        /// 当年起始时间：yyyy-MM-dd 00:00:00
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime YearStartTime(this DateTime dateTime, int? year = null)
        {
            return new DateTime(year ?? dateTime.Year, 1, 1);
        }

        /// <summary>
        /// 当年结束时间：yyyy-MM-dd 23:59:59.999
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime YearEndTime(this DateTime dateTime, int? year = null)
        {
            return new DateTime(year ?? dateTime.Year, 12, 31, 23, 59, 59, 999);
        }

        #endregion

        #region RFC3339时间 時區用UTC-4
        public static string LocalToRFC3339Est(this DateTime dt)
        {
            return dt.ConvertToEstTimeSummer().GetDateTimeFormats('s')[0] + "-04:00";
        }
        public static string LocalToRFC3339EstSummer(this DateTime dt)
        {
            return dt.ConvertToEstTimeSummer().GetDateTimeFormats('s')[0] + "-04:00";
        }
        public static DateTime RFC3339EstToLocal(string dt)
        {
            return Xml.XmlConvert.ToDateTime(dt, Xml.XmlDateTimeSerializationMode.Local);
        }
        #endregion
        public static int GetDateInt(this DateTime dt)
        {
            int year = dt.Year;
            int month = dt.Month;
            int day = dt.Day;
            return year * 10000 + month * 100 + day;
        }

        /// <summary>
        /// 获取本周整型日期范围 20190303-20190310
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int[] GetThisWeekDateInts(this DateTime dt)
        {
            var dates = new List<int>();
            for (int i = TimeHelper.GetMondayDate().GetDateInt(); i <= DateTime.Now.GetDateInt(); i++)
            {
                dates.Add(i);
            }
            return dates.ToArray();
        }

        /// <summary>
        /// 获取上周整型日期范围 20190303-20190310
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int[] GetLastWeekDateInts(this DateTime dt)
        {
            var dates = new List<int>();
            for (int i = TimeHelper.GetMondayDate().AddDays(-7).GetDateInt(); i <= DateTime.Now.GetDateInt(); i++)
            {
                dates.Add(i);
            }
            return dates.ToArray();
        }

        /// <summary>
        /// 获取本月整型日期范围 20190301-20190331
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int[] GetThiMonthDateInts(this DateTime dt)
        {
            var dates = new List<int>();
            for (int i = dt.MonthStartTime().GetDateInt(); i <= dt.GetDateInt(); i++)
            {
                dates.Add(i);
            }
            return dates.ToArray();
        }
    }
}
