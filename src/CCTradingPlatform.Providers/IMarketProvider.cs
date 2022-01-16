using System.Collections.Generic;

namespace CCTradingPlatform.Providers
{
    public interface IMarketProvider
    {
        IAsyncEnumerable<decimal> GetMarketStream();
    }
}
