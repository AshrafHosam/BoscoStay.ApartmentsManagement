using Domain.Enums;

namespace Application.Features.Apartments.Queries.GetApartment
{
    public class GetApartmentQueryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int Floor { get; set; } = 0;
        public NoiseLevelEnum NoiseLevel { get; set; } = NoiseLevelEnum.NA;
        public double DistanceToCenterInKm { get; set; } = 0.0;
        public bool IsVisible { get; set; } = true;
        public double AreaInSquareMeters { get; set; } = 0.0;
        public bool IsFurnished { get; set; } = false;
        public double? PricePerDay { get; set; }
        public double? PricePerMonth { get; set; }
    }
}