using Domain.LogEntities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configurations
{
    internal static class ApiResponseLogConfigurations
    {
        public static void AddApiResponseLogConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiResponseLog>(c =>
            {
                c.HasIndex(u => u.StatusCode);
            });
        }
    }
}
