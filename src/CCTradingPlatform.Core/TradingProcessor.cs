using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CCTradingPlatform.Core
{
    public class TradingProcessor
    {
        private readonly decimal _upperLimit;
        private readonly Channel<decimal> _currentPriceChan;

        public TradingProcessor(decimal upperLimit, Channel<decimal> currentPriceChan)
        {
            _upperLimit = upperLimit;
            _currentPriceChan = currentPriceChan ?? throw new ArgumentNullException(nameof(currentPriceChan));
        }

        public async Task Start(ITradingStrategy strategy)
        {
            await strategy.ExecuteAsync();

        }
    }
}
