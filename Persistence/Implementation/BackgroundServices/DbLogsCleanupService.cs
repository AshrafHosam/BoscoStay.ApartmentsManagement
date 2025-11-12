using Application.Contracts.BackgroundServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Persistence.Implementation.BackgroundServices
{
    internal class DbLogsCleanupService : BackgroundService, IDbLogsCleanupService
    {
        private readonly ILogger<IDbLogsCleanupService> _logger;
        private readonly AppDbContext _context;
        public DbLogsCleanupService(ILogger<IDbLogsCleanupService> logger, IServiceProvider app)
        {
            _context = app.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation($"Start Db Logs Cleanup Service: {DateTime.UtcNow}");

                    var rows = await SuccessLogsCleanup();

                    _logger.LogInformation($"Cleaned up {rows} - success rows");

                    rows = await FailureLogsCleanup();

                    _logger.LogInformation($"Cleaned up {rows} - failure rows");

                    _logger.LogInformation($"Finished Db Logs Cleanup Service: {DateTime.UtcNow}");
                }
                catch (Exception ex)
                {
                    _context.ApiResponseLogs.Add(new Domain.LogEntities.ApiResponseLog
                    {
                        CreatedBy = "DbLogsCleanupService",
                        RequestBody = "NA",
                        RequestName = "DbLogsCleanupService",
                        StatusCode = 500,
                        ResponseBody = $"Exception: {ex.Message} - InnerException: {ex.InnerException}",
                        IsDeleted = false
                    });
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        public async Task<int> SuccessLogsCleanup()
            => await _context.ApiResponseLogs
                .Where(a => a.StatusCode >= 200 && a.StatusCode <= 299
                         && a.CreatedDate < DateTimeOffset.UtcNow.AddHours(-24))
                .ExecuteDeleteAsync();

        public async Task<int> FailureLogsCleanup()
            => await _context.ApiResponseLogs
                .Where(a => a.StatusCode >= 300 && a.StatusCode <= 500
                         && a.CreatedDate < DateTimeOffset.UtcNow.AddDays(-7))
                .ExecuteDeleteAsync();

    }
}
