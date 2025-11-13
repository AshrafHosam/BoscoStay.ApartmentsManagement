using Application.Response;
using Domain.Enums;
using MediatR;

namespace Application.Features.Apartments.Queries.SearchApartments
{
    public class SearchApartmentsQuery : IRequest<ApiResponse<SearchApartmentsQueryResponse>>
    {
        public string SearchText { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 100;
        public bool? IsFurnished { get; set; }
        public double? MaxPrice { get; set; }
        public NoiseLevelEnum? NoiseLevel { get; set; }
        public double? MaxArea { get; set; }
        public double? MaxDistanceToCenter { get; set; }
    }
}
