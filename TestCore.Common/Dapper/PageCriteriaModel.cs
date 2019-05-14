using System;

namespace TestCore.Common.Dapper
{
    public class PageCriteriaModel 
    {
        /// <summary>
        /// 表名或视图名,多表是请使用 tableName1 t1 inner join tableName2 t2 On t1.ID = t2.ID
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        public string Fields{ get; set; } = "*";

        /// <summary>
        /// 主键名
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 页索引，从1开始
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 排序字段（不需要ORDER BY关键字），如直接写ID DESC
        /// </summary>
        public string Sort { get; set; } = String.Empty;

        /// <summary>
        /// 查询条件（不需要WHERE关键字），如直接写ID=1 AND NAME='222'
        /// </summary>
        public string Condition { get; set; } = String.Empty;
    }
}
