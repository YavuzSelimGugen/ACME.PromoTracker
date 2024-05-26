using Acme.Core.Models;

namespace Acme.Core.Interfaces
{
    public interface IOcrService
    {
        Task<Receipt> ParseReceiptAsync(byte[] fileBytes);
    }
}