using Application.Contracts.Identity;
using Application.Contracts.Services;
using Application.Response;
using MediatR;

namespace Application.Features.Identity.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ApiResponse<LoginUserCommandResponse>>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IEmailService _emailService;
        public LoginUserCommandHandler(IAuthorizationService authorizationService, IEmailService emailService)
        {
            _authorizationService = authorizationService;
            _emailService = emailService;
        }

        public async Task<ApiResponse<LoginUserCommandResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.LoginAsync(request);

            return result;
        }
    }
}
