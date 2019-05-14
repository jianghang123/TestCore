using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using TestCore.Common;
using TestCore.Domain.SysEntity;

namespace TestCore.MvcUtils
{

    /// <summary>
    /// 前台项目使用的 web 辅助类
    /// </summary>
    public class MainWebUtil : WebUtil
    {


        /// <summary>
        /// 获取前台配置的站点 Id
        /// </summary>
        /// <returns></returns>
        public static int GetSiteId()
        {
            int siteId = WebConfig.AppSettings.SiteId;

            if (siteId == 0)
            {
                throw new Exception("没有配置站点信息");
            }
            return siteId;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="siteId">站点 Id</param>
        ///// <param name="gameId"> -1， 包括系统，0 包括钱包， 1 不包含钱包的所有游戏</param>
        ///// <returns></returns>
        //public static IEnumerable<SelectListItem> GetGameSelectListFromCache(int gameId = 1)
        //{
        //    var siteId = GetSiteId();

        //    return GetGameSelectListFromCache(siteId, gameId);
        //}

        //public static SysWebConfig GetWebConfigFromCache()
        //{
        //    return WebConfigRepository.GetModelFromCache(GetSiteId());
        //}


    }
}
