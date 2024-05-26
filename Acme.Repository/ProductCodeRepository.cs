using Acme.Core.Interfaces;

namespace Acme.Repository
{
    public class ProductCodeRepository : IProductCodeRepository
    {
        private readonly HashSet<string> _productCodes;

        public ProductCodeRepository()
        {
            _productCodes = new HashSet<string>();
        }

        public Task<bool> IsExist(string productCode)
        {
            return Task.FromResult(_productCodes.Contains(productCode));
        }

        public Task SaveProductCode(string productCode)
        {
            _productCodes.Add(productCode);
            return Task.CompletedTask;
        }
    }
}