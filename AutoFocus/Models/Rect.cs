namespace AutoFocus.Models
{
    public struct Rect
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        
        public Rect(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        
        public override string ToString()
        {
            return $"X:{X:F2}, Y:{Y:F2}, W:{Width:F2}, H:{Height:F2}";
        }
    }
}