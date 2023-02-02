using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.Logging;
using Xunit.Sdk;

namespace WebApiTemplate.CoreLogic.Tests;

/// <summary>
/// Microsoft ILogger implementation, which can be used in XUnit tests as stub for real logger.
/// Writes messages to Output stream and also stores in internal property <seealso cref="LogStatements"/> so they can be asserted.
/// </summary>
/// <typeparam name="T">Logger class type (Typed logger)</typeparam>
/// <seealso cref="ILogger{T}" />
[ExcludeFromCodeCoverage]
public sealed class XUnitLogger<T> : ILogger<T>, IDisposable
{
    private readonly IMessageSink? _messageSink;
    private ITestOutputHelper? _outputHelper;

    /// <summary>
    /// Set to TRUE to pause emitting and storing logging statements (default: FALSE)
    /// </summary>
    public bool LoggingDisabled { get; set; }

    /// <summary>
    /// Logged message store.
    /// </summary>
    public List<LoggingStatement> LogStatements { get; } = new();

    /// <summary>
    /// Microsoft ILogger implementation, which can be used in XUnit tests as stub for real logger.
    /// Writes messages to Output stream and also stores in internal property <seealso cref="LogStatements" /> so they can be asserted.
    /// </summary>
    /// <param name="output">Accepting MessageSink as output from Fixtures.</param>
    public XUnitLogger(IMessageSink output) => _messageSink = output;

    /// <summary>
    /// Microsoft ILogger implementation, which can be used in XUnit tests as stub for real logger.
    /// Writes messages to Output stream and also stores in internal property <seealso cref="LogStatements" /> so they can be asserted.
    /// </summary>
    /// <param name="output">Accepting MessageSink as output from Fixtures.</param>
    public XUnitLogger(ITestOutputHelper output) => _outputHelper = output;

    public XUnitLogger<T> SetOutputHelper(ITestOutputHelper output)
    {
        if (output != null)
        {
            _outputHelper = output;
        }

        return this;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (LoggingDisabled || state == null)
        {
            return;
        }

        LogStatements.Add(new LoggingStatement
        {
            Level = logLevel,
            EventId = eventId.Id,
            EventName = eventId.Name,
            Message = formatter.Invoke(state, exception),
            Exception = exception
        });

        if (_outputHelper != null)
        {
            // Need to wrap as when called from dispose method it throws "There is no currently active test" InvalidOperationException.
            try
            {
                _outputHelper.WriteLine(state.ToString());
                return;
            }
            catch
            {
                // Happens in Dispose() - should be handled by message sink below or (huh) swallowed into void
            }
        }

        _messageSink?.OnMessage(new DiagnosticMessage(state.ToString()));
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => this;

    public void Dispose()
    {
    }
}

/// <summary>
/// Microsoft ILogger implementation, which can be used in XUnit tests as stub for real logger.
/// Writes messages to Output stream and also stores in internal property <seealso cref="LogStatements"/> so they can be asserted.
/// </summary>
/// <seealso cref="ILogger" />
[ExcludeFromCodeCoverage]
public sealed class XUnitLogger : ILogger, IDisposable
{
    private readonly IMessageSink? _messageSink;
    private ITestOutputHelper? _outputHelper;

    /// <summary>
    /// Set to TRUE to pause emitting and storing logging statements (default: FALSE)
    /// </summary>
    public bool LoggingDisabled { get; set; }

    /// <summary>
    /// Logged message store.
    /// </summary>
    public List<LoggingStatement> LogStatements { get; } = new();

    /// <summary>
    /// Microsoft ILogger implementation, which can be used in XUnit tests as stub for real logger.
    /// Writes messages to Output stream and also stores in internal property <seealso cref="LogStatements" /> so they can be asserted.
    /// </summary>
    /// <param name="output">Accepting MessageSink as output from Fixtures.</param>
    public XUnitLogger(IMessageSink output) => _messageSink = output;

    /// <summary>
    /// Microsoft ILogger implementation, which can be used in XUnit tests as stub for real logger.
    /// Writes messages to Output stream and also stores in internal property <seealso cref="LogStatements" /> so they can be asserted.
    /// </summary>
    /// <param name="output">Accepting MessageSink as output from Fixtures.</param>
    public XUnitLogger(ITestOutputHelper output) => _outputHelper = output;

    /// <summary>
    /// Sets the output helper for test logging separately.
    /// </summary>
    /// <param name="output">The output helper from XUnit engine.</param>
    public XUnitLogger SetOutputHelper(ITestOutputHelper output)
    {
        if (output != null)
        {
            _outputHelper = output;
        }

        return this;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (LoggingDisabled || state == null)
        {
            return;
        }

        LogStatements.Add(new LoggingStatement
        {
            Level = logLevel,
            EventId = eventId.Id,
            EventName = eventId.Name,
            Message = formatter.Invoke(state, exception),
            Exception = exception
        });

        if (_outputHelper != null)
        {
            // Need to wrap as when called from dispose method it throws "There is no currently active test" InvalidOperationException.
            try
            {
                _outputHelper.WriteLine(state.ToString());
            }
            catch
            {
                // Happens in Dispose() - should be handled by message sink below or (huh) swallowed into void
            }

            return;
        }

        _messageSink?.OnMessage(new DiagnosticMessage(state.ToString()));
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => this;

    public void Dispose()
    {
    }
}

// <summary>
/// Data contract for Logging statement, used in XUnitLogger.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
[ExcludeFromCodeCoverage]
public class LoggingStatement
{
    public string Message { get; set; } = null!;

    public LogLevel Level { get; set; }

    public Exception? Exception { get; set; }

    public int EventId { get; set; }

    public string? EventName { get; set; }

    public override string ToString()
    {
        var dbgView = new StringBuilder();
        if (Level != LogLevel.None)
        {
            dbgView.Append(CultureInfo.InvariantCulture,
                $"[{Level.ToString().ToUpper(CultureInfo.InvariantCulture)}] ");
        }

        dbgView.Append(CultureInfo.InvariantCulture, $";ID: {EventId} ");

        dbgView.Append(Message);
        if (Exception != null)
        {
            dbgView.Append(CultureInfo.InvariantCulture, $"; EXC: {Exception.Message}");
        }

        return dbgView.ToString();
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => ToString();
}
