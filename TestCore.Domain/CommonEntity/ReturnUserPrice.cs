using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Domain.CommonEntity
{
    public class ReturnUserPrice
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Acwid { get; set; }

        public Decimal Uprice_default { get; set; }

        public int Is_display { get; set; }

        public int Is_state { get; set; }
    }
}
