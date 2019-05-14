using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Domain.SysEntity
{
    public partial class SysOperator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortIndex { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public string Modifier { get; set; }
        public DateTime UpdateTime { get; set; }

        //public SysOperSite SysOperSite { get; set; }

    }
}
