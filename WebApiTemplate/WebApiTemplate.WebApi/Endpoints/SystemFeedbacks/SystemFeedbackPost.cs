using WebApiTemplate.CoreLogic.Handlers.SystemFeedbacks;
using WebApiTemplate.Domain.SystemFeedbacks;
using WebApiTemplate.WebApi.Endpoints.SystemNotifications;

namespace WebApiTemplate.WebApi.Endpoints.SystemFeedbacks;

public class SystemFeedbackPost : Endpoint<SystemFeedback>
{
    private readonly ISystemFeedbackCommands _commandHandler;

    public SystemFeedbackPost(ISystemFeedbackCommands commandHandler) => _commandHandler = commandHandler;

    public override void Configure()
    {
        Post(Urls.SystemFeedbacks.BaseUri);
        Tags(Urls.SystemFeedbacks.SwaggerTag);
        Validator<SystemFeedbackValidator>();
        AllowAnonymous();

        // This disables FastEndpoints built in error handlers (We have our own).
        DontCatchExceptions();
        DontThrowIfValidationFails();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(Urls.SystemFeedbacks.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Create a new end user feedback (bug, request).";
            swagger.Description = "Creates a data for new system feedback by application end-user to development and support team(s).";
            swagger.Response<int>((int)HttpStatusCode.Created, "System feedback is successfully created.");
            swagger.Response<ApiError>((int)HttpStatusCode.InternalServerError, "Error occurred in server during feedback create.");
            swagger.Response<ApiError>((int)HttpStatusCode.UnprocessableEntity, "Request (submitted feedback data) has validation errors.");
            swagger.ResponseExamples[(int)HttpStatusCode.Created] = 1025;
            swagger.ResponseExamples[(int)HttpStatusCode.UnprocessableEntity] = EndpointHelpers.ExampleApiValidationError();
            swagger.ResponseExamples[(int)HttpStatusCode.InternalServerError] = EndpointHelpers.ExampleApiError();
            swagger.ExampleRequest = new SystemFeedback
            {
                Id = 0,
                Title = "Sample end-user feedback",
                Content = "There is some kind of problem to be solved.",
                Category = Enumerations.SystemFeedbackCategory.SupportRequest,
                Status = Enumerations.SystemFeedbackStatus.New,
                Priority = Enumerations.SystemFeedbackPriority.Normal,
                SystemInfo = "Here goes some tech info, like stack trace, exception data etc.",
            };
        });
    }

    public override async Task HandleAsync(SystemFeedback feedback, CancellationToken cancellationToken)
    {
        EndpointHelpers.ThrowIfRequestValidationFailed(ValidationFailed, ValidationFailures, GetType().Name);
        var newId = await _commandHandler.Create(feedback, cancellationToken);
        await SendCreatedAtAsync<SingleSystemNotificationGet>(routeValues: new { id = newId }, responseBody: newId, cancellation: cancellationToken);
    }
}
