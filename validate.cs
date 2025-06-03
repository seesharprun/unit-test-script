#:package MSTest@3.*

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class CalculatorTests
{
    [TestMethod]
    public void TestAddSum()
    {
        // Arrange
        int a = 5;
        int b = 10;
        int expected = 15;

        // Act
        int actual = Calculator.Add(a, b);

        // Assert
        Assert.AreEqual(expected, actual, "The sum of {0} and {1} should be {2}.", a, b, expected);
    }

    [TestMethod]
    [DataRow(1, 1, 2)]
    [DataRow(2, 2, 4)]
    [DataRow(3, 3, 6)]
    [DataRow(0, 0, 1)]
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

class Calculator
{
    public static int Add(int a, int b) => a + b;
}