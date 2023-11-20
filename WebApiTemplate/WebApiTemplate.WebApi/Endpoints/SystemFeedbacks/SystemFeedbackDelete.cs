using WebApiTemplate.CoreLogic.Handlers.SystemFeedbacks;

namespace WebApiTemplate.WebApi.Endpoints.SystemFeedbacks;

public class SystemFeedbackDelete(ISystemFeedbackCommands commandHandler) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete(Urls.SystemFeedbacks.WithId);
        Tags(Urls.SystemFeedbacks.SwaggerTag);
        AllowAnonymous();
        DontCatchExceptions();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(Urls.SystemFeedbacks.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Deletes the system feedback.";
            swagger.Description = "Deletes the system feedback together with its comments.";
            swagger.Response<int>((int)HttpStatusCode.NoContent, "System feedback is successfully deleted.");
            swagger.Response<ApiError>((int)HttpStatusCode.InternalServerError, "Error occurred in server during feedback delete.");
            swagger.Response<ApiError>((int)HttpStatusCode.UnprocessableEntity, "Id is not provided.");
            swagger.ResponseExamples[(int)HttpStatusCode.UnprocessableEntity] = EndpointHelpers.ExampleApiValidationError();
            swagger.ResponseExamples[(int)HttpStatusCode.InternalServerError] = EndpointHelpers.ExampleApiError();
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var feedbackId = Route<int>("id");
        await commandHandler.Delete(feedbackId, cancellationToken);
        await SendNoContentAsync(cancellationToken);
    }
}
