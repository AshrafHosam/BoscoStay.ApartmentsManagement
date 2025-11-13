using Application.Contracts.Repos;
using Application.Contracts.Services;
using Application.Response;
using Domain.Entities;
using MediatR;

namespace Application.Features.Apartments.Commands.CreateApartment
{
    internal class CreateApartmentCommandHandler(IBaseRepo<Apartment> _apartmentRepo, IQueueService _queueService)
        : IRequestHandler<CreateApartmentCommand, ApiResponse<CreateApartmentCommandResponse>>
    {
        public async Task<ApiResponse<CreateApartmentCommandResponse>> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
        {
            var apartment = await _apartmentRepo.AddAsync(new Apartment
            {
                Address = request.Address,
                AreaInSquareMeters = request.AreaInSquareMeters,
                Description = request.Description,
                DistanceToCenterInKm = request.DistanceToCenterInKm,
                Floor = request.Floor,
                Name = request.Name,
                NoiseLevel = request.NoiseLevel,
                PricePerDay = request.PricePerDay,
                PricePerMonth = request.PricePerMonth,
                IsFurnished = request.IsFurnished
            });

            await _queueService.PublishChange(apartment, Domain.Enums.ApartmentChangeEnum.Create);

            return ApiResponse<CreateApartmentCommandResponse>.GetCreatedApiResponse();
        }
    }
}
