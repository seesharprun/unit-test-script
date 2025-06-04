#:package MSTest@3.*
#:package YamlDotNet@16.*

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Reflection;
using System.Text;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

[TestClass]
public class CalculatorTests
{
    [TestMethod]
    [DynamicData(
        dynamicDataSourceName: nameof(TestData.GetTestData),
        dynamicDataDeclaringType: typeof(TestData),
        dynamicDataSourceType: DynamicDataSourceType.Method,
        DynamicDataDisplayName = nameof(TestData.GetTestDisplayName),
        DynamicDataDisplayNameDeclaringType = typeof(TestData))]
    public void AddIntegers_FromDataRowTest(int x, int y, int z)
    {
        // Act
        int a = x;
        int b = y;
        int expected = z;

        // Act
        int actual = Calculator.Add(a, b);

        // Assert
        Assert.AreEqual(expected, actual, "The sum of {0} and {1} should be {2}.", a, b, expected);
    }
}

record TestData
{
    public int? Left { get; set; }
    public int? Right { get; set; }
    public int? Sum { get; set; }

    public (int, int, int) ToTuple()
    {
        return (Left ?? 0, Right ?? 0, Sum ?? 0);
    }

    public static IEnumerable<(int, int, int)> GetTestData()
    {
        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(
            Assembly.GetExecutingAssembly().GetManifestResourceNames().FirstOrDefault(
                name => name.Contains("data.yml", StringComparison.OrdinalIgnoreCase)
            )!
        )!;
        using StreamReader reader = new(stream);
        using TextReader textReader = reader;
        IEnumerable<TestData> data = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build()
            .Deserialize<IEnumerable<TestData>>(textReader);
        return data.Select(d => d.ToTuple());
    }

    public static string GetTestDisplayName(MethodInfo methodInfo, object[] data) =>
        BuildDisplayName(methodInfo.Name, (int)data[0], (int)data[1], (int)data[2]);

    private static string BuildDisplayName(string methodName, int left, int right, int sum) =>
        $"{methodName.ToLowerInvariant().Replace('_', '-')}-{left:00}-{right:00}-{sum:00}";
}

class Calculator
{
    public static int Add(int a, int b) => a + b;
}