namespace Application.Contracts.Helpers
{
    internal interface ICacheableRequest
    {
        public string GetCacheKey();
        public double GetCacheDurationInMinutes();
    }
}
