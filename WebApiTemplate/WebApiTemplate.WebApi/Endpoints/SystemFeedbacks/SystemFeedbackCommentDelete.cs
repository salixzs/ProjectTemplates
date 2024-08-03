using WebApiTemplate.CoreLogic.Handlers.SystemFeedbacks;

namespace WebApiTemplate.WebApi.Endpoints.SystemFeedbacks;

public class SystemFeedbackCommentDelete(ISystemFeedbackCommentCommands commandHandler) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete(Urls.SystemFeedbacks.CommentWithId);
        Tags(Urls.SystemFeedbacks.SwaggerTag);
        AllowAnonymous();
        DontCatchExceptions();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(Urls.SystemFeedbacks.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Deletes the system feedback comment.";
            swagger.Description = "Deletes the specific comment on end-user system feedback.";
            swagger.Response<int>((int)HttpStatusCode.NoContent, "System feedback comment is successfully deleted.");
            swagger.Response<ApiError>((int)HttpStatusCode.InternalServerError, "Error occurred in server during system feedback comment delete.");
            swagger.Response<ApiError>((int)HttpStatusCode.UnprocessableEntity, "Id is not provided.");
            swagger.ResponseExamples[(int)HttpStatusCode.UnprocessableEntity] = EndpointHelpers.ExampleApiValidationError();
            swagger.ResponseExamples[(int)HttpStatusCode.InternalServerError] = EndpointHelpers.ExampleApiError();
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var commentId = Route<int>("commentId");
        await commandHandler.Delete(commentId, cancellationToken);
        await SendNoContentAsync(cancellationToken);
    }
}
