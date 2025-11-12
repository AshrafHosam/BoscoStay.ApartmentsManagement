using Application.Contracts.Repos;
using Application.Response;
using Domain.Entities;
using MediatR;

namespace Application.Features.Apartments.Commands.UpdateApartment
{
    internal class UpdateApartmentCommandHandler(IBaseRepo<Apartment> _apartmentRepo)
        : IRequestHandler<UpdateApartmentCommand, ApiResponse<UpdateApartmentCommandResponse>>
    {
        public async Task<ApiResponse<UpdateApartmentCommandResponse>> Handle(UpdateApartmentCommand request, CancellationToken cancellationToken)
        {
            var apartmentExist = await _apartmentRepo.AnyAsync(request.Id);
            if (!apartmentExist)
                return ApiResponse<UpdateApartmentCommandResponse>.GetNotFoundApiResponse("Apartment not found");

            var apartment = await _apartmentRepo.GetAsync(request.Id);
            apartment.Name = request.Name;
            apartment.Description = request.Description;
            apartment.Address = request.Address;
            apartment.Floor = request.Floor;
            apartment.NoiseLevel = request.NoiseLevel;
            apartment.DistanceToCenterInKm = request.DistanceToCenterInKm;
            apartment.PricePerDay = request.PricePerDay;
            apartment.PricePerMonth = request.PricePerMonth;

            await _apartmentRepo.UpdateAsync(apartment);

            //send to queue

            return ApiResponse<UpdateApartmentCommandResponse>.GetSuccessApiResponse(new());
        }
    }
}
