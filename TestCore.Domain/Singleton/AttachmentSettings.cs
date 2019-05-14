using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using TestCore.Common.Configuration;

namespace TestCore.Domain.Singleton
{
    /// <summary>
    /// 2-附件配置
    /// </summary>
    public class AttachmentSettings : ISettings
    {
        public AttachmentSettings() { }
        public AttachmentSettings(Dictionary<string, string> dic)
        {
            if (dic != null && dic.Count > 0)
            {
                foreach (string key in dic.Keys)
                {
                    string value = dic[key];
                    PropertyInfo property = GetType().GetProperty(key);
                    if (property == null)
                    {
                        continue;
                    }
                    else
                    {
                        property.SetValue(this, Convert.ChangeType(value, property.PropertyType, CultureInfo.CurrentCulture), null);
                    }
                }
            }
        }
        /// <summary>
        /// 上传通讯密钥
        /// </summary>
        public string FileServerMD5Key { get; set; }
        /// <summary>
        /// 是否上传至共享目录(true,false)
        /// </summary>
        public string EnabledUploadShare { get; set; }
        /// <summary>
        /// 上传文件目录
        /// </summary>
        public string UploadDir { get; set; }
        /// <summary>
        /// 上传文件访问基地址
        /// </summary>
        public string UploadUrl { get; set; }
        /// <summary>
        /// 上传文件的保存目录规则
        /// </summary>
        public string UploadFilePathRule { get; set; }
        /// <summary>
        /// 上传文件名保存规则
        /// </summary>
        public string FileNameRule { get; set; }
        /// <summary>
        /// 允许上传图片类型
        /// </summary>
        public string UploadImgExt { get; set; }
        /// <summary>
        /// 允许上传图片最大大小（单位：KB）
        /// </summary>
        public int UploadImgMaxSize { get; set; }
        /// <summary>
        /// 水印类型(0代表没有水印，1代表文字水印，2代表图片水印)
        /// </summary>
        public int WatermarkType { get; set; }
        /// <summary>
        /// 水印质量(必须位于0到100之间)
        /// </summary>
        public int WatermarkQuality { get; set; }
        /// <summary>
        /// 水印位置(1代表上左，2代表上中，3代表上右，4代表中左，5代表中中，6代表中右，7代表下左，8代表下中，9代表下右)
        /// </summary>
        public int WatermarkPosition { get; set; }
        /// <summary>
        /// 水印图片
        /// </summary>
        public string WatermarkImg { get; set; }
        /// <summary>
        /// 水印图片透明度(必须位于1到10之间)
        /// </summary>
        public int WatermarkImgOpacity { get; set; }
        /// <summary>
        /// 水印文字
        /// </summary>
        public string WatermarkText { get; set; }
        /// <summary>
        /// 水印文字字体
        /// </summary>
        public string WatermarkTextFont { get; set; }
        /// <summary>
        /// 水印文字大小
        /// </summary>
        public int WatermarkTextSize { get; set; }
        /// <summary>
        /// 商品展示缩略图大小
        /// </summary>
        public string ProductShowThumbSize { get; set; }
    }
}
