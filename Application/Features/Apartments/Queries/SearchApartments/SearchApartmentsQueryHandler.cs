using Application.Contracts.Repos;
using Application.Response;
using MediatR;

namespace Application.Features.Apartments.Queries.SearchApartments
{
    internal class SearchApartmentsQueryHandler(IApartmentRepo _apartmentRepo)
        : IRequestHandler<SearchApartmentsQuery, ApiResponse<SearchApartmentsQueryResponse>>
    {
        public async Task<ApiResponse<SearchApartmentsQueryResponse>> Handle(SearchApartmentsQuery request, CancellationToken cancellationToken)
        {
            var count = await _apartmentRepo.CountSearch(request);
            var apartments = await _apartmentRepo.Search(request);

            return ApiResponse<SearchApartmentsQueryResponse>.GetSuccessApiResponse(new()
            {
                HasNextPage = request.PageNumber * request.PageSize < count,
                Apartments = apartments.Select(a => new ApartmentDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Address = a.Address,
                    Description = a.Description,
                    AreaInSquareMeters = a.AreaInSquareMeters,
                    DistanceToCenterInKm = a.DistanceToCenterInKm,
                    Floor = a.Floor,
                    IsFurnished = a.IsFurnished,
                    IsVisible = a.IsVisible,
                    NoiseLevel = a.NoiseLevel,
                    PricePerDay = a.PricePerDay
                }).ToList()
            });
        }
    }
}
