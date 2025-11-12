using Application.Contracts.Identity;
using Application.Response;
using MediatR;

namespace Application.Features.Identity.Commands.ChangePassword
{
    internal class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ApiResponse<ChangePasswordCommandResponse>>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IClaimService _claimService;

        public ChangePasswordCommandHandler(IAuthorizationService authorizationService, IClaimService claimService)
        {
            _authorizationService = authorizationService;
            _claimService = claimService;
        }

        public async Task<ApiResponse<ChangePasswordCommandResponse>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            string userId = _claimService.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return ApiResponse<ChangePasswordCommandResponse>.GetNotFoundApiResponse("User Not Logged In");

            return await _authorizationService.ChangePassword(Guid.Parse(userId), request);
        }
    }
}
