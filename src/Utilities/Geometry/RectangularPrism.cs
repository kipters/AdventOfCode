namespace Utilities.Geometry;

public record Box(int Length, int Height, int Width)
{
    public int Area { get; } =
        2 * Length * Width +
        2 * Height * Length +
        2 * Width * Height;

    public int[] SideAreas { get; } = [
        Length * Width,
        Height * Length,
        Width * Height
    ];

    public int[] SidePerimeters { get; } = [
        2 * (Length + Width),
        2 * (Height + Length),
        2 * (Width + Height)
    ];

    public int Volume = Length * Width * Height;
}
