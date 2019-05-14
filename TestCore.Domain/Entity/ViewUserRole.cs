using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Domain.Entity
{
    public partial class ViewUserRole
    {
        public long UserId { get; set; }

        public string UserName { get; set; }

        public int Status { get; set; }

        public int RoleId { get; set; }

        public int OperId { get; set; }

        public string Email { get; set; }

        public string SiteIds { get; set; }

        public int SiteId { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public string LastLoginIp { get; set; }

        public int GroupId { get; set; }

        public bool IsSupper { get; set; }

        public int RoleStatus { get; set; }

        public bool IsChild { get; set; }

        public string ParentName { get; set; }

    }
}
