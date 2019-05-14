using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Common
{
    /// <summary>
    /// 区间时间
    /// </summary>
    public struct RangeOfDateTime
    {
        public RangeOfDateTime(DateTime min, DateTime max)
        {
            Minimum = min;
            Maximum = max;
        }

        public DateTime Minimum { get; set; }

        public DateTime Maximum { get; set; }
    }
}
