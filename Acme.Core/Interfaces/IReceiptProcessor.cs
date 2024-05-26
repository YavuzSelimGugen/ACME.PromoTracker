using Acme.Core.Models;

namespace Acme.Core.Interfaces
{
    public interface IReceiptProcessor
    {
        List<OrderedTextLine> ProcessReceipt(Receipt receipt);
    }
}