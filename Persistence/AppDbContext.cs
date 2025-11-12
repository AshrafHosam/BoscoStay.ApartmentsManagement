using Application.Contracts.Identity;
using Domain.Common;
using Domain.Entities;
using Domain.LogEntities;
using Identity.Entities;
using Identity.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Persistence.Configurations;
using System.Linq.Expressions;

namespace Persistence
{
    public class AppDbContext : DbContext
    //IdentityDbContext<AppUser>
    {
        private readonly IClaimService _claimService;
        public AppDbContext(DbContextOptions options, IClaimService claimService) : base(options)
        {
            _claimService = claimService;
        }
        //public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<ApiResponseLog> ApiResponseLogs { get; set; }
        public DbSet<Apartment> Apartments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetCustomAttributes(typeof(IAuditable), true).Length > 0)
                {
                    builder.Entity(entityType.Name).Property<bool>("IsDeleted");
                }

                var isActiveProperty = entityType.FindProperty("IsDeleted");
                if (isActiveProperty != null && isActiveProperty.ClrType == typeof(bool))
                {
                    var entityBuilder = builder.Entity(entityType.ClrType);
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var methodInfo = typeof(EF).GetMethod(nameof(EF.Property))!.MakeGenericMethod(typeof(bool))!;
                    var efPropertyCall = Expression.Call(null, methodInfo, parameter, Expression.Constant("IsDeleted"));
                    var body = Expression.MakeBinary(ExpressionType.Equal, efPropertyCall, Expression.Constant(false));
                    var expression = Expression.Lambda(body, parameter);
                    entityBuilder.HasQueryFilter(expression);
                }
            }

            //Seed(builder);

            AddEntitiesConfigurations(builder);

            base.OnModelCreating(builder);
        }

        private void AddEntitiesConfigurations(ModelBuilder builder)
        {
            builder.AddApiResponseLogConfiguration();
        }

        private void Seed(ModelBuilder modelBuilder)
        {
            SeedRoles(modelBuilder);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = "c7b013f0-5201-4317-abd8-c211f91b7330",
                    Name = nameof(UserRolesEnum.Owner),
                    NormalizedName = nameof(UserRolesEnum.Owner).ToUpper()
                });
        }

        public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditable>())
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTimeOffset.UtcNow;
                        entry.Entity.CreatedBy = _claimService.GetUserId();
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = DateTimeOffset.UtcNow;
                        entry.Entity.UpdatedBy = _claimService.GetUserId();
                        break;
                    case EntityState.Deleted:
                        entry.Entity.DeletedDate = DateTimeOffset.UtcNow;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedBy = _claimService.GetUserId();
                        entry.State = EntityState.Modified;
                        break;
                }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
