using Acme.Services;
using System.Text;

namespace Acme.Test
{
    public class OcrServiceTest
    {
        [Fact]
        public async Task ParseReceiptAsync_WithValidFile_ReturnsReceipt()
        {
            // Arrange
            var fileBytes = Encoding.UTF8.GetBytes("test");
            var ocrService = new OcrTestService();

            // Act
            var result = await ocrService.ParseReceiptAsync(fileBytes);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.TextBlocks);
            Assert.NotEmpty(result.TextBlocks);
            Assert.True(!string.IsNullOrEmpty(result.TextBlocks[0].Description));
            Assert.NotNull(result.TextBlocks[0].BoundingPoly.Vertices);
        }

        [Fact]
        public async Task ParseReceipt_WithValidFile_DoNotParseMainFrame()
        {
            // Arrange
            var fileBytes = Encoding.UTF8.GetBytes("test");
            var ocrService = new OcrTestService();

            // Act
            var result = await ocrService.ParseReceiptAsync(fileBytes);

            // Assert
            Assert.Null(result.TextBlocks.FirstOrDefault(x => x.IsMainFrame()));
        }
    }
}