using Application.Contracts.Repos;
using Application.Response;
using Domain.Entities;
using MediatR;

namespace Application.Features.Apartments.Queries.GetApartment
{
    internal class GetApartmentQueryHandler(IBaseRepo<Apartment> _apartmentRepo)
        : IRequestHandler<GetApartmentQuery, ApiResponse<GetApartmentQueryResponse>>
    {
        public async Task<ApiResponse<GetApartmentQueryResponse>> Handle(GetApartmentQuery request, CancellationToken cancellationToken)
        {
            var apartment = await _apartmentRepo.GetAsync(request.Id);
            if (apartment is null)
                return ApiResponse<GetApartmentQueryResponse>.GetNotFoundApiResponse("Apartment not found");

            return ApiResponse<GetApartmentQueryResponse>.GetSuccessApiResponse(new()
            {
                Id = apartment.Id,
                Address = apartment.Address,
                AreaInSquareMeters = apartment.AreaInSquareMeters,
                Description = apartment.Description,
                DistanceToCenterInKm = apartment.DistanceToCenterInKm,
                Floor = apartment.Floor,
                IsFurnished = apartment.IsFurnished,
                IsVisible = apartment.IsVisible,
                Name = apartment.Name,
                NoiseLevel = apartment.NoiseLevel,
                PricePerDay = apartment.PricePerDay,
                PricePerMonth = apartment.PricePerMonth
            });
        }
    }
}
