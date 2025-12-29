using Application.Response;
using Domain.Enums;
using MediatR;

namespace Application.Features.Apartments.Commands.UpdateApartment
{
    public class UpdateApartmentCommand : IRequest<ApiResponse<UpdateApartmentCommandResponse>>
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
        public double PricePerDay { get; set; }
    }
}
