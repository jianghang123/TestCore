 
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Caiba.Models.I18N.Admin;

namespace TestCore.Domain.Enums
{
   

    /// <summary>
    /// 项目类别
    /// </summary>
    public enum ProjectTypeEnum
    {
        /// <summary>
        /// 後台
        /// </summary>
        //[Display(Name = "ProjectType_Admin", ResourceType = typeof(Resource))]
        Admin = 0,

        /// <summary> 
        /// 前台
        /// </summary>
        //[Display(Name = "ProjectType_Proxy", ResourceType = typeof(Resource))]
        Web = 1,

        /// <summary> 
        /// 会员
        /// </summary>
        //[Display(Name = "ProjectType_Member", ResourceType = typeof(Resource))]
        //Member = 2,

        /// <summary>
        /// 手机版前台
        /// </summary>
        Wap = 2,


        /// <summary>
        /// 手机app
        /// </summary>
        App = 3

    }

    public enum NoteTypeEnum
    {
        /// <summary>
        /// 管理员
        /// </summary>
        [Display(Name = "ProjectType_Admin", ResourceType = typeof(Resource))]
        Admin = 0,

        /// <summary> 
        /// 代理
        /// </summary>
        [Display(Name = "ProjectType_Proxy", ResourceType = typeof(Resource))]
        Proxy = 1,

        /// <summary> 
        /// 会员
        /// </summary>
        [Display(Name = "ProjectType_Member", ResourceType = typeof(Resource))]
        Member = 2,

        /// <summary>
        /// 手机版前台
        /// </summary>
        //Wap = 2,


        /// <summary>
        /// 手机app
        /// </summary>
        //App = 3

    }

}
 