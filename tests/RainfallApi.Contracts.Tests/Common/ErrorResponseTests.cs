using System.Reflection;

using RainfallApi.Contracts.Common;

namespace RainfallApi.Contracts.Tests.Common;

public class ErrorResponseTests
{
    [Fact]
    public void When_Instantiated_Expect_AdditionalPropertiesIsFalse()
    {
        // Arrange
        var message = "testMessage";
        var details = new ErrorDetail[0];

        // Act
        var response = new ErrorResponse(message, details);

        // Assert
        response.AdditionalProperties.Should().BeFalse();
    }

    [Fact]
    public void Given_MessageIsNull_When_Instantiated_Expect_TargetInvocationException()
    {
        // Arrange
        string? message = null;
        var errors = new ErrorDetail[0];

        Func<object?> newInstanceCreator = () => Activator.CreateInstance(typeof(ErrorResponse), message, errors);

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
        string? message = null;
        var errors = new ErrorDetail[0];

        Func<object?> newInstanceCreator = () => Activator.CreateInstance(typeof(ErrorResponse), message, errors);

        // Act
        var exception = Record.Exception(newInstanceCreator);

        // Assert
        Assert.NotNull(exception);
        Assert.IsAssignableFrom<Exception>(exception);
        Assert.NotNull(exception.InnerException);
        Assert.IsType<ArgumentNullException>(exception.InnerException);
    }

    [Fact]
    public void Given_ErrorsIsNull_When_Instantiated_Expect_TargetInvocationException()
    {
        // Arrange
        string message = "testMessage";
        ErrorDetail[]? errors = null;

        Func<object?> newInstanceCreator = () => Activator.CreateInstance(typeof(ErrorResponse), message, errors);

        // Act
        var exception = Record.Exception(newInstanceCreator);

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<TargetInvocationException>(exception);
    }

    [Fact]
    public void Given_ErrorsIsNull_When_Instantiated_Expect_Exception_With_InnerExceptionAsArgumentNullException()
    {
        // Arrange
        string message = "testMessage";
        ErrorDetail[]? errors = null;

        Func<object?> newInstanceCreator = () => Activator.CreateInstance(typeof(ErrorResponse), message, errors);

        // Act
        var exception = Record.Exception(newInstanceCreator);

        // Assert
        Assert.NotNull(exception);
        Assert.IsAssignableFrom<Exception>(exception);
        Assert.NotNull(exception.InnerException);
        Assert.IsType<ArgumentNullException>(exception.InnerException);
    }
}