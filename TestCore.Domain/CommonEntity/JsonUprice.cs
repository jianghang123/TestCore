using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Domain.CommonEntity
{
    public class JsonUprice
    {
        public int Userid { get; set; }
        public int Channelid { get; set; }
        public decimal Uprice { get; set; }
        public decimal Gprice { get; set; }
        public int Is_state { get; set; }
    }
}
