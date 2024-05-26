using Acme.Services;

namespace Acme.Test
{
    public class ProductCodeServiceTest
    {
        [Fact]
        public void ValidateCode_WithoutChecksumList_ReturnsFalse()
        {
            // Arrange
            var productCodeService = new ProductCodeService();
            
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
        public void ValidateCode_ForGenerateUniqueCode_ReturnsTrue()
        {
            // Arrange
            var productCodeService = new ProductCodeService();
            var generatedCodes = new Dictionary<string, bool>();
            var testSize = 10;
            
            for (int i = 0; i < testSize; i++)
            {
                var generated = productCodeService.GenerateUniqueCode();
                generatedCodes.Add(generated, false);
            }

            // Act
            foreach (var item in generatedCodes)
            {
                generatedCodes[item.Key] = productCodeService.ValidateCode(item.Key);
            }
            // Assert
            foreach (var item in generatedCodes)
            {
                Assert.True(item.Value);
            }
        }
    }
}