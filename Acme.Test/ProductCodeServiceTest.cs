using Acme.Repository;
using Acme.Services;

namespace Acme.Test
{
    public class ProductCodeServiceTest
    {

        [Theory]
        [InlineData(1000)]
        public async Task GenerateUniqueCode_ForMultipleTimes_ReturnsUniqueCode(int testSize)
        {
            // Arrange
            var productCodeService = GetProductCodeService();
            var generatedCodes = new HashSet<string>();

            // Act
            for (int i = 0; i < testSize; i++)
            {
                var generated = await productCodeService.GenerateUniqueCode();
                generatedCodes.Add(generated);
            }

            // Assert
            foreach (var item in generatedCodes)
            {
                Assert.True(productCodeService.ValidateCode(item));
            }
        }

        [Fact]
        public void ValidateCode_WithoutChecksumList_ReturnsFalse()
        {
            // Arrange
            var productCodeService = GetProductCodeService();

            var nonGeneratedCodeList = new Dictionary<string, bool>
            {
                {"xiHDTLGF", false},
                {"Oj2Zh8DI", false},
                {"nhPtzeI7", false},
                {"0cJepSFn", false},
                {"DJ4ftRcN", false},
                {"RsV8uPXN", false}
            };

            // Act
            foreach (var item in nonGeneratedCodeList)
            {
                nonGeneratedCodeList[item.Key] = productCodeService.ValidateCode(item.Key);
            }

            // Assert
            foreach (var item in nonGeneratedCodeList)
            {
                Assert.False(item.Value);
            }
        }

        [Fact]
        public async Task ValidateCode_ForGenerateUniqueCode_ReturnsTrue()
        {
            // Arrange
            var productCodeService = GetProductCodeService();

            // Act
            var generated = await productCodeService.GenerateUniqueCode();

            // Assert
            productCodeService.ValidateCode(generated);
        }


        private ProductCodeService GetProductCodeService()
        {
            var repository = new ProductCodeRepository();
            var cipherService = new CipherService();
            return new ProductCodeService(repository, cipherService);
        }
    }
}