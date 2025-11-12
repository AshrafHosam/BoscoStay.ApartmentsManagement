namespace Application.Contracts.Helpers
{
    internal interface ICacheableResponse
    {
        public bool IsCacheable { get; set; }
    }
}
