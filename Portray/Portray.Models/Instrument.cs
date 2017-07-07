using System;
using System.Collections.Generic;
using System.Linq;

namespace Portray.Models
{
    public class Instrument
    {
        public Instrument(string symbol)
        {
            Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        }

        public string Symbol { get; set; }
        public override string ToString() => $"Type: {GetType().Name}, Symbol: {Symbol}";

        public IEnumerable<Allocation> Allocations { get; set; }
        public IEnumerable<AssetClassAllocation> AssetClassAllocations { get; set; }
        public IEnumerable<SectorAllocation> SectorAllocations { get; set; }
        public IEnumerable<GeographicAllocation> GeographicAllocations { get; set; }

        public IEnumerable<Allocation> GetAllocationLeaves() => GetAllocationLeaves(this, 1)
            .OrderByDescending(a => a.weight)
            .Select(a => new Allocation(a.instrument, a.weight));

        public IEnumerable<AssetClassAllocation> GetAssetClassAllocations() => GetAllocationLeaves(this, 1)
            .SelectMany(a =>
                a.instrument.HasAssetClassAllocations
                    ? (a.instrument.AssetClassAllocations.Select(aca => (code: aca.Code, weight: a.weight * aca.Weight)))
                    : Enumerable.Empty<(AssetClassCode code, double weight)>())
            .GroupBy(a => a.code, s => s.weight)
            .Select(g => new AssetClassAllocation(g.Key, g.Sum()))
            .OrderByDescending(a => a.Weight);

        public IEnumerable<SectorAllocation> GetSectorAllocations() => GetAllocationLeaves(this, 1)
            .SelectMany(a =>
                a.instrument.HasSectorAllocations
                    ? a.instrument.SectorAllocations.Select(sa => (code: sa.Code, weight: a.weight * sa.Weight))
                    : Enumerable.Empty<(SectorCode code, double weight)>())
            .GroupBy(sa => sa.code, s => s.weight)
            .Select(g => new SectorAllocation(g.Key, g.Sum()))
            .OrderByDescending(sa => sa.Weight);

        public IEnumerable<GeographicAllocation> GetGeographicAllocations() => GetAllocationLeaves(this, 1)
            .SelectMany(a =>
                a.instrument.HasGeographicAllocations
                    ? a.instrument.GeographicAllocations?.Select(ga => (code: ga.Code, weight: a.weight * ga.Weight))
                    : Enumerable.Empty<(GeographyCode code, double weight)>())
            .GroupBy(ga => ga.code, s => s.weight)
            .Select(g => new GeographicAllocation(g.Key, g.Sum()))
            .OrderByDescending(ga => ga.Weight);

        protected IEnumerable<(Instrument instrument, double weight)> GetAllocationLeaves(Instrument instrument, double weight) =>
            (instrument.HasConstituents
                ? instrument.Allocations.SelectMany(a => GetAllocationLeaves(a.Instrument, weight * a.Weight))
                : new(Instrument, double)[1] { (instrument, weight) })
            .GroupBy(a => a.instrument, s => s.weight, Comparer)
            .Select(g => (g.Key, g.Sum()));

        protected bool HasConstituents => Allocations != null && Allocations.Any();
        protected bool HasAssetClassAllocations => AssetClassAllocations != null && AssetClassAllocations.Any();
        protected bool HasSectorAllocations => SectorAllocations != null && SectorAllocations.Any();
        protected bool HasGeographicAllocations => GeographicAllocations != null && GeographicAllocations.Any();

        protected InstrumentComparer Comparer { get; set; } = new InstrumentComparer();
    }

    public class InstrumentComparer : IEqualityComparer<Instrument>
    {
        public bool Equals(Instrument x, Instrument y) =>
            x.Symbol == y.Symbol;

        public int GetHashCode(Instrument obj) =>
            obj.Symbol.GetHashCode();
    }
}
