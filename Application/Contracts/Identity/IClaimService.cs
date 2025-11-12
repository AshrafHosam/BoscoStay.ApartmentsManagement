namespace Application.Contracts.Identity
{
    public interface IClaimService
    {
        string GetUserId();
        string GetClaim(string key);
    }
}
