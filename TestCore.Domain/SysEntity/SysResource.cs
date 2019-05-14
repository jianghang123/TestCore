using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Domain.SysEntity
{
    public class SysResource
    {
        public int Id { get; set; }
        public int DbId { get; set; }
        public string Lang { get; set; }
        public int TableId { get; set; }
        public string TableName { get; set; }
        public int PkId { get; set; }
        public string FieldName { get; set; }
        public int SortIndex { get; set; }
        public string ResValue { get; set; }
        public int Status { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
