using Application.Contracts.Repos;
using Application.Features.Apartments.Queries.SearchApartments;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Implementation.Repos
{
    internal class ApartmentRepo : BaseRepo<Apartment>, IApartmentRepo
    {
        public ApartmentRepo(AppDbContext context) : base(context)
        {
        }

        public async Task<int> CountSearch(SearchApartmentsQuery query)
            => await _context.Apartments
                .FilterIf(query.IsFurnished.HasValue, a => a.IsFurnished == query.IsFurnished.Value)
                .FilterIf(query.NoiseLevel.HasValue, a => a.NoiseLevel == query.NoiseLevel.Value)
                .FilterIf(query.MaxArea.HasValue, a => a.AreaInSquareMeters <= query.MaxArea.Value)
                .FilterIf(query.MaxDistanceToCenter.HasValue, a => a.DistanceToCenterInKm <= query.MaxDistanceToCenter.Value)
                .FilterIf(query.MaxPrice.HasValue, a => (a.PricePerDay.HasValue && a.PricePerDay.Value <= query.MaxPrice.Value) ||
                        (a.PricePerMonth.HasValue && a.PricePerMonth.Value <= query.MaxPrice.Value))
                .FilterIf(!string.IsNullOrEmpty(query.SearchText),
                a => (EF.Functions.ILike(a.Name, $"%{query.SearchText}%")
                || EF.Functions.ILike(a.Description, $"%{query.SearchText}%")
                || EF.Functions.ILike(a.Address, $"%{query.SearchText}%")))
                .CountAsync();

        public async Task<List<Apartment>> Search(SearchApartmentsQuery query)
            => await _context.Apartments
                .FilterIf(query.IsFurnished.HasValue, a => a.IsFurnished == query.IsFurnished.Value)
                .FilterIf(query.NoiseLevel.HasValue, a => a.NoiseLevel == query.NoiseLevel.Value)
                .FilterIf(query.MaxArea.HasValue, a => a.AreaInSquareMeters <= query.MaxArea.Value)
                .FilterIf(query.MaxDistanceToCenter.HasValue, a => a.DistanceToCenterInKm <= query.MaxDistanceToCenter.Value)
                .FilterIf(query.MaxPrice.HasValue, a => (a.PricePerDay.HasValue && a.PricePerDay.Value <= query.MaxPrice.Value) ||
                        (a.PricePerMonth.HasValue && a.PricePerMonth.Value <= query.MaxPrice.Value))
                .FilterIf(!string.IsNullOrEmpty(query.SearchText),
                a => (EF.Functions.ILike(a.Name, $"%{query.SearchText}%")
                || EF.Functions.ILike(a.Description, $"%{query.SearchText}%")
                || EF.Functions.ILike(a.Address, $"%{query.SearchText}%")))
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToListAsync();
    }
}
