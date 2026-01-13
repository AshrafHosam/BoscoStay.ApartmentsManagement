using Application.Contracts.Repos;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Implementation.Repos
{
    public class BaseRepo<T> : IBaseRepo<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        public BaseRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task BeginTransaction()
            => await _context.Database.BeginTransactionAsync();

        public async Task CommitTransaction()
            => await _context.Database.CommitTransactionAsync();

        public async Task RollbackTransaction()
            => await _context.Database.RollbackTransactionAsync();

        /// <summary>
        /// Add an entity to database and start tracking it
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<T> AddAsync(T item)
        {
            await _context.Set<T>().AddAsync(item);
            await _context.SaveChangesAsync();
            return item;

        }
        /// <summary>
        /// Add entities to database and start tracking them
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> items)
        {
            await _context.Set<T>().AddRangeAsync(items);
            await _context.SaveChangesAsync();

            return items;
        }
        /// <summary>
        /// Check if an entity exists with the given Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> AnyAsync(Guid Id)
        {
            return await _context.Set<T>().AnyAsync(a => a.Id == Id);
        }
        /// <summary>
        /// (Soft) Delete an entity from the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task DeleteAsync(T item)
        {
            _context.Set<T>().Remove(item);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// (Soft) Delete an entity from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Guid id)
        {
            var item = await _context.Set<T>().FindAsync(id);
            _context.Set<T>().Remove(item);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Get latest 50 entities
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>()
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
        }
        /// <summary>
        /// Get a tracked entity by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(Guid Id)
        {
            return await _context.Set<T>().FindAsync(Id);
        }
        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<T> UpdateAsync(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }
    }
}
