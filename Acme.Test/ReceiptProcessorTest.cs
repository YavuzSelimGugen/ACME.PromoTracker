using Acme.Core.Models;
using Acme.Services;
using System.Text;

namespace Acme.Test
{
    public class ReceiptProcessorTest
    {
        [Fact]
        public async Task ProcessReceipt_WithSampleResponse_ReturnsOrderedTextLines()
        {
            // Arrange
            var ocrService = new OcrService();
            var receiptProcessor = new ReceiptProcessor();
            var receipt = await ocrService.ParseReceiptAsync(Encoding.UTF8.GetBytes("test"));

            var expectedSampleResponse = new List<OrderedTextLine>
            {
                new OrderedTextLine
                {
                    LineNumber = 1,
                    Text = "TEŞEKKÜRLER"
                },
                new OrderedTextLine
                {
                    LineNumber = 2,
                    Text = "GUNEYDOĞU TEKS. GIDA INS SAN. LTD.STI."
                },
                new OrderedTextLine
                {
                    LineNumber = 14,
                    Text = "MANAV DOMATES PETEME *3,32"
                },
                new OrderedTextLine
                {
                    LineNumber = 27,
                    Text = "00 0001/020/000084/1//82/"
                },
            };

            // Act
            var result = receiptProcessor.ProcessReceipt(receipt);

            // Assert
            foreach (var item in expectedSampleResponse)
            {
                var resultItem = result.FirstOrDefault(x => x.LineNumber == item.LineNumber);
                Assert.Equal(item.Text, resultItem.Text);
                Assert.Equal(item.LineNumber, resultItem.LineNumber);
            }
        }
    }
}
