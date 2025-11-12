using Application.Response;
using MediatR;

namespace Application.Features.Identity.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<ApiResponse<ChangePasswordCommandResponse>>
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
