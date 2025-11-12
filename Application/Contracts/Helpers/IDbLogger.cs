using Domain.LogEntities;

namespace Application.Contracts.Helpers
{
    public interface IDbLogger
    {
        Task SaveLog(ApiResponseLog logModel);
    }
}
