using Application.Response;
using MediatR;

namespace Application.Features.Apartments.Queries.GetApartment
{
    public class GetApartmentQuery : IRequest<ApiResponse<GetApartmentQueryResponse>>
    {
        public Guid Id { get; set; }
    }
}
