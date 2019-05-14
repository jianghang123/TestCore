using System;
using System.Collections.Generic;
using System.Text;
using TestCore.Common.Attributes;

namespace TestCore.Domain.Entity
{
    public class Acc
    {
        [FieldInfo(IsPK = true, IsIdEntity = true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Acpcode { get; set; }
        public string Gateway { get; set; }
        public decimal Uprice { get; set; }
        public decimal Gprice { get; set; }
        public decimal Wprice { get; set; }
        public int Sortid { get; set; }
        public int Acwid { get; set; }
        public int Is_state { get; set; }
        public int Is_card { get; set; }
        public int Is_display { get; set; }

    }
}
