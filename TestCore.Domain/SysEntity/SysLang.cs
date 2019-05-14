using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Domain.SysEntity
{
    public class SysLang
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ResTag { get; set; }
        public string Tag { get; set; }
        public int? Status { get; set; }
        public int SortIndex { get; set; }
    }
}
