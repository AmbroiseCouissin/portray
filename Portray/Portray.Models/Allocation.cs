namespace Portray.Models
{
    public class Allocation
    {
        public Allocation(Instrument instrument, double weight)
        {
            Instrument = instrument;
            Weight = weight;
        }

        public Instrument Instrument { get; set; }
        public double Weight { get; set; }
    }

    public class AssetClassAllocation
    {
        public AssetClassAllocation(AssetClassCode code, double weight)
        {
            Code = code;
            Weight = weight;
        }

        public AssetClassCode Code { get; set; }
        public double Weight { get; set; }
    }

    public class SectorAllocation
    {
        public SectorAllocation(SectorCode code, double weight)
        {
            Code = code;
            Weight = weight;
        }

        public SectorCode Code { get; set; }
        public double Weight { get; set; }
    }

    public class GeographicAllocation
    {
        public GeographicAllocation(GeographyCode code, double weight)
        {
            Code = code;
            Weight = weight;
        }

        public GeographyCode Code { get; set; }
        public double Weight { get; set; }
    }
}
