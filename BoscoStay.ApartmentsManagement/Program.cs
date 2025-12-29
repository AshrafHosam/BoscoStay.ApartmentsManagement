using Application;
using Identity;
using Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Application.Middlewares;

namespace BoscoStay.ApartmentsManagement
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
            });

            ApplicationServiceRegistration.AddApplicationServices(builder.Services);

            IdentityServiceRegistration.AddIdentityServices(builder.Services, builder.Configuration);

            PersistenceServiceRegistration.AddPersistenceServices(builder.Services, builder.Configuration);

            var app = builder.Build();


            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var migrator = dbContext.GetService<IMigrator>();

                await migrator.MigrateAsync("20251229192804_apartmentTable_remove_nullablePrice_remove_pricePerMonth_column");
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BoscoStay API");
                });
            }

            app.UseCors(corsPolicyBuilder =>
                corsPolicyBuilder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );
#if DEBUG
            //app.UseHttpsRedirection();
#endif
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.UseMiddleware<ExceptionMiddleware>();

            app.Run();
        }
    }
}
