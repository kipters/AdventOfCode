namespace Utilities.Geometry;

public readonly record struct Line(Point A, Point B)
{
    public bool Vertical { get; } = A.X == B.X;
    public bool Horizontal { get; } = A.Y == B.Y;
    public IEnumerable<Point> Points()
    {
        return this switch
        {
            _ when A == B => [A],
            { Vertical: true } => VerticalPoints(this),
            { Horizontal: true } => HorizontalPoints(this),
            _ => DiagonalPoints(this)
        };
    }

    private static IEnumerable<Point> VerticalPoints(Line line)
    {
        var verticalDelta = line.A.Y > line.B.Y ? -1 : 1;
        var delta = new Point(0, verticalDelta);

        var p = line.A;
        yield return p;
        while (p != line.B)
        {
            p += delta;
            yield return p;
        }
    }

    private static IEnumerable<Point> HorizontalPoints(Line line)
    {
        var horizontalDelta = line.A.X > line.B.X ? -1 : 1;
        var delta = new Point(horizontalDelta, 0);

        var p = line.A;
        yield return p;
        while (p != line.B)
        {
            p += delta;
            yield return p;
        }
    }

    private static IEnumerable<Point> DiagonalPoints(Line line)
    {
        var horizontalDelta = line.A.X > line.B.X ? -1 : 1;
        var verticalDelta = line.A.Y > line.B.Y ? -1 : 1;
        var delta = new Point(horizontalDelta, verticalDelta);
        var n = Math.Abs(line.A.X - line.B.X) + 1;

        return Enumerable
            .Range(0, n)
            .Select(c => line.A + delta * c);
    }
}
