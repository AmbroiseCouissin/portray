using Microsoft.AspNetCore.Mvc;
using Portray.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portray.Hosts.WebApp.Controllers
{
    [Route("api/[controller]")]
    public class AllocationController : Controller
    {
        private readonly IEnumerable<Instrument> _instruments = new List<Instrument>
        {
            new Instrument("child1")
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
            },
            new Instrument("child2")
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
            },
            new Instrument("child3")
            {
                AssetClassAllocations = new List<AssetClassAllocation>
                {
                    new AssetClassAllocation(AssetClassCode.Equities, 0.51),
                    new AssetClassAllocation(AssetClassCode.FixedIncome, 0.49)
                }
            }
        };

        [HttpPost]
        public async Task<IActionResult> GetAllocationsAsync([FromBody] IEnumerable<AllocationVm> allocationVms)
        {
            var instrument = new Instrument("")
            {
                Allocations = allocationVms.Select(a => new Allocation(
                    _instruments.SingleOrDefault(i => i.Symbol == a.Symbol),
                    a.Weight)).ToList()
            };

            return Ok(new AllocationOutputVm
            {
                AssetClassAllocations = instrument.GetAssetClassAllocations(),
                SectorAllocations = instrument.GetSectorAllocations(),
                GeographicAllocations = instrument.GetGeographicAllocations()
            });
        }
    } 

    public class AllocationOutputVm
    {
        public IEnumerable<AssetClassAllocation> AssetClassAllocations { get; set; }
        public IEnumerable<SectorAllocation> SectorAllocations { get; set; }
        public IEnumerable<GeographicAllocation> GeographicAllocations { get; set; }
    }

    public class AllocationVm
    {
        public string Symbol { get; set; }
        public double Weight { get; set; }
    }
}
