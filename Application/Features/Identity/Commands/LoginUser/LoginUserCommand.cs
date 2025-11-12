using Application.Response;
using MediatR;

namespace Application.Features.Identity.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<ApiResponse<LoginUserCommandResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
