using Application.Contracts.Repos;
using Application.Response;
using Domain.Entities;
using MediatR;

namespace Application.Features.Apartments.Commands.DeleteApartment
{
    public class DeleteApartmentCommandHandler(IBaseRepo<Apartment> _apartmentRepo)
        : IRequestHandler<DeleteApartmentCommand, ApiResponse<DeleteApartmentCommandResponse>>
    {
        public async Task<ApiResponse<DeleteApartmentCommandResponse>> Handle(DeleteApartmentCommand request, CancellationToken cancellationToken)
        {
            var apartmentExist = await _apartmentRepo.AnyAsync(request.Id);
            if (!apartmentExist)
                return ApiResponse<DeleteApartmentCommandResponse>.GetNotFoundApiResponse("Apartment not found");

            await _apartmentRepo.DeleteAsync(request.Id);

            //send to queue

            return ApiResponse<DeleteApartmentCommandResponse>.GetNoContentApiResponse();
        }
    }
}
