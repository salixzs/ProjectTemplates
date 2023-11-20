using WebApiTemplate.CoreLogic.Handlers.SystemFeedbacks;
using WebApiTemplate.Domain.SystemFeedbacks;

namespace WebApiTemplate.WebApi.Endpoints.SystemFeedbacks;

public class SystemFeedbackPatch(ISystemFeedbackCommands commandHandler) : Endpoint<SystemFeedback>
{
    public override void Configure()
    {
        Patch(Urls.SystemFeedbacks.BaseUri);
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
            swagger.Summary = "Updates a system feedback.";
            swagger.Description = @"Updates a data for of an existing system feedback with new data (properties).
Comments and data relating it is left unchanged.";
            swagger.Response<int>((int)HttpStatusCode.Created, "System feedback is successfully updated.");
            swagger.Response<ApiError>((int)HttpStatusCode.InternalServerError, "Error occurred in server during feedback update.");
            swagger.Response<ApiError>((int)HttpStatusCode.UnprocessableEntity, "Request (submitted feedback data) has validation errors.");
            swagger.ResponseExamples[(int)HttpStatusCode.UnprocessableEntity] = EndpointHelpers.ExampleApiValidationError();
            swagger.ResponseExamples[(int)HttpStatusCode.InternalServerError] = EndpointHelpers.ExampleApiError();
            swagger.ExampleRequest = new SystemFeedback
            {
                Id = 0,
                Title = "Updated Title",
                Content = "Updated content",
                Category = Enumerations.SystemFeedbackCategory.Proposal,
                Priority = Enumerations.SystemFeedbackPriority.Critical,
                Status = Enumerations.SystemFeedbackStatus.InProcess,
                SystemInfo = "Updated system info"
            };
        });
    }

    public override async Task HandleAsync(SystemFeedback feedback, CancellationToken cancellationToken)
    {
        if (feedback.Id == 0)
        {
            AddError("Request should have an ID of existing system feedback.");
        }

        EndpointHelpers.ThrowIfRequestValidationFailed(ValidationFailed, ValidationFailures, GetType().Name);
        await commandHandler.Update(feedback, cancellationToken);
        await SendOkAsync(cancellation: cancellationToken);
    }
}
