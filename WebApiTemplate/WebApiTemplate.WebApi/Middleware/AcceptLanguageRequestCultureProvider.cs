using Microsoft.AspNetCore.Localization;

namespace WebApiTemplate.WebApi.Middleware;

public class AcceptLanguageRequestCultureProvider : IRequestCultureProvider
{
    public async Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var acceptLanguages = httpContext.Request.GetTypedHeaders().AcceptLanguage
            .OrderByDescending(lang => lang.Quality)
            .ToList();
        var firstLanguage = acceptLanguages.FirstOrDefault();
        return new ProviderCultureResult("en", "US");
    }
}
