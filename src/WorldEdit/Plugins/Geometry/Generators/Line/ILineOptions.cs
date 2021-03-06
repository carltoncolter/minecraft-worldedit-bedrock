namespace ShapeGenerator
{
    public interface ILineOptions
    {
        int X { get; set; }
        int Y { get; set; }
        int Z { get; set; }

        int X2 { get; set; }
        int Y2 { get; set; }
        int Z2 { get; set; }

        Point Start { get; set; }
        Point End { get; set;  }
    }
}