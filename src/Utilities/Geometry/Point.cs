namespace Utilities.Geometry;

public readonly record struct Point(int X, int Y)
{
    public static Point operator +(Point left, Point right) => Add(left, right);
    public static Point operator *(Point point, int scalar) => Multiply(point, scalar);

    public static Point Add(Point left, Point right) =>
        new(left.X + right.X, left.Y + right.Y);

    public static Point Multiply(Point point, int scalar) =>
        new(point.X * scalar, point.Y * scalar);

    public static implicit operator Point((int x, int y) tuple) =>
        FromValueTuple(tuple);

    public static implicit operator (int x, int y)(Point point) =>
        ToValueTuple(point);

    public static Point FromValueTuple((int x, int y) tuple) => new(tuple.x, tuple.y);
    public static (int x, int y) ToValueTuple(Point point) => (point.X, point.Y);
}

