using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace CCTradingPlatform.Providers
{
    public interface IMarketProvider
    {
        Channel<decimal> GetMarketStream();
    }
}
