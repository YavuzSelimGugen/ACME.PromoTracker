﻿using Acme.Core.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Acme.Services
{
    public class ProductCodeService : IProductCodeService
    {
        private static readonly string _characterSet = "ACDEFGHKLMNPRTXYZ234579";
        private static readonly int _codeLength = 8;
        private static readonly int _checksumLength = 2;
        private static readonly int _mainCodeLength = _codeLength - _checksumLength;
        private static readonly Regex _codeRegex = new Regex($"^[{_characterSet}]+$");
        private readonly IProductCodeRepository _productCodeRepository;

        public ProductCodeService(IProductCodeRepository productCodeRepository)
        {
            _productCodeRepository = productCodeRepository;
        }

        public async Task<string> GenerateUniqueCode()
        {
            for (int attempt = 0; attempt < 100; attempt++) // Allow up to 100 attempts
            {
                var mainCode = GenerateRandomCode(_mainCodeLength);
                var checksum = GenerateChecksum(mainCode);
                var code = mainCode + checksum;

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

            var mainCode = code.Substring(0, _mainCodeLength);
            var providedChecksum = code.Substring(_mainCodeLength, _checksumLength);
            var expectedChecksum = GenerateChecksum(mainCode);

            return providedChecksum == expectedChecksum;
        }

        private string GenerateRandomCode(int length)
        {
            var codeChars = new char[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                for (int i = 0; i < length; i++)
                {
                    int index = randomBytes[i] % _characterSet.Length;
                    codeChars[i] = _characterSet[index];
                }
            }
            return new string(codeChars);
        }

        private string GenerateChecksum(string input)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            var checksumChars = new char[_checksumLength];

            for (int i = 0; i < _checksumLength; i++)
            {
                int index = hash[i] % _characterSet.Length;
                checksumChars[i] = _characterSet[index];
            }
            return new string(checksumChars);
        }
    }
}