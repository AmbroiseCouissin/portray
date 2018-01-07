using System;
using System.Collections.Generic;
using System.Linq;

namespace Portray.Models.Rebalancing
{
    public class CashFlowRebalancingMethod : IRebalancingMethod
    {
        public CashFlowRebalancingMethod(string cashSymbol)
        {
            CashSymbol = cashSymbol;
        }

        public string CashSymbol { get; }

        public IEnumerable<(string symbol, decimal quantity)> CalculateTargetHoldings(
            IEnumerable<(string symbol, decimal quantity)> currentHoldings,
            IEnumerable<(string symbol, double weight)> targetAllocations,
            IEnumerable<(string symbol, decimal price)> prices)
        {
            decimal cashQuantity = currentHoldings.SingleOrDefault(ch => ch.symbol == CashSymbol).quantity;
            decimal totalMarketValue = currentHoldings.Sum(ch => ch.quantity * prices.Single(p => p.symbol == ch.symbol).price);

            IEnumerable<(string symbol, decimal quantity)> targetHoldings = null;
            if (cashQuantity == 0)
            {
                targetHoldings = Enumerable.Empty<(string symbol, decimal quantity)>();
            }
            else if (cashQuantity > 0)
            {
                return CalculateTargetInflowHoldings(
                    currentHoldings,
                    targetAllocations,
                    prices,
                    totalMarketValue,
                    cashQuantity,
                    CashSymbol);
            }
            else
            {
                throw new NotImplementedException();
            }

            return targetHoldings;
        }

        // TODO: There has to be a better, simpler way of doing this.
        private IEnumerable<(string symbol, decimal quantity)> CalculateTargetInflowHoldings(
            IEnumerable<(string symbol, decimal quantity)> holdings,
            IEnumerable<(string symbol, double weight)> targetAllocations,
            IEnumerable<(string symbol, decimal price)> prices,
            decimal totalMarketValue,
            decimal cashAmount,
            string cashSymbol)
        {
            List<(string symbol, decimal quantity)> holdingList = holdings.ToList();

            while (Math.Round(cashAmount, 6) != 0)
            {
                // Calculate current allocations
                IEnumerable<(string symbol, double weight)> currentAllocations = CalculateWeights(
                    holdingList, 
                    prices, 
                    totalMarketValue);
                
                // get the highest negative deviation
                double maxDev = targetAllocations
                    .Min(ta => (currentAllocations
                        .Where(c => c.symbol != cashSymbol)
                        .SingleOrDefault(h => h.symbol == ta.symbol).weight) - ta.weight);

                // get the allocations with highest negative deviations
                IEnumerable<(string symbol, double weight)> allocs = targetAllocations
                    .Where(ta => Math.Round(
                        ((currentAllocations
                            .Where(c => c.symbol != cashSymbol)
                            .SingleOrDefault(h => h.symbol == ta.symbol)
                            .weight)
                        - ta.weight), 6) == Math.Round(maxDev, 6));

                // get the second highest negative deviation
                IEnumerable<(string symbol, double weight)> remainingAllocations = targetAllocations.Except(allocs);
                double secondMaxDev = remainingAllocations.Any()
                    ? remainingAllocations
                        .Min(ra => (currentAllocations
                            .Where(c => c.symbol != cashSymbol)
                            .SingleOrDefault(h => h.symbol == ra.symbol).weight)
                        - ra.weight)
                    : 0;

                // increase holding quantity up to Min(cashacity, secondMaxNegDev)
                double cashWeight = currentAllocations.Single(ca => ca.symbol == cashSymbol).weight;
                IEnumerable<(string symbol, double weight)> allocsLocalModels =
                    allocs.Select(ra => (ra.symbol, weight: Math.Min(cashWeight, secondMaxDev - maxDev)));

                IEnumerable<(string symbol, decimal quantity)> augmentedHoldings =
                    GetWeightedHoldings(allocsLocalModels, prices, totalMarketValue, ref cashAmount);

                // Replace holdings
                foreach ((string symbol, decimal quantity) augmentedHolding in augmentedHoldings)
                {
                    (string symbol, decimal quantity) holding = holdingList.SingleOrDefault(h => h.symbol == augmentedHolding.symbol);
                    if (!holding.Equals(default))
                        holdingList.Remove(augmentedHolding);

                    holdingList.Add((augmentedHolding.symbol, quantity: holding.quantity + augmentedHolding.quantity));
                }

                // Replace cash holding
                holdingList.Remove(holdingList.First(h => h.symbol == cashSymbol));
                holdingList.Add((symbol: cashSymbol, quantity: cashAmount));
            }

            return holdingList;
        }

        private IEnumerable<(string symbol, double weight)> CalculateWeights(
            IEnumerable<(string symbol, decimal quantity)> holdings,
            IEnumerable<(string symbol, decimal price)> prices,
            decimal totalMarketValue) =>
            holdings.Select(h => (
                h.symbol, 
                weight: (double)(h.quantity * prices.First(p => p.symbol == h.symbol).price / totalMarketValue)));

        private static List<(string symbol, decimal quantity)> GetWeightedHoldings(
            IEnumerable<(string symbol, double weight)> allocations,
            IEnumerable<(string symbol, decimal price)> prices,
            decimal totalMarketValue,
            ref decimal capacity)
        {
            var idealHoldings = new List<(string symbol, decimal quantity)>();
            foreach ((string allocSymbol, double allocWeight) in allocations)
            {
                (string priceSymbol, decimal price) = prices.SingleOrDefault(p => p.symbol == allocSymbol);
                if (price.Equals(default))
                    throw new ArgumentNullException(nameof(priceSymbol));

                decimal quantity = (totalMarketValue * (decimal)allocWeight / price);

                capacity -= price * quantity;

                idealHoldings.Add((symbol: allocSymbol, quantity));
            }

            return idealHoldings;
        }
    }
}
