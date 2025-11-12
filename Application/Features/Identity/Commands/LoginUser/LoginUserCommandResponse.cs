
namespace Application.Features.Identity.Commands.LoginUser
{
    public class LoginUserCommandResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}
