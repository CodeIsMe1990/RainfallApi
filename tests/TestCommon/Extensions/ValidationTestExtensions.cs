using FluentValidation;
using FluentValidation.TestHelper;

namespace TestCommon.Extensions;

    public static class ValidationTestExtensions
    {
        public static void ShouldNotHaveValidationErrors<T>(this IValidator<T> validator, T model)
        {
            validator.TestValidate(model).ShouldNotHaveAnyValidationErrors();
        }

        public static void ShouldHaveValidationError<T>(this IValidator<T> validator, T model)
        {
            validator.TestValidate(model).ShouldHaveAnyValidationError();
        }

        public static void ShouldNotHaveValidationErrorFor<T>(this IValidator<T> validator, T model, string propertyName)
        {
            validator.TestValidate(model).ShouldNotHaveValidationErrorFor(propertyName);
        }

        public static void ShouldHaveValidationErrorFor<T>(this IValidator<T> validator, T model, string propertyName)
        {
            validator.TestValidate(model).ShouldHaveValidationErrorFor(propertyName);
        }
    }