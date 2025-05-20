using Carter;
using Clean.Solutions.Vertical.Contracts;
using Clean.Solutions.Vertical.Database;
using Clean.Solutions.Vertical.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Clean.Solutions.Vertical.Features.Location;

public static class GetMapDetails
{
    public class Query : IRequest<Result<GetMapDetailsResponse>>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Result<GetMapDetailsResponse>>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<GetMapDetailsResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var GetMapDetailsResponse = await _dbContext
                .Todos
                .AsNoTracking()
                .Where(o => o.Id == request.Id)
                .Select(o => new GetMapDetailsResponse
                {
                    //Id = o.Id,
                    //Subject = o.Subject,
                    //Content = o.Content,
                    //CreatedOnUtc = o.CreatedOnUtc
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (GetMapDetailsResponse is null)
            {
                return Result.Failure<GetMapDetailsResponse>(new Error(
                    "GetMapDetails.Null",
                    "The todo with the specified ID was not found"));
            }

            return GetMapDetailsResponse;
        }
    }
}

public class GetMapDetailsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/getmapdetails", async (string? path, string? pl, string? t, string? o) =>
        {
            //var query = new GetMapDetails.Query { Id = id };

            //var result = await sender.Send(query);

            //if (result.IsFailure)
            //{
            //    return Results.NotFound(result.Error);
            //}

            return Results.Ok();
        })
            .WithName("GetMapDetails")
            .WithOpenApi(o => new Microsoft.OpenApi.Models.OpenApiOperation(o)
            {
                Summary = "Get Map Details",
                Description = "Return Map Details",
                Tags = new List<OpenApiTag>
                {
                    new() { Name="Location" }
                }
            });
    }
}
