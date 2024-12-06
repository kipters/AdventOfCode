using System.Buffers;
using System.Numerics;
using System.Threading.Tasks.Dataflow;
using Utilities.Extensions;
using Utilities.Movement;

namespace Year2024;

public class Day6
{
    [Theory]
    [FileLines("data_sample.txt", 41)]
    [FileLines("data.txt", 4559)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var map = data.ToArray();

        var initialPosition = map
            .Select((l, i) => (x: l.IndexOf('^'), y: i))
            .First(t => t.x != -1)
            .Then(t => new Vector2(t.x, t.y))
            .Then(ImmutableCartesianPlane.FromPosition);

        var tracedSteps = WalkRoute(map, initialPosition)
            .Select(p => p.Position)
            .Distinct()
            .ToHashSet();

        _ = tracedSteps.Add(initialPosition.Position);
        Assert.Equal(result, tracedSteps.Count);
    }

    private static IEnumerable<ImmutableCartesianPlane> WalkRoute(string[] map, ImmutableCartesianPlane initialPosition)
    {
        var maxX = map[0].Length;
        var maxY = map.Length;
        return initialPosition
            .TransformWhile(
                pos => WalkNextStep(pos, map),
                pos => IsOnTheEdge(pos, maxX, maxY)
            );
    }

    private static IEnumerable<ImmutableCartesianPlane> WalkRouteWithTrap(string[] map, ImmutableCartesianPlane initialPosition, Vector2 trap)
    {
        var maxX = map[0].Length;
        var maxY = map.Length;
        return initialPosition
            .TransformWhile(
                pos => WalkNextStepWithTrap(pos, map, trap),
                pos => IsOnTheEdge(pos, maxX, maxY)
            );
    }

    private static bool IsOnTheEdge(
        ImmutableCartesianPlane cur,
        int maxX,
        int maxY) => cur
            .LookAhead()
            .Then(p => p.X < maxX && p.Y < maxY && p.X >= 0 && p.Y >= 0);

    private static ImmutableCartesianPlane WalkNextStep(ImmutableCartesianPlane pos, string[] map)
    {
        var ahead = pos.LookAhead();
        var nextChar = map[(int)ahead.Y][(int)ahead.X];
        return nextChar switch
        {
            '#' => pos.Turn(Utilities.TurnDirection.Right),
            _ => pos.Walk(1)
        };
    }

    private static ImmutableCartesianPlane WalkNextStepWithTrap(ImmutableCartesianPlane pos, string[] map, Vector2 trap)
    {
        var ahead = pos.LookAhead();
        var nextChar = ahead == trap ? '#' : map[(int)ahead.Y][(int)ahead.X];
        return nextChar switch
        {
            '#' => pos.Turn(Utilities.TurnDirection.Right),
            _ => pos.Walk(1)
        };
    }

    [Theory]
    [FileLines("data_sample.txt", 6)]
    [FileLines("data.txt", 1604)]
    public void Part2_Linear(IEnumerable<string> data, int result)
    {
        var map = data.ToArray();
        var maxX = map[0].Length - 1;
        var maxY = map.Length - 1;

        var initialPosition = map
            .Select((l, i) => (x: l.IndexOf('^'), y: i))
            .First(t => t.x != -1)
            .Then(t => new Vector2(t.x, t.y))
            .Then(ImmutableCartesianPlane.FromPosition);

        var validPositions = 0.CountTo(maxX)
            .CartesianProduct(0.CountTo(maxY))
            .Where(c => map[c.y][c.x] == '.')
            .Count(c => IsLoop(c, map, initialPosition));

        Assert.Equal(result, validPositions);
    }

    private static bool IsLoop((int x, int y) c, string[] map, ImmutableCartesianPlane initialPosition)
    {
        var mapSize = map.Length * map[0].Length;
        var steps = WalkRouteWithTrap(map, initialPosition, new(c.x, c.y))
            .Take(mapSize + 1)
            .GroupBy(_ => _)
            .Where(g => g.Count() >= 2)
            .FirstOrDefault();

        return steps is not null;
    }

    [Theory]
    [FileLines("data_sample.txt", 6)]
    [FileLines("data.txt", 1604)]
    public async Task Part2_Dataflow(IEnumerable<string> data, int result)
    {
        var map = data.ToArray();
        var maxX = map[0].Length - 1;
        var maxY = map.Length - 1;

        var initialPosition = map
            .Select((l, i) => (x: l.IndexOf('^'), y: i))
            .First(t => t.x != -1)
            .Then(t => new Vector2(t.x, t.y))
            .Then(ImmutableCartesianPlane.FromPosition);

        var concurrent = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
        var routerBlock = new TransformBlock<(int x, int y), bool>(c => IsLoop(c, map, initialPosition), concurrent);

        var validPositions = 0.CountTo(maxX)
            .CartesianProduct(0.CountTo(maxY))
            .Where(c => map[c.y][c.x] == '.');

        foreach (var p in validPositions)
        {
            var accepted = routerBlock.Post(p);
            if (!accepted)
            {
                throw new Exception("An item was discarded");
            }
        }

        routerBlock.Complete();

        var count = 0;
        await foreach (var r in routerBlock.ReceiveAllAsync())
        {
            count += r ? 1 : 0;
        }

        Assert.Equal(result, count);
    }
}
