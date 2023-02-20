namespace WebApiTemplate.Urls;

public static class SystemNotifications
{
    public const string SwaggerTag = "SystemNotifications";

    public const string BaseUri = Root.ApiRoot + "/system-notifications";
    public const string WithId = BaseUri + "/{id}";
    public const string All = BaseUri + "/all";
}
