namespace Application.Contracts.Helpers
{
    public interface IEncryptionHelper
    {
        public string Encrypt(string input, string key);
        public string Decrypt(string input, string key);
    }
}
