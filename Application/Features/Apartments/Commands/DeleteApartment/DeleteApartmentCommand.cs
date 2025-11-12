using Application.Response;
using MediatR;

namespace Application.Features.Apartments.Commands.DeleteApartment
{
    public class DeleteApartmentCommand : IRequest<ApiResponse<DeleteApartmentCommandResponse>>
    {
        public Guid Id { get; set; }
    }
}
