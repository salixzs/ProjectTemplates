using System.Diagnostics.CodeAnalysis;
using Microsoft.IdentityModel.Tokens;
using WebApiTemplate.Crosscut.Exceptions;
using WebApiTemplate.Translations;

namespace WebApiTemplate.WebApi.Middleware;

/// <summary>
/// Handle unhandled server exceptions as JSON <see cref="ApiError"/> object, describing it.
/// </summary>
[ExcludeFromCodeCoverage]
public class ApiJsonErrorMiddleware(
    RequestDelegate next,
    ILogger<ApiJsonExceptionMiddleware> logger,
    ApiJsonExceptionOptions options,
    ITranslate<ErrorMessageTranslations> l10n) : ApiJsonExceptionMiddleware(next, logger, options)
{
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
                    apiError.Title = l10n[nameof(ErrorMessageTranslations.SecurityError)];
                    apiError.Status = 401;
                    apiError.ErrorType = ApiErrorType.SecurityError;
                    break;
                case BusinessExceptionType.AccessRestrictedError:
                    apiError.Title = l10n[nameof(ErrorMessageTranslations.SecurityError)];
                    apiError.Status = 403;
                    apiError.ErrorType = ApiErrorType.AccessRestrictedError;
                    break;
                case BusinessExceptionType.ServerError:
                    apiError.Title = l10n[nameof(ErrorMessageTranslations.ServerError)];
                    apiError.ErrorType = ApiErrorType.ServerError;
                    apiError.Status = 500;
                    break;
                case BusinessExceptionType.RequestError:
                    apiError.Title = l10n[nameof(ErrorMessageTranslations.RequestError)];
                    apiError.ErrorType = ApiErrorType.RequestError;
                    apiError.Status = 400;
                    break;
                case BusinessExceptionType.DataValidationError:
                    apiError.Title = l10n[nameof(ErrorMessageTranslations.RequestValidationError)];
                    apiError.ErrorType = ApiErrorType.DataValidationError;
                    apiError.Status = 422;
                    break;
                case BusinessExceptionType.ConfigurationError:
                    apiError.Title = l10n[nameof(ErrorMessageTranslations.ConfigurationError)];
                    apiError.ErrorType = ApiErrorType.ConfigurationError;
                    apiError.Status = 500;
                    break;
                case BusinessExceptionType.ExternalError:
                    apiError.Title = l10n[nameof(ErrorMessageTranslations.ExternalError)];
                    apiError.ErrorType = ApiErrorType.ExternalError;
                    apiError.Status = 500;
                    break;
                case BusinessExceptionType.StorageError:
                    apiError.Title = l10n[nameof(ErrorMessageTranslations.StorageError)];
                    apiError.ErrorType = ApiErrorType.StorageError;
                    apiError.Status = 500;
                    break;
                case BusinessExceptionType.StorageConcurrencyError:
                    apiError.Title = l10n[nameof(ErrorMessageTranslations.ConcurrencyError)];
                    apiError.ErrorType = ApiErrorType.StorageConcurrencyError;
                    apiError.Status = 500;
                    break;
                case BusinessExceptionType.CancelledOperation:
                    apiError.Title = l10n[nameof(ErrorMessageTranslations.CancelledOperation)];
                    apiError.ErrorBehavior = ApiErrorBehavior.Ignore;
                    break;
                default:
                    apiError.Title = l10n[nameof(ErrorMessageTranslations.GeneralError)];
                    apiError.Status = 500;
                    apiError.ErrorType = ApiErrorType.Undetermined;
                    break;
            }
        }

        if (exception is ValidationFailureException validation)
        {
            apiError.ErrorType = ApiErrorType.DataValidationError;
            apiError.Status = 422;
            apiError.Title = l10n[nameof(ErrorMessageTranslations.ValidationError), arguments: validation.Source ?? string.Empty];
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
            apiError.Title = l10n[nameof(ErrorMessageTranslations.SecurityError)];
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
            apiError.Title = l10n[nameof(ErrorMessageTranslations.NotImplementedError)];
            apiError.Status = 501;
        }

        // Also true/works for TaskCanceledException (derived class)
        if (exception is OperationCanceledException)
        {
            apiError.Title = l10n[nameof(ErrorMessageTranslations.CancelledOperation)];
            apiError.ErrorBehavior = ApiErrorBehavior.Ignore;
        }

        return apiError;
    }
}
