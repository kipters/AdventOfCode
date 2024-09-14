namespace Year2015;

public class Day7
{
    [Theory]
    [FileLines("data.txt", 46065)]
    public void Part1(IEnumerable<string> data, int result)
    {
        Dictionary<string, Func<int>> nodes = [];
        Dictionary<string, int> cache = [];
        nodes = data
            .Select(l =>
            {
                var p = l.Split(' ');
                var key = p[^1];
                Func<int> func = p[0..^2] switch
                {
                    [string src] when int.TryParse(src, out var num) => () => num,
                    [string src] => () => nodes[src](),

                    [string left, "RSHIFT", string right] =>
                        () => (int.TryParse(left, out var leftNum) ? leftNum : nodes[left]()) >> (int.TryParse(right, out var rightNum) ? rightNum : nodes[right]()),

                    [string left, "LSHIFT", string right] =>
                        () => (int.TryParse(left, out var leftNum) ? leftNum : nodes[left]()) << (int.TryParse(right, out var rightNum) ? rightNum : nodes[right]()),

                    [string left, "OR", string right] =>
                        () => (int.TryParse(left, out var leftNum) ? leftNum : nodes[left]()) | (int.TryParse(right, out var rightNum) ? rightNum : nodes[right]()),

                    [string left, "AND", string right] =>
                        () => (int.TryParse(left, out var leftNum) ? leftNum : nodes[left]()) & (int.TryParse(right, out var rightNum) ? rightNum : nodes[right]()),

                    ["NOT", string src] => () => nodes[src]() ^ 1,

                    _ => throw new InvalidOperationException($"Unsupported: {l}")
                };
                var cachedFunc = () =>
                {
                    if (cache.TryGetValue(key, out var cached))
                    {
                        return cached;
                    }

                    cache[key] = func();
                    return cache[key];
                };

                return (key, func: cachedFunc);
            })
            .ToDictionary(t => t.key, t => t.func);

            var a = nodes["a"]();
            Assert.Equal(result, a);
    }

    [Theory]
    [FileLines("data.txt", 14134)]
    public void Part2(IEnumerable<string> data, int result)
    {
        Dictionary<string, Func<int>> nodes = [];
        Dictionary<string, int> cache = [];
        nodes = data
            .Select(l =>
            {
                var p = l.Split(' ');
                var key = p[^1];
                Func<int> func = p[0..^2] switch
                {
                    [string src] when int.TryParse(src, out var num) => () => num,
                    [string src] => () => nodes[src](),

                    [string left, "RSHIFT", string right] =>
                        () => (int.TryParse(left, out var leftNum) ? leftNum : nodes[left]()) >> (int.TryParse(right, out var rightNum) ? rightNum : nodes[right]()),

                    [string left, "LSHIFT", string right] =>
                        () => (int.TryParse(left, out var leftNum) ? leftNum : nodes[left]()) << (int.TryParse(right, out var rightNum) ? rightNum : nodes[right]()),

                    [string left, "OR", string right] =>
                        () => (int.TryParse(left, out var leftNum) ? leftNum : nodes[left]()) | (int.TryParse(right, out var rightNum) ? rightNum : nodes[right]()),

                    [string left, "AND", string right] =>
                        () => (int.TryParse(left, out var leftNum) ? leftNum : nodes[left]()) & (int.TryParse(right, out var rightNum) ? rightNum : nodes[right]()),

                    ["NOT", string src] => () => nodes[src]() ^ 1,

                    _ => throw new InvalidOperationException($"Unsupported: {l}")
                };
                var cachedFunc = () =>
                {
                    if (cache.TryGetValue(key, out var cached))
                    {
                        return cached;
                    }

                    cache[key] = func();
                    return cache[key];
                };

                return (key, func: cachedFunc);
            })
            .ToDictionary(t => t.key, t => t.func);

            var a = nodes["a"]();
            cache.Clear();
            nodes["b"] = () => a;
            a = nodes["a"]();
            Assert.Equal(result, a);
    }
}
