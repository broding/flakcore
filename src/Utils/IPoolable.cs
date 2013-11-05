using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flakcore.Utils
{
    public interface IPoolable
    {
        int PoolIndex { get; set; }
        Action<int> ReportDeadToPool { get; set; }
    }
}
