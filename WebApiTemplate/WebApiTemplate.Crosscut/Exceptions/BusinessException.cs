namespace WebApiTemplate.Crosscut.Exceptions;

/// <summary>
/// Custom business logic exception with possibility to set type of error.
/// </summary>
public class BusinessException : Exception
{
    /// <summary>
    /// Type of exception.
    /// </summary>
    public BusinessExceptionType ExceptionType { get; set; } = BusinessExceptionType.GeneralError;

    /// <summary>
    /// Error code to be used mainly for distinguish exceptions if they need handling in regard to message translations and customizations. 
    /// </summary>
    public int ErrorCode { get; set; }

    /// <summary>
    /// Custom business logic exception with possibility to set type of error.
    /// </summary>
    public BusinessException()
    {
    }

    /// <summary>
    /// Custom business logic exception with possibility to set type of error.
    /// </summary>
    /// <param name="message">Custom error message.</param>
    public BusinessException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Custom business logic exception with possibility to set type of error.
    /// </summary>
    /// <param name="message">Custom error message.</param>
    /// <param name="errorCode">Unique code for error to be used in client (translations, special handling).</param>
    public BusinessException(string message, int errorCode)
        : base(message) => ErrorCode = errorCode;

    /// <summary>
    /// Custom business logic exception with possibility to set type of error.
    /// </summary>
    /// <param name="message">Custom error message.</param>
    /// <param name="type">Exception type.</param>
    public BusinessException(string message, BusinessExceptionType type)
        : base(message) => ExceptionType = type;

    /// <summary>
    /// Custom business logic exception with possibility to set type of error.
    /// </summary>
    /// <param name="message">Custom error message.</param>
    /// <param name="type">Exception type.</param>
    /// <param name="errorCode">Unique code for error to be used in client (translations, special handling).</param>
    public BusinessException(string message, BusinessExceptionType type, int errorCode)
        : base(message)
    {
        ExceptionType = type;
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Custom business logic exception with possibility to set type of error.
    /// </summary>
    /// <param name="message">Custom error message.</param>
    public BusinessException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Custom business logic exception with possibility to set type of error.
    /// </summary>
    /// <param name="message">Custom error message.</param>
    /// <param name="type">Exception type.</param>
    public BusinessException(string message, Exception innerException, BusinessExceptionType type)
        : base(message, innerException) => ExceptionType = type;

    /// <summary>
    /// Custom business logic exception with possibility to set type of error.
    /// </summary>
    /// <param name="message">Custom error message.</param>
    /// <param name="errorCode">Unique code for error to be used in client (translations, special handling).</param>
    public BusinessException(string message, Exception innerException, int errorCode)
        : base(message, innerException) => ErrorCode = errorCode;

    /// <summary>
    /// Custom business logic exception with possibility to set type of error.
    /// </summary>
    /// <param name="message">Custom error message.</param>
    /// <param name="type">Exception type.</param>
    /// <param name="errorCode">Unique code for error to be used in client (translations, special handling).</param>
    public BusinessException(string message, Exception innerException, BusinessExceptionType type, int errorCode)
        : base(message, innerException)
    {
        ExceptionType = type;
        ErrorCode = errorCode;
    }
}
