using System;

namespace TestCore.Common.IO
{
    /// <summary>
    /// 文件路径格式化类
    /// </summary>
    public static class PathFormatter
    {
        /// <summary>
        /// 格式化路径
        /// </summary> 
        /// <param name="pathFormat">路径</param>
        /// <param name="folder">新文件夹</param>
        /// <returns></returns>
        public static string Format(string pathFormat, string folder)
        {
            pathFormat = pathFormat.Replace("{folder}", folder);   
            pathFormat = pathFormat.Replace("{yyyy}", DateTime.Now.Year.ToString());
            pathFormat = pathFormat.Replace("{yy}", (DateTime.Now.Year % 100).ToString("D2"));
            pathFormat = pathFormat.Replace("{mm}", DateTime.Now.Month.ToString("D2"));
            pathFormat = pathFormat.Replace("{dd}", DateTime.Now.Day.ToString("D2"));
            //pathFormat = pathFormat.Replace("{hh}", DateTime.Now.Hour.ToString("D2"));
            //pathFormat = pathFormat.Replace("{ii}", DateTime.Now.Minute.ToString("D2"));
            //pathFormat = pathFormat.Replace("{ss}", DateTime.Now.Second.ToString("D2"));

            return pathFormat;
        }
    }
}
