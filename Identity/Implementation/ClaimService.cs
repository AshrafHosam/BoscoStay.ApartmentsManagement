using Application.Contracts.Helpers;
using Application.Contracts.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Identity.Implementation
{
    public class ClaimService : IClaimService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IEncryptionHelper _encryptionHelper;
        public ClaimService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IEncryptionHelper encryptionHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _encryptionHelper = encryptionHelper;
        }

        public string GetUserId()
        {
            var claimValue = GetClaim(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(claimValue))
                return null;

            return _encryptionHelper.Decrypt(claimValue, _configuration.GetSection("Encryption:Key").Value);
        }

        public string GetClaim(string key)
            => _httpContextAccessor.HttpContext?.User?.FindFirst(key)?.Value;
    }
}
