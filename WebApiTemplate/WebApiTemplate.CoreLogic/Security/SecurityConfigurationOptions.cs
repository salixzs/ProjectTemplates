using ConfigurationValidation;

namespace WebApiTemplate.CoreLogic.Security;

public class SecurityConfigurationOptions : IValidatableConfiguration
{
    public const string ConfigurationSectionName = "Security";

    public CorsOptions Cors { get; set; } = new CorsOptions();

    public IEnumerable<ConfigurationValidationItem> Validate()
    {
        var validations = new ConfigurationValidationCollector<SecurityConfigurationOptions>(this);

        foreach (var corsUrl in Cors.Origins)
        {
            if (!Uri.IsWellFormedUriString(corsUrl, UriKind.Absolute))
            {
                validations.ValidateAddCustom(c => c.Cors, $"{corsUrl} is not valid abolute URL.");
            }
        }

        return validations.Result;
    }
}

public class CorsOptions
{
    public List<string> Origins { get; set; } = new List<string>();
    public override string ToString() => "dfd";
}
