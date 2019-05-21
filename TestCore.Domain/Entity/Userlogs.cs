using System;
using System.Collections.Generic;
using System.Text;
using TestCore.Common.Attributes;

namespace TestCore.Domain.Entity
{
    public class Userlogs
    {
        [FieldInfo(IsPK = true, IsIdEntity = true)]
        public int Id { get; set; }
        public int Userid { get; set; }
        public string Ip { get; set; }
        public string Address { get; set; }
        public DateTime Addtime { get; set; }
    }
}
