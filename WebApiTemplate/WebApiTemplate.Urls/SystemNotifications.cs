namespace WebApiTemplate.Urls;

public static class SystemNotifications
{
    public const string SwaggerTag = "SystemNotifications";

    public const string BaseUri = Global.ApiRoot + "/system-notifications";
    public const string GetById = BaseUri + "/{id}";
    public const string All = BaseUri + "/all";
}
