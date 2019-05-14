using System;
using System.Collections.Generic;
using System.Text;
using TestCore.Common.Attributes;

namespace TestCore.Domain.Entity
{
    public class Userprice
    {
        [FieldInfo(IsPK = true, IsIdEntity = true)]
        public int Id { get; set; }
        public int Userid { get; set; }
        public int Channelid { get; set; }
        public decimal Uprice { get; set; }
        public decimal Gprice { get; set; }
        public int Is_state { get; set; }
        public DateTime Addtime { get; set; }
    }
}
