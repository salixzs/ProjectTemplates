using WebApiTemplate.CoreLogic.Handlers.SystemFeedbacks;
using WebApiTemplate.Domain.SystemFeedbacks;
using WebApiTemplate.WebApi.Endpoints.SystemNotifications;

namespace WebApiTemplate.WebApi.Endpoints.SystemFeedbacks;

public class SystemFeedbackCommentPost : Endpoint<SystemFeedbackComment>
{
    private readonly ISystemFeedbackCommentCommands _commandHandler;

    public SystemFeedbackCommentPost(ISystemFeedbackCommentCommands commandHandler) => _commandHandler = commandHandler;

    public override void Configure()
    {
        Post(Urls.SystemFeedbacks.Comment);
        Tags(Urls.SystemFeedbacks.SwaggerTag);
        Validator<SystemFeedbackCommentPostValidator>();
        AllowAnonymous();

        // This disables FastEndpoints built in error handlers (We have our own).
        DontCatchExceptions();
        DontThrowIfValidationFails();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(Urls.SystemFeedbacks.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Create a new comment for end-user feedback (bug, request).";
            swagger.Description = "Creates a data for new system feedback by application end-user to development and support team(s).";
            swagger.Response<int>((int)HttpStatusCode.Created, "System feedback comment is successfully created.");
            swagger.Response<ApiError>((int)HttpStatusCode.InternalServerError, "Error occurred in server during feedback comment create.");
            swagger.Response<ApiError>((int)HttpStatusCode.UnprocessableEntity, "Request (submitted comment) has validation errors.");
            swagger.ResponseExamples[(int)HttpStatusCode.Created] = 1033;
            swagger.ResponseExamples[(int)HttpStatusCode.UnprocessableEntity] = EndpointHelpers.ExampleApiValidationError();
            swagger.ResponseExamples[(int)HttpStatusCode.InternalServerError] = EndpointHelpers.ExampleApiError();
            swagger.ExampleRequest = new SystemFeedbackComment
            {
                Id = 0,
                Content = "This is a comment to feedback.",
                SystemFeedbackId = 1025
            };
        });
    }

    public override async Task HandleAsync(SystemFeedbackComment feedbackComment, CancellationToken cancellationToken)
    {
        EndpointHelpers.ThrowIfRequestValidationFailed(ValidationFailed, ValidationFailures, GetType().Name);
        var newId = await _commandHandler.Create(feedbackComment, cancellationToken);
        await SendCreatedAtAsync<SingleSystemNotificationGet>(
            routeValues: new { id = feedbackComment.SystemFeedbackId },
            responseBody: newId,
            cancellation: cancellationToken);
    }
}
