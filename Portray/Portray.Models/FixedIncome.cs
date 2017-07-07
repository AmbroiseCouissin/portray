using System;
using System.Collections.Generic;

namespace Portray.Models
{
    public class FixedIncome : Instrument
    {
        public FixedIncome(string issuer, DateTime maturityDate)
            : base($"{issuer}:{maturityDate.ToString("yyMMdd")}")
        {
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
            MaturityDate = maturityDate;
        }

        public string Issuer { get; set; }
        public DateTime MaturityDate { get; set; }
        public IEnumerable<CurrencySymbol> CurrencySymbols { get; set; }
    }

    public class Bond : FixedIncome
    {
        public Bond(string issuer, DateTime maturityDate)
            : base(issuer, maturityDate)
        {

        }
    }
}
