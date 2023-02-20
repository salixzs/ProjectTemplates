global using System.Net;
global using FastEndpoints;
global using Salix.AspNetCore.JsonExceptionHandler;
using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Naming",
    "CA1725:Parameter names should match base declaration",
    Justification = "FastEndpoints shortened naming override")]

[assembly: SuppressMessage(
    "Roslynator",
    "RCS1168:Parameter name differs from base name.",
    Justification = "Shortened names in FastEndpoints base classes.")]
