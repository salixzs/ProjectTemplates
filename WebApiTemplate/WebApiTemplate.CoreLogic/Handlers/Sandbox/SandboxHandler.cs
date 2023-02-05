using System.Diagnostics.CodeAnalysis;
using WebApiTemplate.Crosscut.Exceptions;

namespace WebApiTemplate.CoreLogic.Handlers.Sandbox;

[ExcludeFromCodeCoverage]
public class SandboxHandler : ISandboxHandler
{
    public string ThrowExceptionOnPurpose()
    {
        throw new BusinessException("This exception is thrown on puspose", new BusinessException("Inner exception showcase", BusinessExceptionType.NotImplemented), BusinessExceptionType.ServerError, 22);
    }
}
