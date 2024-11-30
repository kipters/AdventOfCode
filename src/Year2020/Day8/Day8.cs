namespace Year2020;

public class Day8
{
    [Theory]
    [FileLines("data_sample.txt", 5)]
    [FileLines("data.txt", 1727)]
    public void Part1(string[] data, int result)
    {
        var acc = 0;
        var pc = 0;
        var visited = new HashSet<int>();

        for (;;)
        {
            if (!visited.Add(pc))
            {
                break;
            }

            var parts = data[pc].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var opcode = parts[0];
            var arg = int.Parse(parts[1]);

            (acc, pc) = opcode switch
            {
                "acc" => (acc + arg, pc + 1),
                "jmp" => (acc, pc + arg),
                "nop" => (acc, pc + 1),
                _ => throw new InvalidDataException($"Invalid opcode {opcode}")
            };
        }

        Assert.Equal(result, acc);
    }

    [Theory]
    [FileLines("data_sample.txt", 8)]
    [FileLines("data.txt", 552)]
    public void Part2(string[] data, int result)
    {
        var (ranToCompletion, acc) = Enumerable.Range(0, data.Length)
            .Select(i => RunWithMutation(data, i))
            .Where(r => r.ranToCompletion)
            .First();

        Assert.Equal(result, acc);
    }

    private static (bool ranToCompletion, int acc) RunWithMutation(string[] code, int mutation)
    {
        var (acc, pc) = (0, 0);
        var visited = new HashSet<int>();

        for (;;)
        {
            if (pc == code.Length)
            {
                return (true, acc);
            }

            if (!visited.Add(pc))
            {
                return (false, acc);
            }

            var parts = code[pc].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var opcode = parts[0];
            var arg = int.Parse(parts[1]);

            (acc, pc) = opcode switch
            {
                "acc" => (acc + arg, pc + 1),
                "jmp" when pc == mutation => (acc, pc + 1),
                "jmp" => (acc, pc + arg),
                "nop" when pc == mutation => (acc, pc + arg),
                "nop" => (acc, pc + 1),
                _ => throw new InvalidDataException($"Invalid opcode {opcode}")
            };
        }
    }
}
