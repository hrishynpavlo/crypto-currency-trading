using Binance.Net;
using Binance.Net.Objects;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Threading.Channels;

namespace CCTradingPlatform.Providers.Binance
{
    public class BinanceMarket : IMarketProvider
    {
        private readonly ChannelReader<decimal> _channelReader;

        public BinanceMarket(ChannelReader<decimal> channelReader)
        {
            _channelReader = channelReader ?? throw new ArgumentNullException(nameof(channelReader));
        }

        public IAsyncEnumerable<decimal> GetMarketStream() => _channelReader.ReadAllAsync();
    }
}
