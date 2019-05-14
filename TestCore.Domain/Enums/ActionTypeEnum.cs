using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Caiba.Models.I18N.Admin;

namespace TestCore.Domain.Enums
{

    /// <summary>
    /// 操作行为的类型（用户、会员、代理等通用）
    /// </summary>
    public enum ActionTypeEnum
    {
        /// <summary>
        /// 没有任何操作类型
        /// </summary>
        ///[Display(Name = "ActionTypeEnum_None",ResourceType = typeof(Resource))]
        None = 0,

        /// <summary>
        /// 登录
        /// </summary>
        [Display(Name = "ActionTypeEnum_Login", ResourceType = typeof(Resource))]
        Login = 1,


        /// <summary>
        /// 登出
        /// </summary>
        [Display(Name = "ActionTypeEnum_Logout", ResourceType = typeof(Resource))]
        Logout = 2,

        /// <summary>
        /// 浏览数据
        /// </summary>
        [Display(Name = "ActionTypeEnum_View", ResourceType = typeof(Resource))]
        View = 3,

        /// <summary>
        /// 修改密码
        /// </summary>
        [Display(Name = "ActionTypeEnum_UpdatePwd", ResourceType = typeof(Resource))]
        UpdatePwd = 4,

        /// <summary>
        /// 新增数据
        /// </summary>
        [Display(Name = "ActionTypeEnum_Create", ResourceType = typeof(Resource))]
        Create = 5,

        /// <summary>
        /// 编辑数据
        /// </summary>
        [Display(Name = "ActionTypeEnum_Edit", ResourceType = typeof(Resource))]
        Edit = 6,

        /// <summary>
        /// 删除数据
        /// </summary>
        [Display(Name = "ActionTypeEnum_Delete", ResourceType = typeof(Resource))]
        Delete = 7,

        /// <summary>
        /// 提款申请
        /// </summary>
        [Display(Name = "ActionTypeEnum_Apply", ResourceType = typeof(Resource))]
        Apply = 8,

        /// <summary>
        /// 审核
        /// </summary>
        [Display(Name = "ActionTypeEnum_Audit", ResourceType = typeof(Resource))]
        Audit = 9,

        /// <summary>
        /// 充值或付款
        /// </summary>
        [Display(Name = "ActionTypeEnum_Payment", ResourceType = typeof(Resource))]
        Payment = 10,

        /// <summary>
        /// 找回密码
        /// </summary>
        [Display(Name = "ActionTypeEnum_FindBackPwd", ResourceType = typeof(Resource))]
        FindBackPwd = 11,

        /// <summary>
        /// 钱包转账
        /// </summary>
        [Display(Name = "ActionTypeEnum_WalletTransfer", ResourceType = typeof(Resource))]
        WalletTransfer = 12,

        /// <summary>
        /// 上传图片
        /// </summary>
        [Display(Name = "ActionTypeEnum_UploadImage", ResourceType = typeof(Resource))]
        UploadImage = 13,

        /// <summary>
        /// 上传图片
        /// </summary>
        [Display(Name = "ActionTypeEnum_RedemptionPoint", ResourceType = typeof(Resource))]
        RedemptionPoint = 14,

        /// <summary>
        /// 推广金统计
        /// </summary>
        //[Display(Name = "ActionTypeEnum_RedemptionPoint", ResourceType = typeof(Resource))]
        StatMemSale = 15,

        /// <summary>
        /// 发送消息
        /// </summary>
        [Display(Name = "ActionTypeEnum_SendMessage", ResourceType = typeof(Resource))]
        SendMessage = 16,

        /// <summary>
        /// 拨号
        /// </summary>
        [Display(Name = "Dial", ResourceType = typeof(Resource))]
        Dial = 17
    }
}
