using System.Reflection;

using RainfallApi.Contracts.Common;

namespace RainfallApi.Contracts.Tests.Common;

public class ErrorDetailTests
{
    [Fact]
    public void When_Instantiated_Expect_AdditionalPropertiesIsFalse()
    {
        // Arrange
        var propertyName = "testProperty";
        var message = "testMessage";

        // Act
        var detail = new ErrorDetail(propertyName, message);

        // Assert
        detail.AdditionalProperties.Should().BeFalse();
    }

    [Fact]
    public void Given_PropertyNameIsNull_When_Instantiated_Expect_TargetInvocationException()
    {
        // Arrange
        string? propertyName = null;
        string message = "testMessage";

        Func<object?> newInstanceCreator = () => Activator.CreateInstance(typeof(ErrorDetail), propertyName, message);

        // Act
        var exception = Record.Exception(newInstanceCreator);

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<TargetInvocationException>(exception);
    }

    [Fact]
    public void Given_PropertyNameIsNull_When_Instantiated_Expect_Exception_With_InnerExceptionAsArgumentNullException()
    {
        // Arrange
        string? propertyName = null;
        string message = "testMessage";

        Func<object?> newInstanceCreator = () => Activator.CreateInstance(typeof(ErrorDetail), propertyName, message);

        // Act
        var exception = Record.Exception(newInstanceCreator);

        // Assert
        Assert.NotNull(exception);
        Assert.IsAssignableFrom<Exception>(exception);
        Assert.NotNull(exception.InnerException);
        Assert.IsType<ArgumentNullException>(exception.InnerException);
    }

    [Fact]
    public void Given_MessageIsNull_When_Instantiated_Expect_TargetInvocationException()
    {
        // Arrange
        string propertyName = "testMessage";
        string? message = null;

        Func<object?> newInstanceCreator = () => Activator.CreateInstance(typeof(ErrorDetail), propertyName, message);

        // Act
        var exception = Record.Exception(newInstanceCreator);

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<TargetInvocationException>(exception);
    }

    [Fact]
    public void Given_MessageIsNull_When_Instantiated_Expect_Exception_With_InnerExceptionAsArgumentNullException()
    {
        // Arrange
        string propertyName = "testMessage";
        string? message = null;

        Func<object?> newInstanceCreator = () => Activator.CreateInstance(typeof(ErrorDetail), propertyName, message);

        // Act
        var exception = Record.Exception(newInstanceCreator);

        // Assert
        Assert.NotNull(exception);
        Assert.IsAssignableFrom<Exception>(exception);
        Assert.NotNull(exception.InnerException);
        Assert.IsType<ArgumentNullException>(exception.InnerException);
    }
}
