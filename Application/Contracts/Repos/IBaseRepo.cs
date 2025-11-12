namespace Application.Contracts.Repos
{
    public interface IBaseRepo<T> where T : class
    {
        Task<T> GetAsync(Guid Id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T item);
        Task<T> UpdateAsync(T item);
        Task DeleteAsync(T item);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> items);
        Task<bool> AnyAsync(Guid Id);
        Task DeleteAsync(Guid id);
        Task BeginTransaction();
        Task CommitTransaction();
        Task RollbackTransaction();
    }
}
