using Application.Contracts.Helpers;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Behaviours
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IMemoryCache _cache;
        public CachingBehavior(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is ICacheableRequest cacheableQuery)
            {
                var cacheKey = cacheableQuery.GetCacheKey();
                if (_cache.TryGetValue(cacheKey, out TResponse cachedResult))
                {
                    return cachedResult;
                }

                var response = await next();

                if (response is ICacheableResponse cacheableResponse && cacheableResponse.IsCacheable)
                    _cache.Set(cacheKey, response, TimeSpan.FromMinutes(cacheableQuery.GetCacheDurationInMinutes()));

                return response;
            }

            return await next();
        }
    }
}
