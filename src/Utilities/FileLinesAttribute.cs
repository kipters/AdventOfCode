using System.Reflection;
using Xunit.Sdk;

namespace Xunit;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class FileLinesAttribute(string filename, params object[] passthrough) : DataAttribute
{
    public bool IsSlow
    {
        get;
        init
        {
            field = value;
            Skip = value && Environment.GetEnvironmentVariable("RUN_SLOW_TESTS") is null ? "Slow test" : Skip;
        }
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        ArgumentNullException.ThrowIfNull(testMethod);
        var typeName = testMethod.DeclaringType!.Name;
        var path = Path.Combine(typeName, filename);

        yield return [ReadLines(path), .. passthrough];
    }

    private static IEnumerable<string> ReadLines(string filePath)
    {
        using var file = File.OpenRead(filePath);
        using var reader = new StreamReader(file);

        while (true)
        {
            var line = reader.ReadLine();

            if (line is not null)
            {
                yield return line.Trim();
            }
            else
            {
                yield break;
            }
        }
    }

    public string Filename => filename;
    public object[] Passthrough => passthrough;
}
