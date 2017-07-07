using System;
using System.Collections.Generic;

namespace Portray.Models
{
    public class Equity : Instrument
    {
        public Equity(ExchangeCode exchangeCode, string marketCode)
            : base($"{exchangeCode.ToString().ToUpperInvariant()}:{marketCode}")
        {
            ExchangeCode = exchangeCode;
            MarketCode = marketCode ?? throw new ArgumentNullException(nameof(marketCode));
        }

        public string MarketCode { get; set; }
        public ExchangeCode ExchangeCode { get; set; }
        public IEnumerable<CurrencySymbol> CurrencySymbols { get; set; }
    }

    public class Stock : Equity
    {
        public Stock(ExchangeCode exchangeCode, string marketCode)
            : base(exchangeCode, marketCode)
        {

        }
    }

    public class Etf : Equity
    {
        public Etf(ExchangeCode exchangeCode, string marketCode)
            : base(exchangeCode, marketCode)
        {

        }
    }
}
