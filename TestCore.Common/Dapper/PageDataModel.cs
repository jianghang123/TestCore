using System.Collections.Generic;

namespace TestCore.Common.Dapper
{
    /// <summary>
    /// 分页模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageDataModel<T> 
    {
        public PageDataModel()
        {
            this.Data = new List<T>();
        }

        /// <summary>
        /// 数据
        /// </summary>
        public IList<T> Data { get; set; }

        /// <summary>
        /// 记录总行数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// 扩展分页模型（提供扩展字段）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageDataModelExtend<T> : PageDataModel<T>
    {
        public PageDataModelExtend() : base()
        {

        }

        public object Extend { get; set; }
    }
}
