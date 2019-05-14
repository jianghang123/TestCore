using System;

namespace TestCore.Admin.ViewModels
{
    /// <summary>
    /// 提示信息模型
    /// </summary>
    public class PromptModel
    {

        public PromptModel(string message)
        {
            this.Message = message;
            this.IsShowBackLink = false;
            this.IsAutoBack = false;
        }

        public PromptModel(string backUrl, string message)
        {
            this.BackUrl = backUrl;
            this.Message = message;
        }

        public PromptModel(string backUrl, string message, bool isAutoBack)
        {
            this.BackUrl = backUrl;
            this.Message = message;
            this.IsAutoBack = isAutoBack;
        }

        /// <summary>
        /// 返回地址
        /// </summary>
        public string BackUrl { get; set; } = String.Empty;

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; } = String.Empty;

        /// <summary>
        /// 倒计时模型
        /// </summary>
        public int CountdownModel { get; set; } = 1;

        /// <summary>
        /// 倒计时时间
        /// </summary>
        public int CountdownTime { get; set; } = 3;

        /// <summary>
        /// 是否显示返回地址
        /// </summary>
        public bool IsShowBackLink { get; set; } = true;

        /// <summary>
        /// 是否自动返回
        /// </summary>
        public bool IsAutoBack { get; set; } = true;
    }
}
