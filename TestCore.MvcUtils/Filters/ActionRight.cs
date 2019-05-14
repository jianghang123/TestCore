using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using TestCore.Domain.Enums;

namespace TestCore.MvcUtils
{

    /// <summary>
    /// 操作权限的配置属性
    /// </summary>
    public class ActionRight : Attribute , IActionConstraintMetadata
    {
        public RightTypeEnum RightType { get; set; } = RightTypeEnum.none;

        public int NodeId { get; set; }

        //public int[] NodeIds
        //{
        //    get;
        //    set;
        //} = new int[0];

        /// <summary>
        /// 只需登錄，不检查权限
        /// </summary>
        public bool NoAudit { get; set; } = false;

    }

    /// <summary>
    /// 日志的操作类别的属性
    /// </summary>
    public class ActionType : Attribute, IActionConstraintMetadata
    {

        /// <summary>
        /// 操作类别
        /// </summary>
        public ActionTypeEnum ActType { get; set; }


        ///// <summary>
        ///// 日志的存储方式
        ///// </summary>
        //public StoreTypeEnum StoreType { get; set; } = StoreTypeEnum.All;

    }
}
