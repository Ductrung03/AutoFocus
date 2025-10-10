/// <summary>
/// Represents an image file with focus information
/// </summary>
namespace AutoFocus.Models
{

    public class ImageData
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int FocusIndex { get; set; }
        public double FocusScore { get; set; }
        public DateTime LastModified { get; set; }
        public long FileSize { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int BitDepth { get; set; }

        public override string ToString()
        {
            return $"[{FocusIndex:D3}] {FileName} ({FocusScore:F2})";
        }
    }
}