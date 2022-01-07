using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CCTradingPlatform.Core.Strategies
{
    public class TwentyPercentTradingStrategy : ITradingStrategy
    {
        private readonly ChannelReader<decimal> _stream;
        private readonly decimal _volatility;
        private readonly decimal _maxPriceLimit;
        private readonly byte _maxRepetitionLimit;

        //public 

        public TwentyPercentTradingStrategy()
        {

        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var prevPrice = 0.0m;
            var repetition = 1;

            await foreach (var price in _stream.ReadAllAsync(cancellationToken))
            {
                if (repetition <= _maxRepetitionLimit && CanCreateOrder)
                {
                    if(repetition == 1)
                    {
                        if (_maxPriceLimit * (1 - _volatility) >= price)
                        {
                            var result = await MakeOrder();
                            if (result)
                            {
                                repetition++;
                                prevPrice = price;
                            }
                        }
                    }
                    else
                    {
                        if(_maxPriceLimit * (1 - _volatility) >= price && ((prevPrice * (1 + _volatility) <= price) || (prevPrice * (1 - _volatility) >= price)))
                        {
                            var result = await MakeOrder();
                            if (result)
                            {
                                repetition++;
                                prevPrice = price;
                            }
                        }
                    }
                }
                else;
            }
        }

        private bool CanCreateOrder => true;

        private async Task<bool> MakeOrder()
        {
            return await Task.FromResult(true);
        }

        private async Task MakeOrder(ref int repetition, ref int prevPrice, int price)
        {
            var result = await MakeOrder();
            if (result)
            {
                repetition++;
                prevPrice = price;
            }
        }
    }
}
