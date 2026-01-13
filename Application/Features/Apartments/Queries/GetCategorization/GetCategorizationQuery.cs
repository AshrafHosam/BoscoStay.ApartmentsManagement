using Application.Response;
using MediatR;

namespace Application.Features.Apartments.Queries.GetCategorization
{
    public class GetCategorizationQuery : IRequest<ApiResponse<GetCategorizationQueryResponse>>
    {
    }
}
