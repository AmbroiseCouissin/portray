using System.Collections.Generic;

namespace Portray.Models
{
    public interface IRebalancingMethod
    {
        IEnumerable<(string symbol, decimal quantity)> CalculateTargetHoldings(
            IEnumerable<(string symbol, decimal quantity)> currentHoldings,
            IEnumerable<(string symbol, double weight)> targetAllocations,
            IEnumerable<(string symbol, decimal price)> prices);
    }
}
