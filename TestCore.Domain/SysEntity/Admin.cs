using System;
using System.Collections.Generic;
using System.Text;
using TestCore.Common.Attributes;

namespace TestCore.Domain.SysEntity
{
    public class Admin
    {
        [FieldInfo(IsPK = true, IsIdEntity = true)]
        public int Id { get; set; }
        public string Adminname { get; set; }
        public string Adminpass { get; set; }
        public string Token { get; set; }
        public int Is_state { get; set; }
        public string Limits { get; set; }
        public string Limit_ip { get; set; }
        public int Is_limit_ip { get; set; }
        public string Google_auth { get; set; }
        public int ErrorTimes { get; set; }

        /// <summary>
        /// 权限值
        /// </summary>
        public class UserPermissionCodeEntity
        {
            /// <summary>
            /// 权限值
            /// </summary>
            public string MpCode { get; set; }

            public bool Any(Func<object, bool> p)
            {
                throw new NotImplementedException();
            }
        }
    }
}
