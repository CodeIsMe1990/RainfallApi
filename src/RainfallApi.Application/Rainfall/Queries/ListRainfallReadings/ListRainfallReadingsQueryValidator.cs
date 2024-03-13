using FluentValidation;

namespace RainfallApi.Application.Rainfall.Queries.ListRainfallReadings;

/// <summary>
/// Validator for the ListRainfallReadingsQuery.
/// </summary>
public class ListRainfallReadingsQueryValidator : AbstractValidator<ListRainfallReadingsQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ListRainfallReadingsQueryValidator"/> class.
    /// </summary>
    public ListRainfallReadingsQueryValidator()
    {
        // Specifies the validation rule for the 'Count' property of the ListRainfallReadingsQuery class.
        // It ensures that the 'Count' value is greater than or equal to 1 and less than or equal to 100,
        // which is the valid range for the number of rainfall readings to be queried.
        RuleFor(x => x.Count)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(100);
    }
}