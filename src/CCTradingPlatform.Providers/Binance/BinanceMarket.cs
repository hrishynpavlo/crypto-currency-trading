using Binance.Net;
using Binance.Net.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace CCTradingPlatform.Providers.Binance
{
    public class BinanceMarket : IMarketProvider
    {
        public Channel<decimal> GetMarketStream()
        {
            var chan = Channel.CreateBounded<decimal>(new BoundedChannelOptions(512)
            {
                SingleWriter = true,
                SingleReader = false,
                AllowSynchronousContinuations = false,
                FullMode = BoundedChannelFullMode.DropOldest
            });
            var client = new BinanceSocketClient(new BinanceSocketClientOptions
            {
                
            });
            client.FuturesUsdt.SubscribeToMarkPriceUpdatesAsync("btc", 1000, msg => chan.Writer.TryWrite(msg.Data.MarkPrice));

            return chan;
        }
    }
}
