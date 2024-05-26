using Acme.Core.Interfaces;
using Acme.Core.Models;

namespace Acme.Services
{
    public class ReceiptProcessor : IReceiptProcessor
    {

        public List<OrderedTextLine> ProcessReceipt(Receipt receipt)
        {
            // use v1 for word ordering
            // use v4 for line ordering
            // same line words v4.y can be spase in 10 pixels

            // Group the text blocks into lines based on the fourth vertex (bottom right) and a tolerance of 10 units in Y-axis
            var lines = new List<List<TextBlock>>();
            var currentLine = new List<TextBlock>();

            var textBlocks = receipt.TextBlocks.OrderBy(tb => tb.BoundingPoly.Vertices[3].Y).ToList();

            int currentY = textBlocks.First().BoundingPoly.Vertices[3].Y;
            foreach (var textBlock in textBlocks)
            {
                int y4 = textBlock.BoundingPoly.Vertices[3].Y;

                if (y4 > currentY + 10)
                {
                    lines.Add(currentLine);
                    currentLine = new List<TextBlock>();
                    currentY = y4;
                }

                currentLine.Add(textBlock);
            }

            if (currentLine.Count > 0)
            {
                lines.Add(currentLine);
            }

            // Order each line by X coordinate and produce OrderedTextLine objects
            var orderedLines = new List<OrderedTextLine>();
            int lineNumber = 1;
            foreach (var line in lines)
            {
                var orderedLineText = string.Join(" ", line.OrderBy(tb => tb.BoundingPoly.Vertices[3].X).Select(tb => tb.Description));
                orderedLines.Add(new OrderedTextLine
                {
                    LineNumber = lineNumber++,
                    Text = orderedLineText
                });
            }

            return orderedLines;
        }
    }
}