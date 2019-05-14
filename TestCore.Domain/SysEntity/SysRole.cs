using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Domain.SysEntity
{
    public class SysRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSupper { get; set; }
        public int GroupId { get; set; }
        public int SortIndex { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
