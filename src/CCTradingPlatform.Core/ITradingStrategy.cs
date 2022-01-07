using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CCTradingPlatform.Core
{
    public interface ITradingStrategy
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
