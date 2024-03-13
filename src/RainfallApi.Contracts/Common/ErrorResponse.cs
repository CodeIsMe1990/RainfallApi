using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace RainfallApi.Contracts.Common;

/// <summary>
/// Details of a rainfall reading.
/// </summary>
public record ErrorResponse
{
    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the details of the error.
    /// </summary>
    [DisallowNull]
    public ErrorDetail[] Detail { get; }

    /// <summary>
    /// Gets a value indicating whether there are additional properties.
    /// </summary>
    [DefaultValue(false)]
    public bool AdditionalProperties { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorResponse"/> class with the specified message and details.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errors">The details of the error.</param>
    public ErrorResponse(string message, ErrorDetail[] errors)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Detail = errors ?? throw new ArgumentNullException(nameof(errors));
        AdditionalProperties = false;
    }
}

/// <summary>
/// Details of invalid request property.
/// </summary>
public record ErrorDetail
{
    /// <summary>
    /// Gets the name of the property causing the error.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    [DisallowNull]
    public string Message { get; }

    /// <summary>
    /// Gets a value indicating whether there are additional properties.
    /// </summary>
    [DefaultValue(false)]
    public bool AdditionalProperties { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorDetail"/> class with the specified property name and message.
    /// </summary>
    /// <param name="propertyName">The name of the property causing the error.</param>
    /// <param name="message">The error message.</param>
    public ErrorDetail(string propertyName, string message)
    {
        PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        Message = message ?? throw new ArgumentNullException(nameof(message));
        AdditionalProperties = false;
    }
}

