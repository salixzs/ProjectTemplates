using FluentValidation.Results;

namespace WebApiTemplate.WebApi.Endpoints;

/// <summary>
/// Shortcut methods for endpoint configuration.
/// </summary>
internal static class EndpointHelpers
{
    /// <summary>
    /// Throws specific exception, if request validation found problems in expoint request data.
    /// </summary>
    /// <param name="validationFailed">Tru/False if needs to be thrown.</param>
    /// <param name="validationFailures">List of validation errors.</param>
    /// <param name="endpointName">Name of endpoint class. Used in error message.</param>
    /// <exception cref="ValidationFailureException">Throws this if specified with boolean.</exception>
    public static void ThrowIfRequestValidationFailed(bool validationFailed, List<ValidationFailure> validationFailures, string endpointName)
    {
        if (!validationFailed)
        {
            return;
        }

        var validationException = new ValidationFailureException(validationFailures, "Request validation failed");
        validationException.Data.Add("EnpointName", endpointName);
        validationException.Source = endpointName;
        throw validationException;
    }

    public static ApiError ExampleApiError() =>
        new ApiError
        {
            ErrorType = ApiErrorType.ServerError,
            ExceptionType = "ArgumentNullException",
            RequestedUrl = "/api/MyEndpoint",
            Status = 500,
            Title = "This is example error message",
            StackTrace = new List<string>
                {
                    "/Solution/Project/MyClass.cs, MyMethod, Line 19 (Col:8)",
                    "/Solution/Project/MyOtherClass.cs, MyOtherMethod, Line 32 (Col:15)"
                }
        };

    public static ApiError ExampleApiValidationError() =>
        new ApiError
        {
            ErrorType = ApiErrorType.DataValidationError,
            ExceptionType = "ValidationException",
            RequestedUrl = "/api/MyEndpoint",
            Status = 422,
            Title = "There are problems with request data.",
            StackTrace = new List<string>(),
            ValidationErrors = new List<ApiDataValidationError>
            {
                new ApiDataValidationError
                {
                    PropertyName = "FirstName",
                    AttemptedValue = string.Empty,
                    Message = "First name cannot be empty."
                },
                new ApiDataValidationError
                {
                    PropertyName = "BirthDate",
                    AttemptedValue = DateTime.Now.AddYears(1),
                    Message = "Birth date cannot be in future."
                }
            }
        };
}
