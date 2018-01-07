namespace Portray.Models
{
    public class Currency : Instrument
    {
        public Currency(CurrencySymbol currencySymbol)
            : base(currencySymbol.ToString().ToUpperInvariant())
        {

        }
    }

    public enum CurrencySymbol
    {
        CNY,
        HKD,
        JPY,
        USD,
        EUR,
        ETH,
        BTC
    }
}
