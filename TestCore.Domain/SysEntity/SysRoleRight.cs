using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Domain.SysEntity
{
    public partial class SysRoleRight
    {
        public long Id { get; set; }
        ///public int SiteId { get; set; }
        public int RoleId { get; set; }

        public Int64 UserId { get; set; }
        //public string UserName { get; set; }
        public int NodeId { get; set; }
        public int RightId { get; set; }
        public string Creator { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
