using Domain.Enums;

namespace Application.Features.Apartments.Queries.GetCategorization
{
    public class GetCategorizationQueryResponse
    {
        public List<CategorizationDto> CategorizedApartments { get; set; }
    }
    public class CategorizationDto
    {
        public Guid ApartmentId { get; set; }
        public string Category { get; set; }
    }
}