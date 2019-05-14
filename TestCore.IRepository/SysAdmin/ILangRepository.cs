using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using TestCore.Common.Ioc;
using TestCore.Domain.SysEntity;
using TestCore.IRepository;

namespace TestCore.IRepository.SysAdmin
{
    public interface ILangRepository : IRepository<SysLang>, ISingletonDependency
    {

        /// <summary>
        /// 获取语言
        /// </summary>
        /// <returns></returns>
        IEnumerable<SysLang> GetLangListFromCache();

        /// <summary>
        /// 从缓存异步获取语言数据字典
        /// </summary>
        /// <returns></returns>
        IEnumerable<SelectListItem> GetSelectListFromCache();


        /// <summary>
        /// 清除语言列表缓存
        /// </summary>
        void ClearCache();
    }
}
