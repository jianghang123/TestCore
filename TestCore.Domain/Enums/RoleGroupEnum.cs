using Caiba.Models.I18N.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestCore.Domain.Enums
{
    /// <summary>
    /// 角色組别
    /// </summary>
    public enum RoleGroupEnum
    {

        /// <summary>
        /// none
        /// </summary>
        None = 0,

        /// <summary>
        /// 系统商
        /// </summary>
        [Display(Name = "RoleGroupEnum_System", ResourceType = typeof(Resource))]
        System = 1,

        /// <summary>
        /// 运营商
        /// </summary>
        [Display(Name = "RoleGroupEnum_Operate", ResourceType = typeof(Resource))]
        Operator = 2,

        /// <summary>
        /// 代理商
        /// </summary>
        [Display(Name = "RoleGroupEnum_Agent", ResourceType = typeof(Resource))]
        Agent = 3,
    }
}
