using Application.Contracts.Identity;
using Identity.Implementation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Identity
{
    public static class IdentityServiceRegistration
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IClaimService, ClaimService>();

            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddJwtBearerTokenAndIdentityCookie(configuration);
            return services;
        }

        private static void AddJwtBearerTokenAndIdentityCookie(this IServiceCollection services, IConfiguration configuration)
        {
            var secretKey = configuration.GetSection("JwtConfiguration:SecretKey").Value;
            var key = Encoding.ASCII.GetBytes(secretKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                    //ClockSkew = TimeSpan.FromDays(1),
                };
            })
            .AddCookie(IdentityConstants.ApplicationScheme, delegate (CookieAuthenticationOptions o)
            {
                o.LoginPath = new PathString("/Users/LoginUser");
                CookieAuthenticationEvents events1 = new CookieAuthenticationEvents();
                events1.OnValidatePrincipal = new Func<CookieValidatePrincipalContext, Task>(SecurityStampValidator.ValidatePrincipalAsync);
                o.Events = events1;
            }
            );
        }
    }
}
