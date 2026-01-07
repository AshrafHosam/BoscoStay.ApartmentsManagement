using Application.Contracts.Helpers;
using Application.Contracts.Repos;
using Application.Contracts.Services;
using Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Implementation.BackgroundServices;
using Persistence.Implementation.Helpers;
using Persistence.Implementation.Repos;
using Persistence.Implementation.Services;
using RabbitMQ.Client;

namespace Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration _config)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(_config.GetConnectionString("Default"),
                    opt => opt.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            services.AddScoped<AppDbContext>();

            services.AddRepositories();

            services.AddServices();

            services.AddBackgroundServices();

            services.AddIdentity();

            services.AddFactories();

            services.AddHelpers();

            services.AddRabbitMQConnection(_config);

            return services;
        }

        private static void AddHelpers(this IServiceCollection services)
        {
            services.AddScoped<IDbLogger, DbLogger>();
            services.AddScoped<IEncryptionHelper, EncryptionHelper>();
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepo<>), typeof(BaseRepo<>));

            services.AddScoped<IApartmentRepo, ApartmentRepo>();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IQueueService, RabbitQueueService>();
        }

        private static void AddBackgroundServices(this IServiceCollection services)
        {
            //services.AddHostedService<DbLogsCleanupService>();
            services.AddHostedService<RabbitMQConfigurationsService>();
        }

        private static void AddFactories(this IServiceCollection services)
        {
        }

        private static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentityCore<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddSignInManager<SignInManager<AppUser>>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = true;
            });
        }

        public static void AddRabbitMQConnection(this IServiceCollection services, IConfiguration _config)
        {
            services.AddSingleton<IConnection>(sp =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = _config.GetValue<string>("RabbitMQ:Host") ?? "host.docker.internal",
                    UserName = _config.GetValue<string>("RabbitMQ:Username") ?? "guest",
                    Password = _config.GetValue<string>("RabbitMQ:Password") ?? "guest",
                    VirtualHost = _config.GetValue<string>("RabbitMQ:VirtualHost") ?? "/"
                };
                return factory.CreateConnectionAsync().Result;
            });

            services.AddScoped<IChannel>(sp =>
            {
                var connection = sp.GetRequiredService<IConnection>();
                return connection.CreateChannelAsync().Result;
            });
        }
    }
}
