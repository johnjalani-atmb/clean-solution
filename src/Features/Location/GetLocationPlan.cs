using Carter;
using Clean.Solutions.Vertical.Contracts;
using Clean.Solutions.Vertical.Database;
using Clean.Solutions.Vertical.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Clean.Solutions.Vertical.Features.Location;

public static class GetLocationPlan
{
    public class Query : IRequest<Result<GetLocationPlanResponse>>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Result<GetLocationPlanResponse>>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<GetLocationPlanResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var GetLocationPlanResponse = await _dbContext
                .Todos
                .AsNoTracking()
                .Where(o => o.Id == request.Id)
                .Select(o => new GetLocationPlanResponse
                {
                    //Id = o.Id,
                    //Subject = o.Subject,
                    //Content = o.Content,
                    //CreatedOnUtc = o.CreatedOnUtc
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (GetLocationPlanResponse is null)
            {
                return Result.Failure<GetLocationPlanResponse>(new Error(
                    "GetLocationPlan.Null",
                    "The todo with the specified ID was not found"));
            }

            return GetLocationPlanResponse;
        }
    }
}

public class GetLocationPlanEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/getlocationplan", async () =>
        {
            //var query = new GetLocationPlan.Query { Id = id };

            //var result = await sender.Send(query);

            //if (result.IsFailure)
            //{
            //    return Results.NotFound(result.Error);
            //}

            return Results.Ok();
        })
            .WithName("GetLocationPlan")
            .WithOpenApi(o => new Microsoft.OpenApi.Models.OpenApiOperation(o)
            {
                Summary = "Get Location Plan",
                Description = "Return Location Plan",
                Tags = new List<OpenApiTag>
                {
                    new() { Name="Location" }
                }
            });
    }
}
