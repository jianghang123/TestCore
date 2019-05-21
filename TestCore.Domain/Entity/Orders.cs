using System;
using System.Collections.Generic;
using System.Text;
using TestCore.Common.Attributes;

namespace TestCore.Domain.Entity
{
    public class Orders
    {
      [FieldInfo(IsPK = true, IsIdEntity = true)]
      public string Id { get; set; }
      public string Userid{ get; set; }
      public string Agentid{ get; set; }
      public string Orderid{ get; set; }
      public string Sdorderno{ get; set; }
      public string Total_fee{ get; set; }
      public string Realmoney{ get; set; }
      public string Channelid{ get; set; }
      public string Uprice{ get; set; }
      public string Gprice{ get; set; }
      public string Wprice{ get; set; }
      public string Is_state{ get; set; }
      public string Is_ship{ get; set; }
      public string Is_ship_agent{ get; set; }
      public string Is_paytype{ get; set; }
      public string Is_notify{ get; set; }
      public string Orderinfoid{ get; set; }
      public string Freeze{ get; set; }
      public string Lastime{ get; set; }
      public string Addtime{ get; set; }
    }
}
