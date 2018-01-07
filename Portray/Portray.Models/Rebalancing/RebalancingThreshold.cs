using System;
using System.Collections.Generic;
using System.Linq;

namespace Portray.Models
{
    public interface IRebalancingThreshold
    {
        bool IsThresholdReached();
    }

    public class AllocationRebalancingThreshold : IRebalancingThreshold
    {
        public AllocationRebalancingThreshold(
            double drift,
            IEnumerable<(string symbol, double weight)> currentAllocations,
            IEnumerable<(string symbol, double weight)> idealAllocations)
        {
            Drift = drift;
            CurrentAllocations = currentAllocations;
            IdealAllocations = idealAllocations;
        }

        public double Drift { get; }
        public IEnumerable<(string symbol, double weight)> CurrentAllocations { get; }
        public IEnumerable<(string symbol, double weight)> IdealAllocations { get; }

        public bool IsThresholdReached() => CalculateDrift() > Drift;

        private double CalculateDrift() => IdealAllocations
            .Sum(ia => Math.Abs(CurrentAllocations.SingleOrDefault(ca => ca.symbol == ia.symbol).weight))
            / 2;
    }

    public class FrequencyRebalancingThreshold : IRebalancingThreshold
    {
        public FrequencyRebalancingThreshold(TimeSpan timeSpan)
        {
            TimeSpan = timeSpan;
        }

        public TimeSpan TimeSpan { get; set; }
        public DateTime LastRebalancingDate { get; set; }

        public bool IsThresholdReached() =>
            LastRebalancingDate + TimeSpan >= DateTime.UtcNow;
    }
}