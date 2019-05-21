using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Domain.ViewEntity
{
   public class ViewCfo
    {
        public int Id { get; set; }
        public int Userid { get; set; }
        public string Bankname { get; set; }
        public string Realname { get; set; }
        public string Provice { get; set; }
        public string City { get; set; }
        public string Branchname { get; set; }
        public string Accountname { get; set; }
        public string Cardno { get; set; }
        public DateTime Addtime { get; set; }
    }
}
