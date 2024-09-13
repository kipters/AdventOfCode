using Utilities.Extensions;

namespace Year2023;

public class Day5
{
    [Theory]
    [FileLines("data_sample.txt", 35)]
    [FileLines("data.txt", 196167384)]
    public void Part1(IEnumerable<string> data, uint result)
    {
        var seeds = data
            .First()[7..]
            .Split(' ')
            .Select(uint.Parse);

        var min = FindLowestLocation(data, seeds);

        Assert.Equal(result, min);
    }

    [Theory]
    [FileLines("data_sample.txt", 46)]
    [FileLines("data.txt", 125742456, IsSlow = true)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var seedsRanges = data
            .First()[7..]
            .Split(' ')
            .Select(uint.Parse)
            .Chunk(2)
            .Select(x => EnumerableExtensions.Range(x[0], x[1]))
            ;

        // var min = FindLowestLocation(data, seedsRanges);
        var min = seedsRanges
            .AsParallel()
            .Select(r => FindLowestLocation(data, r))
            .Min();

        Assert.Equal(result, min);
    }

    [Theory]
    [FileLines("data_sample.txt", 46)]
    [FileLines("data.txt", 125742456, IsSlow = true)]
    public void Part2_BottomUp(IEnumerable<string> data, int result)
    {
        var seeds = data
            .First()[7..]
            .Split(' ')
            .Select(uint.Parse)
            .Chunk(2)
            .Select(p => new MapRange(0, p[0], p[1]))
            .ToArray();

        var rawMaps = data
            .Skip(2)
            .ChunkBy("")
            .Select(c =>
            {
                var key = c[0]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[0];

                var maps = c.Skip(1)
                    .Select(l => l.Split(' ').Select(uint.Parse).ToArray())
                    .Select(t => new MapRange(t[1], t[0], t[2]))
                    .ToArray();

                return (key, maps);
            })
            .ToDictionary(x => x.key, x => x.maps);

        var soilMap = rawMaps["seed-to-soil"];
        var fertilizerMap = rawMaps["soil-to-fertilizer"];
        var waterMap = rawMaps["fertilizer-to-water"];
        var lightMap = rawMaps["water-to-light"];
        var temperatureMap = rawMaps["light-to-temperature"];
        var humidityMap = rawMaps["temperature-to-humidity"];
        var locationMap = rawMaps["humidity-to-location"];

        var min = EnumerableExtensions.Range(0u, 150_000_000)
            .Select(x => FindInMap(locationMap, x))
            .Select(x => FindInMap(humidityMap, x))
            .Select(x => FindInMap(temperatureMap, x))
            .Select(x => FindInMap(lightMap, x))
            .Select(x => FindInMap(waterMap, x))
            .Select(x => FindInMap(fertilizerMap, x))
            .Select(x => FindInMap(soilMap, x))
            .Where(x => seeds.Any(s => s.IsInSourceRange(x)))
            .First();
    }

    private static long FindLowestLocation(IEnumerable<string> data, IEnumerable<uint> seeds)
    {
        var rawMaps = data
            .Skip(2)
            .ChunkBy("")
            .Select(c =>
            {
                var key = c[0]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[0];

                var maps = c.Skip(1)
                    .Select(l => l.Split(' ').Select(uint.Parse).ToArray())
                    .Select(t => new MapRange(t[0], t[1], t[2]))
                    .ToArray();

                return (key, maps);
            })
            .ToDictionary(x => x.key, x => x.maps);

        var soilMap = rawMaps["seed-to-soil"];
        var fertilizerMap = rawMaps["soil-to-fertilizer"];
        var waterMap = rawMaps["fertilizer-to-water"];
        var lightMap = rawMaps["water-to-light"];
        var temperatureMap = rawMaps["light-to-temperature"];
        var humidityMap = rawMaps["temperature-to-humidity"];
        var locationMap = rawMaps["humidity-to-location"];

        var min = seeds
            .AsParallel()
            .Select(x => FindInMap(soilMap, x))
            .Select(x => FindInMap(fertilizerMap, x))
            .Select(x => FindInMap(waterMap, x))
            .Select(x => FindInMap(lightMap, x))
            .Select(x => FindInMap(temperatureMap, x))
            .Select(x => FindInMap(humidityMap, x))
            .Select(x => FindInMap(locationMap, x))
            .Min();
        return min;
    }

    private static uint FindInMap(MapRange[] map, uint value)
    {
        var range = map.FirstOrDefault(m => m.IsInSourceRange(value));
        return range is null ? value : range.MapToDestination(value);
    }

    private record MapRange(uint DestinationStart, uint SourceStart, uint Length)
    {
        private readonly uint _sourceEnd = SourceStart + Length;
        public bool IsInSourceRange(uint value) => value >= SourceStart && value < _sourceEnd;
        public uint MapToDestination(uint value) => DestinationStart + (value - SourceStart);
    }
}
