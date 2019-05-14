using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Domain.CommonEntity
{

    /// <summary>
    /// 缓存数据用
    /// </summary>
    public class SimpleResource
    {
        public Int32 TableId { get; set; }

        public String FieldName { get; set; }

        public int PkId { get; set; }

        public int SortIndex { get; set; }

        public int Status { get; set; }

        public String ResValue { get; set; }

    }




}
