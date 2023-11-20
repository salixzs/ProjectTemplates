# Solution template for some Console Tool.

Provides boilerplate initial code for Dependency Injection-able, Logger and Configuration enabled console tool.

## Used Technologies and packages
- Net 8.0, Hosted approach.
- Consolix (beautifying, parameter wiring)
- Serilog (logging to file)


# How to use

Mainly only File(s) in Commands folder needs to be developed according to your business operation needs.

`Program.cs` only register these commands classes with dependency injection, so console tool can load and execute them.

Modify help texts and other strings to match your business things.

There could be multiple commands handled by tool. If there are more than one - 
tool will need a name of your command as the first parameter to console tool invocation, like

```
ConsoleTool.exe first
```

followed by any other parameters required for this command prepended with double dashes `--`

## Configuration

Template has added usage of `settings.json` file to add possibility to add console configuration values (DB connections, API addresses, etc.)
If you do not use configuration, remove marked section in `Program.cs` and `settings.json` file.

## Logging

Template has added Serilog based logging to text file.
If you do not use logging, remove marked section in `Program.cs` and `Serilog` NuGet package.

