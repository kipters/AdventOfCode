namespace Year2019;

public class Day2
{
    [Theory]
    [FileData("data.txt", 5110675)]
    public void Part1(string data, int result)
    {
        var program = data
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        program[1] = 12;
        program[2] = 2;

        RunProgram(program, 10);

        Assert.Equal(result, program[0]);
    }

    [Theory]
    [FileData("data_sample.txt", 0, 3500)]
    [InlineData("1,0,0,0,99", 0, 2)]
    [InlineData("2,3,0,3,99", 3, 6)]
    [InlineData("2,4,4,5,99,0", 5, 9801)]
    [InlineData("1,1,1,4,99,5,6,0,99", 0, 30)]
    public void SampleProgram(string data, int position, int result)
    {
        var program = data
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        RunProgram(program, 10);

        Assert.Equal(result, program[position]);
    }

    private static void RunProgram(int[] program, int timeoutUs)
    {
        var pc = 0;
        var cts = new CancellationTokenSource(TimeSpan.FromMicroseconds(timeoutUs));
        while (!cts.IsCancellationRequested)
        {
            switch (program[pc++])
            {
                case 1:
                    var a = program[program[pc++]];
                    var b = program[program[pc++]];
                    program[program[pc++]] = a + b;
                    break;

                case 2:
                    var x = program[program[pc++]];
                    var y = program[program[pc++]];
                    program[program[pc++]] = x * y;
                    break;

                case 99:
                    cts.Cancel();
                    break;
            }
        }
    }

    [Theory]
    [FileData("data.txt", 4847)]
    public void Part2(string data, int result)
    {
        var program = data
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();
        var (verb, noun) = BruteForceParams(program);

        var output = 100 * noun + verb;

        Assert.Equal(result, output);

        static (int verb, int noun) BruteForceParams(int[] program)
        {
            for (var noun = 0; noun <= 99; noun++)
            {
                for (var verb = 0; verb <= 99; verb++)
                {
                    var copy = new int[program.Length];
                    program.CopyTo(copy, 0);

                    copy[1] = noun;
                    copy[2] = verb;

                    RunProgram(copy, 10);

                    if (copy[0] == 19690720)
                    {
                        return (verb, noun);
                    }
                }
            }

            throw new Exception("Not Found!");
        }
    }
}
