using Microsoft.AspNetCore.Mvc;
using WebApiTemplate.CoreLogic.Handlers.SystemFeedbacks;
using WebApiTemplate.Domain.Fakes;
using WebApiTemplate.Domain.SystemFeedbacks;

namespace WebApiTemplate.WebApi.Endpoints.SystemFeedbacks;

public class SystemFeedbacksGet(ISystemFeedbackQueries queryHandler) : Endpoint<SystemFeedbackFilter, List<SystemFeedback>>
{
    public override void Configure()
    {
        Get(Urls.SystemFeedbacks.BaseUri);
        Tags(Urls.SystemFeedbacks.SwaggerTag);
        AllowAnonymous();
        //DontCatchExceptions();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(Urls.SystemFeedbacks.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Returns end-user feedbacks (all or filtered via query parameters).";
            swagger.Description =
                "Returns end-user created feedbacks (bugs, suggestions, requests) in full contract. If filter is empty - returns all feedbacks.";
            swagger.Response<List<SystemFeedback>>(
                (int)HttpStatusCode.OK,
                "Returns existing - all or filtered feedbacks(s) or empty list if no feedbacks exist.");
            swagger.Response<ApiError>((int)HttpStatusCode.InternalServerError, "Error occurred in server during data retrieval.");
            swagger.ResponseExamples[(int)HttpStatusCode.InternalServerError] = EndpointHelpers.ExampleApiError();
            var example = new DomainFakesFactory().GetTestObject<SystemFeedback>();
            swagger.ResponseExamples[(int)HttpStatusCode.OK] = example;
        });
    }

    public override async Task HandleAsync([FromQuery] SystemFeedbackFilter filter, CancellationToken cancellationToken)
    {
        var allNotifications = await queryHandler.GetFeedbacks(filter, cancellationToken);
        await SendOkAsync(allNotifications, cancellationToken);
    }
}
