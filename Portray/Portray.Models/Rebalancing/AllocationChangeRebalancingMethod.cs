using System;
using System.Collections.Generic;

namespace Portray.Models.Rebalancing
{
    public class AllocationChangeRebalancingMethod : IRebalancingMethod
    {
        public IEnumerable<(string symbol, decimal quantity)> CalculateTargetHoldings(
            IEnumerable<(string symbol, decimal quantity)> currentHoldings,
            IEnumerable<(string symbol, double weight)> idealAllocations,
            IEnumerable<(string symbol, decimal price)> prices)
        {
            throw new NotImplementedException();
        }
    }
}
