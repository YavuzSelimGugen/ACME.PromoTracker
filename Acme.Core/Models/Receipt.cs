
namespace Acme.Core.Models
{
    public class Receipt
    {
        public List<TextBlock>? TextBlocks { get; set; }
    }

    public class Vertex
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class BoundingPoly
    {
        public List<Vertex> Vertices { get; set; } = null!;
    }

    public class TextBlock
    {
        public string? Locale { get; set; }
        public string? Description { get; set; }
        public BoundingPoly BoundingPoly { get; set; } = null!;

        public bool IsMainFrame()
        {
            return !string.IsNullOrEmpty(this.Locale);
        }
    }

    public class OrderedTextLine
    {
        public int LineNumber { get; set; }
        public string Text { get; set; }
    }
}