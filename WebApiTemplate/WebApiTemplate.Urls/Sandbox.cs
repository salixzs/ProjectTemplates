namespace WebApiTemplate.Urls;

public static class Sandbox
{
    public const string SwaggerTag = "Sandbox";

    public const string BaseUri = Root.ApiRoot + "/sandbox";
    public const string Numbers = BaseUri + "/numbers";
    public const string DateTimes = BaseUri + "/datetimes";
    public const string Strings = BaseUri + "/strings";
    public const string OtherTypes = BaseUri + "/othertypes";
    public const string Exception = BaseUri + "/exception";
    public const string Localization = BaseUri + "/localization";
}
