using Caiba.Models.I18N.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestCore.Domain.Enums
{
    public enum RightTypeEnum
    {
        /// <summary>
        /// 没有权限
        /// </summary>
        [Display(Name = "RightTypeEnum_none", ResourceType = typeof(Resource))]
        none = 0,
        /// <summary>
        /// 浏览权限
        /// </summary>
        [Display(Name = "RightTypeEnum_view", ResourceType = typeof(Resource))]
        view = 1,

        /// <summary>
        /// 新增权限
        /// </summary>
        [Display(Name = "RightTypeEnum_create", ResourceType = typeof(Resource))]
        create = 2,

        /// <summary>
        /// 编辑权限
        /// </summary>
        [Display(Name = "RightTypeEnum_edit", ResourceType = typeof(Resource))]
        edit = 3,

        /// <summary>
        /// 删除权限
        /// </summary>
        [Display(Name = "RightTypeEnum_delete", ResourceType = typeof(Resource))]
        delete = 4,

        /// <summary>
        /// 审核授权
        /// </summary>
        [Display(Name = "RightTypeEnum_audit", ResourceType = typeof(Resource))]
        audit = 5,

        /// <summary>
        /// 付款权限
        /// </summary>
        [Display(Name = "RightTypeEnum_payment", ResourceType = typeof(Resource))]
        payment = 6,


    }
}
