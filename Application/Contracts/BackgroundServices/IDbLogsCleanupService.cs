namespace Application.Contracts.BackgroundServices
{
    public interface IDbLogsCleanupService
    {
        Task<int> SuccessLogsCleanup();
        Task<int> FailureLogsCleanup();
    }
}
