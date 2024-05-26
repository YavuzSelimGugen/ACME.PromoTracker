namespace Acme.Core.Interfaces
{
    public interface IProductCodeRepository
    {
        Task<bool> IsExist(string productCode);
        Task SaveProductCode(string productCode);
    }
}