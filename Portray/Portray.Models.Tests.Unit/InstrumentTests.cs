using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Portray.Models.Tests.Unit
{
    public class InstrumentTests
    {
        [Fact]
        public void test_allocation_calculations()
        {
            // arrange
            var child1 = new Instrument("child1")
            {
                Allocations = new List<Allocation>
                {
                    new Allocation(new Instrument("child1-1")
                    {
                        AssetClassAllocations = new List<AssetClassAllocation>
                        {
                            new AssetClassAllocation(AssetClassCode.Equities, 0.42),
                            new AssetClassAllocation(AssetClassCode.FixedIncome, 0.58)
                        }
                    }, 0.3),
                    new Allocation(new Instrument("child1-2")
                    {
                        AssetClassAllocations = new List<AssetClassAllocation>
                        {
                            new AssetClassAllocation(AssetClassCode.Equities, 0.78),
                            new AssetClassAllocation(AssetClassCode.FixedIncome, 0.22)
                        }
                    }, 0.7),
                }
            };
            var child2 = new Instrument("child2")
            {
                Allocations = new List<Allocation>
                {
                    new Allocation(new Instrument("child2-1")
                    {
                        AssetClassAllocations = new List<AssetClassAllocation>
                        {
                            new AssetClassAllocation(AssetClassCode.Equities, 0.37),
                            new AssetClassAllocation(AssetClassCode.FixedIncome, 0.63)
                        }
                    }, 0.4),
                    new Allocation(new Instrument("child2-2")
                    {
                        AssetClassAllocations = new List<AssetClassAllocation>
                        {
                            new AssetClassAllocation(AssetClassCode.Equities, 0.12),
                            new AssetClassAllocation(AssetClassCode.FixedIncome, 0.88)
                        }
                    }, 0.6),
                }
            };
            var child3 = new Instrument("child3")
            {
                AssetClassAllocations = new List<AssetClassAllocation>
                {
                    new AssetClassAllocation(AssetClassCode.Equities, 0.51),
                    new AssetClassAllocation(AssetClassCode.FixedIncome, 0.49)
                }
            };
            var duplicateChild3 = new Instrument("child3")
            {
                AssetClassAllocations = new List<AssetClassAllocation>
                {
                    new AssetClassAllocation(AssetClassCode.Equities, 0.51),
                    new AssetClassAllocation(AssetClassCode.FixedIncome, 0.49)
                }
            };

            var parent = new Instrument("parent")
            {
                Allocations = new List<Allocation>
                {
                    new Allocation(child1, 0.3),
                    new Allocation(child2, 0.5),
                    new Allocation(child3, 0.1),
                    new Allocation(duplicateChild3, 0.1)
                }
            };

            // act
            List<Allocation> allocs = parent.GetAllocationLeaves().ToList();
            List<AssetClassAllocation> acAllocs = parent.GetAssetClassAllocations().ToList();
            List<SectorAllocation> sAllocs = parent.GetSectorAllocations().ToList();
            List<GeographicAllocation> gAllocs = parent.GetGeographicAllocations().ToList();

            // assert
            Assert.NotNull(allocs);
            Assert.Equal("child2-2", allocs[0].Instrument.Symbol);
            Assert.Equal(0.5 * 0.6, allocs[0].Weight);
            Assert.Equal("child1-2", allocs[1].Instrument.Symbol);
            Assert.Equal(0.3 * 0.7, allocs[1].Weight);
            Assert.Equal("child2-1", allocs[2].Instrument.Symbol);
            Assert.Equal(0.5 * 0.4, allocs[2].Weight);
            Assert.Equal("child3", allocs[3].Instrument.Symbol);
            Assert.Equal(0.2, allocs[3].Weight);
            Assert.Equal("child1-1", allocs[4].Instrument.Symbol);
            Assert.Equal(0.3 * 0.3, allocs[4].Weight);

            Assert.NotNull(acAllocs);
            Assert.Equal(AssetClassCode.FixedIncome, acAllocs[0].Code);
            Assert.Equal((decimal)(0.3 * (0.3 * 0.58 + 0.7 * 0.22) + 0.5 * (0.4 * 0.63 + 0.6 * 0.88) + 0.2 * 0.49), (decimal)acAllocs[0].Weight);
            Assert.Equal(AssetClassCode.Equities, acAllocs[1].Code);
            Assert.Equal((decimal)(0.3 * (0.3 * 0.42 + 0.7 * 0.78) + 0.5 * (0.4 * 0.37 + 0.6 * 0.12) + 0.2 * 0.51), (decimal)acAllocs[1].Weight);

            Assert.NotNull(sAllocs);
            Assert.Empty(sAllocs);

            Assert.NotNull(gAllocs);
            Assert.Empty(gAllocs);
        }
    }
}
