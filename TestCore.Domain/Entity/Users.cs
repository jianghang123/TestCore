using System;
using System.Collections.Generic;
using System.Text;
using TestCore.Common.Attributes;

namespace TestCore.Domain.Entity
{
    public partial class Users
    {
        [FieldInfo(IsPK = true, IsIdEntity = true)]
        public int Id { get; set; }    
        public int Is_agent { get; set; }
        public int Ship_type { get; set; }
        public int Ship_cycle { get; set; }
        public string Username { get; set; }
        public string Userpass { get; set; }
        public int Is_state { get; set; }
        public decimal Paid { get; set; }
        public decimal Unpaid { get; set; }
        public string Token { set; get; }
        public string Apikey { get; set; }
        public int Is_checkout { get; set; }
        public int Is_paysubmit { get; set; }
        public int Is_verify_email { get; set; }
        public int Is_verify_phone { get; set; }
        public int Is_verify_siteurl { get; set; }
        public int Is_takecash { get; set; }
        public int Superid { get; set; }
        public string Salt { get; set; }
        public int Payfee { get; set; }
        public decimal Dpaid { get; set; }
        public string Dremark { get; set; }
        public string Paycodes { get; set; }
        public DateTime Addtime { get; set; }
    }
}
