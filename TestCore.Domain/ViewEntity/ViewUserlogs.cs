using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Domain.ViewEntity
{
    public class ViewUserlogs
    {
      public string Username { get; set; }
      public int Id { get; set; }
      public int Userid { get; set; }
      public string Ip { get; set; }
      public string Address { get; set; }
      public DateTime Addtime { get; set; }
    }
}
