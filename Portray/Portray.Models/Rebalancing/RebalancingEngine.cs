using System;
using System.Collections.Generic;
using System.Linq;

namespace Portray.Models
{
    public class RebalancingEngine
    {
        private readonly IEnumerable<IRebalancingThreshold> _rebalancingThresholds;
        private readonly IRebalancingMethod _rebalancingMethod;

        public RebalancingEngine(
            IEnumerable<IRebalancingThreshold> rebalancingThresholds,
            IRebalancingMethod rebalancingMethod)
        {
            _rebalancingThresholds = rebalancingThresholds;
            _rebalancingMethod = rebalancingMethod;
        }

        public IEnumerable<(string symbol, decimal quantity)> ExecuteRebalancing(
            IEnumerable<(string symbol, decimal quantity)> currentHoldings,
            IEnumerable<(string symbol, double weight)> targetAllocations,
            IEnumerable<(string symbol, decimal price)> prices)
        {
            // Check all prices are there
            if (currentHoldings
                .Select(ch => ch.symbol)
                .Concat(targetAllocations.Select(ta => ta.symbol))
                .Any(s => !prices.Select(p => p.symbol).Contains(s)))
            {
                throw new ArgumentNullException("one or more prices are missing");
            }

            // Calculate total market value
            decimal totalMarketValue = currentHoldings.Sum(ch => 
                ch.quantity * 
                prices.First(p => p.symbol == ch.symbol).price);

            // Check if thresholds are reached
            bool isThresholdReached = false;
            foreach (IRebalancingThreshold threshold in _rebalancingThresholds)
            {
                isThresholdReached = threshold.IsThresholdReached();
            }
            if (!isThresholdReached)
                return Enumerable.Empty<(string, decimal)>();
            
            // Get rebalancing orders
            return _rebalancingMethod.CalculateTargetHoldings(
                    currentHoldings, 
                    targetAllocations, 
                    prices);
        }
    }
}
