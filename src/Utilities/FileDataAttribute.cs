using System.Reflection;
using Xunit.Sdk;

namespace Xunit;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class FileDataAttribute(string filename, params object[] passthrough) : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var typeName = testMethod.DeclaringType!.Name;
        var path = Path.Combine(typeName, filename);
        var content = File.ReadAllText(path);
        yield return [content, .. passthrough];
    }
}
