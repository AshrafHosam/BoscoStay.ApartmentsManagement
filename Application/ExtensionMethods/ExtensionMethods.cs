using System.Linq.Expressions;

namespace Application.Contracts.Repos
{
    public static class ExtensionMethods
    {
        public static IQueryable<TEntity> FilterIf<TEntity>(this IQueryable<TEntity> query, bool ifCondition, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            if (ifCondition)
                return query.Where(predicate);
            else
                return query;
        }
    }
}
