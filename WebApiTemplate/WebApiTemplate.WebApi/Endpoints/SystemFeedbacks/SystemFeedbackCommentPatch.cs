using WebApiTemplate.CoreLogic.Handlers.SystemFeedbacks;
using WebApiTemplate.Domain.SystemFeedbacks;

namespace WebApiTemplate.WebApi.Endpoints.SystemFeedbacks;

public class SystemFeedbackCommentPatch(ISystemFeedbackCommentCommands commandHandler) : Endpoint<SystemFeedbackComment>
{
    public override void Configure()
    {
        Patch(Urls.SystemFeedbacks.Comment);
        Tags(Urls.SystemFeedbacks.SwaggerTag);
        Validator<SystemFeedbackCommentPatchValidator>();
        AllowAnonymous();

        // This disables FastEndpoints built in error handlers (We have our own).
        DontCatchExceptions();
        DontThrowIfValidationFails();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(Urls.SystemFeedbacks.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Updates an existing comment for end-user feedback (bug, request).";
            swagger.Description = "Updates a content of an existing system feedback comment.";
            swagger.Response<int>((int)HttpStatusCode.OK, "System feedback comment is successfully updated.");
            swagger.Response<ApiError>((int)HttpStatusCode.InternalServerError, "Error occurred in server during feedback comment create.");
            swagger.Response<ApiError>((int)HttpStatusCode.UnprocessableEntity, "Request (submitted comment) has validation errors.");
            swagger.ResponseExamples[(int)HttpStatusCode.UnprocessableEntity] = EndpointHelpers.ExampleApiValidationError();
            swagger.ResponseExamples[(int)HttpStatusCode.InternalServerError] = EndpointHelpers.ExampleApiError();
            swagger.ExampleRequest = new SystemFeedbackComment
            {
                Id = 1033,
                Content = "This comment needs an update."
            };
        });
    }

    public override async Task HandleAsync(SystemFeedbackComment feedbackComment, CancellationToken cancellationToken)
    {
        EndpointHelpers.ThrowIfRequestValidationFailed(ValidationFailed, ValidationFailures, GetType().Name);
        await commandHandler.Update(feedbackComment, cancellationToken);
        await SendOkAsync(cancellation: cancellationToken);
    }
}
