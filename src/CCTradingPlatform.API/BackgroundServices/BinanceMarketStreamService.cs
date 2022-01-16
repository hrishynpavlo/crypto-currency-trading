using Microsoft.Extensions.Hosting;
using PuppeteerSharp;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CCTradingPlatform.API.BackgroundServices
{
    public class BinanceMarketStreamService : IHostedService
    {
        private const string BTC_USDT_ENDPOINT = "https://www.binance.com/ru/trade/BTC_USDT?layout=pro";

        private readonly ChannelWriter<decimal> _channelWriter;
        private readonly IFormatProvider _currencyFormatProvider;

        private Browser _browser;
        private Page _page;

        public BinanceMarketStreamService(ChannelWriter<decimal> channelWriter)
        {
            _channelWriter = channelWriter ?? throw new ArgumentNullException(nameof(channelWriter));
            _currencyFormatProvider = CultureInfo.CreateSpecificCulture("en-GB"); 
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
            _browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            _page = await _browser.NewPageAsync();
            await _page.SetJavaScriptEnabledAsync(true);
            await _page.GoToAsync(BTC_USDT_ENDPOINT);

            _page.Console += OnMessage;

            var handle = await _page.EvaluateExpressionAsync(JS_PRICE_OBSERVER);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _page.Console -= OnMessage;
            await _page.DisposeAsync();
            await _browser.DisposeAsync();
        }

        private async void OnMessage(object sender, ConsoleEventArgs eventArgs)
        {
            if (decimal.TryParse(eventArgs.Message.Text, NumberStyles.AllowDecimalPoint, _currencyFormatProvider, out var price))
            {
                await _channelWriter.WriteAsync(price);
                Console.WriteLine(string.Format("Pushed value: {0} to channel!", price));
            }
        }

        private const string JS_PRICE_OBSERVER = @"
                const em = document.querySelector('div.showPrice');

                const onNext = (mutationsList, observer) => {
                    for (let mutation of mutationsList) {
                        if(mutation.type == 'characterData') {
                            console.log(parseFloat(mutation.target.data.replace(/,/g, '')));
                        }
                    }
                };

                const config = {
                    subtree: true,
                    characterData: true
                };

                const observer = new MutationObserver(onNext);
                observer.observe(em, config);
        ";
    }
}
