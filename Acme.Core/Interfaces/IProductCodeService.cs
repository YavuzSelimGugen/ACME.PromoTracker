namespace Acme.Core.Interfaces
{
    public interface IProductCodeService
    {
        Task<string> GenerateUniqueCode();

        bool ValidateCode(string code);
    }
}