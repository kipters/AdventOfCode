using Utilities.Geometry;

namespace Utilities.Tests.Geometry;

public class LineTests
{
    [Fact]
    public void VerticalPoints()
    {
        var line = new Line((1, 1), (1, 3));
        Point[] points = [(1, 1), (1, 2), (1, 3)];

        Assert.Equal(points, line.Points());
    }

    [Fact]
    public void HorizontalPoints()
    {
        var line = new Line((9, 7), (7, 7));
        Point[] points = [(9, 7), (8, 7), (7, 7)];

        Assert.Equal(points, line.Points());
    }

    [Fact]
    public void DiagonalPoints()
    {
        var line = new Line((1, 1), (3, 3));
        Point[] points = [(1, 1), (2, 2), (3, 3)];

        Assert.Equal(points, line.Points());
    }

    [Fact]
    public void DiagonalPoints2()
    {
        var line = new Line((9, 7), (7, 9));
        Point[] points = [(9, 7), (8, 8), (7, 9)];

        Assert.Equal(points, line.Points());
    }
}
