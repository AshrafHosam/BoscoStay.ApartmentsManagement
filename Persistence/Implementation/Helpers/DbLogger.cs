using Application.Contracts.Helpers;
using Domain.LogEntities;

namespace Persistence.Implementation.Helpers
{
    internal class DbLogger : IDbLogger
    {
        private readonly AppDbContext _context;
        public DbLogger(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveLog(ApiResponseLog logModel)
        {
            _context.Add(new ApiResponseLog
            {
                ResponseBody = logModel.ResponseBody,
                RequestBody = logModel.RequestBody,
                StatusCode = logModel.StatusCode,
                RequestName = logModel.RequestName
            });

            await _context.SaveChangesAsync();
        }
    }
}
