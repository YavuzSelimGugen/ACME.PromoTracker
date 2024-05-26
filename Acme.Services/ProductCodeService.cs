using Acme.Core.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Acme.Services
{
    public class ProductCodeService : IProductCodeService
    {
        private readonly IProductCodeRepository _productCodeRepository;
        private CipherService _cipherService;
        
        private static readonly string _characterSet = "ACDEFGHKLMNPRTXYZ234579";
        private static readonly int _codeLength = 8;
        private static readonly Regex _codeRegex = new Regex($"^[{_characterSet}]+$");


        public ProductCodeService(IProductCodeRepository productCodeRepository, CipherService cipherService)
        {
            _productCodeRepository = productCodeRepository;
            _cipherService = cipherService;
        }

        public async Task<string> GenerateUniqueCode()
        {
            for (int attempt = 0; attempt < 100; attempt++) // Allow up to 100 attempts
            {
                var code = _cipherService.GenerateCode(_codeLength, _characterSet);

                if (await _productCodeRepository.IsExist(code) == false)
                {
                    await _productCodeRepository.SaveProductCode(code);
                    return code;
                }
            }
            throw new Exception("Failed to generate a unique code after multiple attempts.");
        }

        public bool ValidateCode(string code)
        {

            if (code.Length != _codeLength || !_codeRegex.IsMatch(code))
                return false;

            return _cipherService.ValidateCode(code, _codeLength, _characterSet);
        }
    }
}