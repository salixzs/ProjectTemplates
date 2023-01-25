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
        if (validationFailed)
        {
            var validationException = new ValidationFailureException(validationFailures, "Request validation failed");
            validationException.Data.Add("EnpointName", endpointName);
            validationException.Source = endpointName;
            throw validationException;
        }
    }
}
