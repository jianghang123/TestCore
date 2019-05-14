using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Common
{
    /// <summary>
    /// 用于避免输入错误的MimeType常量的集合
    /// 如果需要的mimetype缺失，可以随意添加
    /// </summary>
    public static class MimeTypes
    {
        #region application/*

        /// <summary>
        /// Type
        /// </summary>
        public const string ApplicationForceDownload = "application/force-download";

        /// <summary>
        /// json
        /// </summary>
        public const string ApplicationJson = "application/json";

        /// <summary>
        /// Type
        /// </summary>
        public const string ApplicationOctetStream = "application/octet-stream";

        /// <summary>
        /// pdf
        /// </summary>
        public const string ApplicationPdf = "application/pdf";

        /// <summary>
        /// rss xml
        /// </summary>
        public const string ApplicationRssXml = "application/rss+xml";

        /// <summary>
        /// Type
        /// </summary>
        public const string ApplicationXWwwFormUrlencoded = "application/x-www-form-urlencoded";

        /// <summary>
        /// zip
        /// </summary>
        public const string ApplicationXZipCo = "application/x-zip-co";

        /// <summary>
        /// 7z
        /// </summary>
        public const string Application7z = "application/x-7z-compressed";

        #endregion

        #region image/*

        /// <summary>
        /// bmp
        /// </summary>
        public const string ImageBmp = "image/bmp";

        /// <summary>
        /// gif
        /// </summary>
        public const string ImageGif = "image/gif";

        /// <summary>
        /// jpeg
        /// </summary>
        public const string ImageJpeg = "image/jpeg";

        /// <summary>
        /// pjpeg
        /// </summary>
        public const string ImagePJpeg = "image/pjpeg";

        /// <summary>
        /// png
        /// </summary>
        public const string ImagePng = "image/png";

        /// <summary>
        /// tiff、tif
        /// </summary>
        public const string ImageTiff = "image/tiff";

        /// <summary>
        /// mac
        /// </summary>
        public const string ImageMac = "image/x-macpaint";

        /// <summary>
        /// qti、qtif
        /// </summary>
        public const string ImageQti = "image/x-quicktime";

        /// <summary>
        /// pbm
        /// </summary>
        public const string ImagePbm = "image/x-portable-bitmap";

        /// <summary>
        /// pct、pict、pic
        /// </summary>
        public const string ImagePict = "image/pict";

        #endregion

        #region text/*

        /// <summary>
        /// css
        /// </summary>
        public const string TextCss = "text/css";

        /// <summary>
        /// csv
        /// </summary>
        public const string TextCsv = "text/csv";

        /// <summary>
        /// js
        /// </summary>
        public const string TextJavascript = "text/javascript";

        /// <summary>
        /// txt
        /// </summary>
        public const string TextPlain = "text/plain";

        /// <summary>
        /// xlsx
        /// </summary>
        public const string TextXlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        /// <summary>
        /// html
        /// </summary>
        public const string TextHtml = "text/html";

        /// <summary>
        /// wsc
        /// </summary>
        public const string TextWsc = "text/scriptlet";

        /// <summary>
        /// xml
        /// </summary>
        public const string TextXml = "text/xml";

        #endregion

        #region video/*

        /// <summary>
        /// 3gpp、3gpp
        /// </summary>
        public const string Video3gp = "video/3gpp";

        /// <summary>
        /// 3gpp2、3g2
        /// </summary>
        public const string Video3gp2 = "video/3gpp2";

        /// <summary>
        /// mp4、mp4v
        /// </summary>
        public const string VideoMp4 = "video/mp4";

        /// <summary>
        /// m4v
        /// </summary>
        public const string VideoM4v = "video/x-m4v";

        /// <summary>
        /// mpeg、mpa、mpe、mpg、mpv2
        /// </summary>
        public const string VideoMpg = "video/mpeg";

        /// <summary>
        /// mov、mqv
        /// </summary>
        public const string VideoMov = "video/quicktime";

        /// <summary>
        /// m2t
        /// </summary>
        public const string VideoM2t = "video/vnd.dlna.mpeg-tts";

        /// <summary>
        /// dv
        /// </summary>
        public const string VideoDv = "video/x-dv";

        /// <summary>
        /// lsf
        /// </summary>
        public const string VideoLsf = "video/x-la-asf";

        /// <summary>
        /// asf
        /// </summary>
        public const string VideoAsf = "video/x-ms-asf";

        #endregion
    }
}
