using Application.Contracts.Repos;
using Application.Response;
using Domain.Entities;
using MediatR;

namespace Application.Features.Apartments.Commands.CreateApartment
{
    internal class CreateApartmentCommandHandler(IBaseRepo<Apartment> _apartmentRepo)
        : IRequestHandler<CreateApartmentCommand, ApiResponse<CreateApartmentCommandResponse>>
    {
        public async Task<ApiResponse<CreateApartmentCommandResponse>> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
        {
            await _apartmentRepo.AddAsync(new Apartment
            {
                Address = request.Address,
                AreaInSquareMeters = request.AreaInSquareMeters,
                Description = request.Description,
                DistanceToCenterInKm = request.DistanceToCenterInKm,
                Floor = request.Floor,
                Name = request.Name,
                NoiseLevel = request.NoiseLevel,
                PricePerDay = request.PricePerDay,
                PricePerMonth = request.PricePerMonth
            });

            //send to queue

            return ApiResponse<CreateApartmentCommandResponse>.GetCreatedApiResponse();
        }
    }
}
