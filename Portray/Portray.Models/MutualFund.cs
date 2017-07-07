using System.Collections.Generic;

namespace Portray.Models
{
    public class MutualFund : Instrument
    {
        public MutualFund(string issuer, string marketCode)
            : base($"{issuer.ToUpperInvariant()}:{marketCode.ToUpperInvariant()}")
        {

        }

        public string Issuer { get; set; }
        public string MarketCode { get; set; }
        public IEnumerable<CurrencySymbol> CurrencySymbols { get; set; }
    }
}
