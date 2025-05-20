using Carter;
using Clean.Solutions.Vertical.Contracts;
using Clean.Solutions.Vertical.Database;
using Clean.Solutions.Vertical.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Clean.Solutions.Vertical.Features.Location;

public static class GetLocationCount
{
    public class Query : IRequest<Result<GetLocationCountResponse>>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Result<GetLocationCountResponse>>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<GetLocationCountResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var GetLocationCountResponse = await _dbContext
                .Todos
                .AsNoTracking()
                .Where(o => o.Id == request.Id)
                .Select(o => new GetLocationCountResponse
                {
                    //Id = o.Id,
                    //Subject = o.Subject,
                    //Content = o.Content,
                    //CreatedOnUtc = o.CreatedOnUtc
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (GetLocationCountResponse is null)
            {
                return Result.Failure<GetLocationCountResponse>(new Error(
                    "GetLocationCount.Null",
                    "The todo with the specified ID was not found"));
            }

            return GetLocationCountResponse;
        }
    }
}

public class GetLocationCountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/getlocationcount", async (bool locationPricing) =>
        {
            //var query = new GetLocationCount.Query { Id = id };

            //var result = await sender.Send(query);

            //if (result.IsFailure)
            //{
            //    return Results.NotFound(result.Error);
            //}

            return Results.Ok();
        })
            .WithName("GetLocationCount")
            .WithOpenApi(o => new Microsoft.OpenApi.Models.OpenApiOperation(o)
            {
                Summary = "Get Location Count",
                Description = "Return Location Count",
                Tags = new List<OpenApiTag>
                {
                    new() { Name="Location" }
                }
            });
    }
}
