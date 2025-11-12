using Application.Features.Apartments.Queries.SearchApartments;
using Domain.Entities;

namespace Application.Contracts.Repos
{
    public interface IApartmentRepo : IBaseRepo<Apartment>
    {
        Task<int> CountSearch(SearchApartmentsQuery query);
        Task<List<Apartment>> Search(SearchApartmentsQuery query);
    }
}
