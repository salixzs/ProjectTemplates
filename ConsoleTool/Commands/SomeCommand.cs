using Microsoft.Extensions.Options;
using Salix.Extensions;

namespace ConsoleTool.Commands;

public class SomeCommand : IConsoleOperation
{
    private readonly IOptions<SomeCommandConfiguration> _config;

    public SomeCommand(IOptions<SomeCommandConfiguration> config) => _config = config;

    /// <summary>
    /// Short name of the command. Relevant if have multiple!
    /// </summary>
    public string OperationName => "first";

    /// <summary>
    /// Short description (help text of the command).
    /// </summary>
    public string HelpText => "Example command/operation boilerplate.";

    /// <summary>
    /// Should check whether all input parameters are set for the command.
    /// </summary>
    public bool IsReady => (Count ?? 0) > 0;

    [ConsoleOption(
        "count",
        "Dummy parameter to get some count",
        "c")]
    public int? Count { get; set; }

    /// <summary>
    /// Actual business operation routines goes here.
    /// </summary>
    public async Task<int> DoWork()
    {
        Consolix.WriteLine("Run with --h to see help.", ConsoleColor.Cyan);
        Consolix.WriteLine("Count parameter: {0}", ConsoleColor.Gray, ConsoleColor.Cyan, Count ?? 0);
        Consolix.WriteLine("  Config value1: {0}", ConsoleColor.Gray, ConsoleColor.Cyan, _config.Value.Injected1 ?? "Not loaded!");
        Consolix.WriteLine("  Config value2: {0}", ConsoleColor.Gray, ConsoleColor.Cyan, _config.Value.Injected2 ?? "Not loaded!");
        return 0;
    }
}
