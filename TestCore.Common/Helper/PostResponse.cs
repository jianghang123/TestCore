using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Common.Helper
{
    public class PostResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
