using System.Diagnostics.CodeAnalysis;
using WebApiTemplate.Crosscut.Exceptions;

namespace WebApiTemplate.CoreLogic.Handlers.Sandbox;

[ExcludeFromCodeCoverage]
public class SandboxHandler : ISandboxHandler
{
    public string ThrowExceptionOnPurpose() =>
        throw new BusinessException(
            "This exception is thrown on purpose",
            new BusinessException(
                "Showcase inner exception",
                BusinessExceptionType.NotImplemented),
            BusinessExceptionType.ServerError,
            22);
}
