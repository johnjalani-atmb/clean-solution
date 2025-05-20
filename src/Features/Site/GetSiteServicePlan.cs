using Carter;
using Clean.Solutions.Vertical.Contracts;
using Clean.Solutions.Vertical.Database;
using Clean.Solutions.Vertical.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Clean.Solutions.Vertical.Features.Site;

public static class GetSiteServicePlan
{
    public class Query : IRequest<Result<GetSiteServicePlanResponse>>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Result<GetSiteServicePlanResponse>>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<GetSiteServicePlanResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var GetSiteServicePlanResponse = await _dbContext
                .Todos
                .AsNoTracking()
                .Where(o => o.Id == request.Id)
                .Select(o => new GetSiteServicePlanResponse
                {
                    //Id = o.Id,
                    //Subject = o.Subject,
                    //Content = o.Content,
                    //CreatedOnUtc = o.CreatedOnUtc
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (GetSiteServicePlanResponse is null)
            {
                return Result.Failure<GetSiteServicePlanResponse>(new Error(
                    "GetSiteServicePlan.Null",
                    "The todo with the specified ID was not found"));
            }

            return GetSiteServicePlanResponse;
        }
    }
}

public class GetSiteServicePlanEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/getsiteserviceplan", async (string? path, ISender sender) =>
        {
            //var query = new GetSiteServicePlan.Query { Id = id };

            //var result = await sender.Send(query);

            //if (result.IsFailure)
            //{
            //    return Results.NotFound(result.Error);
            //}

            return Results.Ok();
        })
            .WithName("GetSiteServicePlan")
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Get Site Service Plan",
                Description = "Return GetSiteServicePlan",
                Tags = new List<OpenApiTag>
                {
                    new() { Name="Site" }
                }
            });
    }
}
