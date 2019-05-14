using System;

namespace TestCore.Common.Attributes
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class FieldInfoAttribute : Attribute
    {
        /// <summary>
        /// 是否自增长
        /// </summary>
        public bool IsIdEntity { get; set; } = false;

        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPK { get; set; } = false;

        /// <summary>
        /// 是否允许编辑
        /// </summary>
        public bool IsAllowEdit {  get; set; } = true;

    }
}
