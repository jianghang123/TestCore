using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Caiba.Models.I18N.Admin;

namespace TestCore.Domain.Enums
{
    /// <summary>
    /// 日志正常、异常类别
    /// </summary>
    public enum LogTypeEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Display(Name = "LogTypeEnum_Normal",ResourceType = typeof(Resource))]
        Normal = 1,

        /// <summary>
        /// 异常
        /// </summary>
        [Display(Name = "LogTypeEnum_Exception", ResourceType = typeof(Resource))]
        Exception = 2,
    }

    /// <summary>
    /// 日志存储方式
    /// </summary>
    public enum StoreTypeEnum
    {
        /// <summary>
        /// 文件和数据库同时存储
        /// </summary>
        All = 0,

        /// <summary>
        /// 文件存储
        /// </summary>
        File = 1,

        /// <summary>
        /// 数据库存储
        /// </summary>
        DB = 2,

    }




}
