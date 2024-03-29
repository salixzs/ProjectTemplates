namespace WebApiTemplate.Urls;

public static class SystemFeedbacks
{
    public const string SwaggerTag = "SystemFeedbacks";

    public const string BaseUri = Root.ApiRoot + "/system-feedbacks";
    public const string WithId = BaseUri + "/{id}";
    public const string Comment = WithId + "/comments";
    public const string CommentWithId = Comment + "/{commentId}";
}
