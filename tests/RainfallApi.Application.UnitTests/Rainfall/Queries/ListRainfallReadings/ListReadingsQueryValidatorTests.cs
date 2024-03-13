using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RainfallApi.Application.Rainfall.Queries.ListRainfallReadings;

using TestCommon.Extensions;

namespace RainfallApi.Application.UnitTests.RainfallReadings.Queries.ListReadings;

public class ListReadingsQueryValidatorTests
{
    protected ListRainfallReadingsQueryValidator _validator;
    protected CancellationTokenSource _cts;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListReadingsQueryValidatorTests"/> class.
    /// </summary>
    public ListReadingsQueryValidatorTests()
    {
        _validator = new ListRainfallReadingsQueryValidator();
        _cts = new CancellationTokenSource();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    public void GivenAValidTemperatureCValue_ShouldNotHaveValidationError(int count)
        => _validator.ShouldNotHaveValidationErrors(new ListRainfallReadingsQuery("Station1", count));

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void GivenAValidTemperatureCValue_ShouldHaveValidationError(int count)
        => _validator.ShouldHaveValidationError(new ListRainfallReadingsQuery("Station1", count));

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    public void GivenAValidTemperatureCValue_ShouldNotHaveValidationErrorForPropertyCount(int count)
    {
        var propertyToValidate = "Count";
        _validator.ShouldNotHaveValidationErrorFor(new ListRainfallReadingsQuery("Station1", count), propertyToValidate);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void GivenAValidTemperatureCValue_ShouldHaveValidationErrorForPropertyCount(int count)
    {
        var propertyToValidate = "Count";
        _validator.ShouldHaveValidationErrorFor(new ListRainfallReadingsQuery("Station1", count), propertyToValidate);
    }
}