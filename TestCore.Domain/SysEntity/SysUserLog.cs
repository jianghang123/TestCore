using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Domain.SysEntity
{
    public  class SysUserLog
    {
        public long Id { get; set; }

        //public int SiteId { get; set; }
        public string UserName { get; set; }
        public int NodeId { get; set; }
        public int LogType { get; set; }
        public int Result { get; set; }
        public int ActionType { get; set; }
        public string ActionDesc { get; set; }
        public int ClientType { get; set; }
        public string Ip { get; set; }
        public DateTime LogTime { get; set; }
        public DateTime LogTimeEst { get; set; }
    }
}
