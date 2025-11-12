using Application.Features.Identity.Commands.ChangePassword;
using Application.Features.Identity.Commands.LoginUser;
using Application.Response;

namespace Application.Contracts.Identity
{
    public interface IAuthorizationService
    {
        //Task<ConfirmEmailResponseModel> ConfirmEmailAsync(ConfirmEmailModel confirmEmailModel);


        Task<ApiResponse<LoginUserCommandResponse>> LoginAsync(LoginUserCommand loginUserModel);

        string GenerateJwtToken(string userId, string userName, string email, List<string> roles);
        Task<ApiResponse<ChangePasswordCommandResponse>> ChangePassword(Guid userId, ChangePasswordCommand request);
    }
}
