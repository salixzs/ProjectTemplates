using System.Diagnostics.CodeAnalysis;
using Microsoft.IdentityModel.Tokens;
using WebApiTemplate.Crosscut.Exceptions;

namespace WebApiTemplate.WebApi.Middleware;

/// <summary>
/// Handle unhandled server exceptions as JSON <see cref="ApiError"/> object, describing it.
/// </summary>
[ExcludeFromCodeCoverage]
public class ApiJsonErrorMiddleware : ApiJsonExceptionMiddleware
{
    /// <summary>
    /// Handle unhandled server exceptions as JSON <see cref="ApiError"/> object, describing it.
    /// </summary>
    public ApiJsonErrorMiddleware(RequestDelegate next, ILogger<ApiJsonExceptionMiddleware> logger, ApiJsonExceptionOptions options)
        : base(next, logger, options)
    {
    }

    /// <summary>
    /// Override necessary <see cref="ApiError"/> properties based on thrown exception types.
    /// </summary>
    /// <param name="apiError">Api Error object to modify.</param>
    /// <param name="exception">Thrown exception.</param>
    protected override ApiError HandleSpecialException(ApiError apiError, Exception exception)
    {
        if (exception is BusinessException apiException)
        {
            switch (apiException.ExceptionType)
            {
                case BusinessExceptionType.SecurityError:
                    apiError.Status = 401;
                    apiError.ErrorType = ApiErrorType.SecurityError;
                    break;
                case BusinessExceptionType.AccessRestrictedError:
                    apiError.Status = 403;
                    apiError.ErrorType = ApiErrorType.AccessRestrictedError;
                    break;
                case BusinessExceptionType.ServerError:
                    apiError.ErrorType = ApiErrorType.ServerError;
                    apiError.Status = 500;
                    break;
                case BusinessExceptionType.RequestError:
                    apiError.ErrorType = ApiErrorType.RequestError;
                    apiError.Status = 400;
                    break;
                case BusinessExceptionType.DataValidationError:
                    apiError.ErrorType = ApiErrorType.DataValidationError;
                    apiError.Status = 422;
                    break;
                case BusinessExceptionType.ConfigurationError:
                    apiError.ErrorType = ApiErrorType.ConfigurationError;
                    apiError.Status = 500;
                    break;
                case BusinessExceptionType.ExternalError:
                    apiError.ErrorType = ApiErrorType.ExternalError;
                    apiError.Status = 500;
                    break;
                case BusinessExceptionType.StorageError:
                    apiError.ErrorType = ApiErrorType.StorageError;
                    apiError.Status = 500;
                    break;
                case BusinessExceptionType.StorageConcurrencyError:
                    apiError.ErrorType = ApiErrorType.StorageConcurrencyError;
                    apiError.Status = 500;
                    break;
                case BusinessExceptionType.CancelledOperation:
                    apiError.ErrorBehavior = ApiErrorBehavior.Ignore;
                    break;
                default:
                    apiError.Status = 500;
                    apiError.ErrorType = ApiErrorType.Undetermined;
                    break;
            }
        }

        if (exception is ValidationFailureException validation)
        {
            apiError.ErrorType = ApiErrorType.DataValidationError;
            apiError.Status = 422;
            apiError.Title = $"Request data is not valid for {validation.Source ?? string.Empty} endpoint";
            if (validation.Failures != null)
            {
                foreach (var validationError in validation.Failures)
                {
                    apiError.ValidationErrors.Add(
                        new ApiDataValidationError
                        {
                            PropertyName = validationError.PropertyName,
                            AttemptedValue = validationError.AttemptedValue,
                            Message = validationError.ErrorMessage,
                        });
                }
            }
        }

        if (exception is AccessViolationException)
        {
            apiError.Status = 401; // or 403
            apiError.ErrorType = ApiErrorType.AccessRestrictedError;
        }

        // All possible token exceptions derives from this - validation, expiration, signature.
        // and all these should return 401 (authentication/security problems)
        if (exception is SecurityTokenException)
        {
            apiError.Status = 401;
            if (exception is SecurityTokenExpiredException)
            {
                // Expired tokens should not write to log, its normal situation,
                // but still respond with 401 error to client.
                apiError.ErrorBehavior = ApiErrorBehavior.RespondWithError;
            }
        }

        if (exception is NotImplementedException)
        {
            apiError.Status = 501;
            apiError.Title = "Functionality is not yet implemented.";
        }

        // Also true/works for TaskCanceledException (derived class)
        if (exception is OperationCanceledException)
        {
            apiError.ErrorBehavior = ApiErrorBehavior.Ignore;
        }

        return apiError;
    }
}
