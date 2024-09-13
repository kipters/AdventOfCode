using System.Reflection;
using Xunit.Sdk;

namespace Xunit;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class FileDataAttribute(string filename, params object[] passthrough) : DataAttribute
{
    private bool _isSlow;
    public bool IsSlow
    {
        get => _isSlow;
        init
        {
            _isSlow = value;
            Skip = value && Environment.GetEnvironmentVariable("RUN_SLOW_TESTS") is null ? "Slow test" : Skip;
        }
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        ArgumentNullException.ThrowIfNull(testMethod);
        var typeName = testMethod.DeclaringType!.Name;
        var path = Path.Combine(typeName, filename);
        var content = File.ReadAllText(path);
        yield return [content, .. passthrough];
    }

    public string Filename => filename;
    public object[] Passthrough => passthrough;
}
