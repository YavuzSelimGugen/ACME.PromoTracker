using Acme.Core.Interfaces;
using Acme.Core.Models;
using Newtonsoft.Json;

namespace Acme.Services
{
    public class OcrTestService : IOcrService
    {
        public async Task<Receipt> ParseReceiptAsync(byte[] fileBytes)
        {
            var filePath = Path.Combine("Sample", "response.json");
            var jsonString = await File.ReadAllTextAsync(filePath);
            var textBlocks = JsonConvert.DeserializeObject<List<TextBlock>>(jsonString);

            // remove first decription element
            // first element is main frame which allocates whole text as one rectangle
            var firstElementIndexWhichHasLocale = textBlocks.FindIndex(x => x.IsMainFrame());
            textBlocks.RemoveAt(firstElementIndexWhichHasLocale);

            return new Receipt { TextBlocks = textBlocks };
        }
    }
}