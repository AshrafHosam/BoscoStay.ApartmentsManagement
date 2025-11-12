using Application.Contracts.Helpers;
using Application.Contracts.Identity;
using Application.Contracts.Services;
using Application.Features.Identity.Commands.ChangePassword;
using Application.Features.Identity.Commands.LoginUser;
using Application.Response;
using Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Implementation
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private const string TemplatePassword = "Hello@123";
        private readonly IEmailService _emailService;
        private readonly IEncryptionHelper _encryptionHelper;
        public AuthorizationService(IConfiguration configuration, SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
            IEmailService emailService, IEncryptionHelper encryptionHelper)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _encryptionHelper = encryptionHelper;
        }

        public string GenerateJwtToken(string userId, string userName, string email, List<string> roles)
        {
            var secretKey = _configuration.GetSection("JwtConfiguration:SecretKey").Value;
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenHandler = new JwtSecurityTokenHandler();

            var userClaims = roles.Select(a => new Claim(ClaimTypes.Role, a)).ToList();

            userClaims.AddRange(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email,email),
            });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<ApiResponse<LoginUserCommandResponse>> LoginAsync(LoginUserCommand loginUserModel)
        {
            var user = await _userManager.FindByEmailAsync(loginUserModel.Email);

            if (user == null)
                return ApiResponse<LoginUserCommandResponse>
                    .GetNotFoundApiResponse(error: "Username or password is incorrect");

            var roles = (await _userManager.GetRolesAsync(user)).ToList();

            var signInResult = await _signInManager.PasswordSignInAsync(user, loginUserModel.Password, false, false);

            if (!signInResult.Succeeded)
                return ApiResponse<LoginUserCommandResponse>
                    .GetNotFoundApiResponse(error: "Username or password is incorrect");

            var encryptionKey = _configuration.GetSection("Encryption:Key").Value;

            return ApiResponse<LoginUserCommandResponse>
                .GetSuccessApiResponse(new LoginUserCommandResponse
                {
                    Email = user.Email,
                    Username = user.UserName,
                    Roles = roles,
                    Token = GenerateJwtToken(_encryptionHelper.Encrypt(user.Id.ToString(), encryptionKey), user.UserName, user.Email, roles)
                });
        }

        public async Task<ApiResponse<ChangePasswordCommandResponse>> ChangePassword(Guid userId, ChangePasswordCommand request)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return ApiResponse<ChangePasswordCommandResponse>.GetNotFoundApiResponse("User Not Found");

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
                return ApiResponse<ChangePasswordCommandResponse>.GetNotFoundApiResponse((result.Errors.FirstOrDefault()?.Description));

            return ApiResponse<ChangePasswordCommandResponse>.GetSuccessApiResponse(new ChangePasswordCommandResponse
            {
                Message = "Password Changed Successfully"
            });
        }

    }
}
