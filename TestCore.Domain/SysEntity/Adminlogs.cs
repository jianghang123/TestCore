using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TestCore.Common.Attributes;

namespace TestCore.Domain.SysEntity
{
    public class Adminlogs
    {
        [FieldInfo(IsPK = true, IsIdEntity = true)]
        public int Id { get; set; }

        public string AdminId { get; set; }
        public string Ip { get; set; }
        public DateTime Addtime { get; set; }

    }
}
