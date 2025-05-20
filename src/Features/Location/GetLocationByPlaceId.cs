using Carter;
using Clean.Solutions.Vertical.Contracts;
using Clean.Solutions.Vertical.Database;
using Clean.Solutions.Vertical.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Clean.Solutions.Vertical.Features.Location;

public static class GetLocationByPlaceId
{
    public class Query : IRequest<Result<GetLocationByPlaceIdResponse>>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Result<GetLocationByPlaceIdResponse>>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<GetLocationByPlaceIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var GetLocationByPlaceIdResponse = await _dbContext
                .Todos
                .AsNoTracking()
                .Where(o => o.Id == request.Id)
                .Select(o => new GetLocationByPlaceIdResponse
                {
                    //Id = o.Id,
                    //Subject = o.Subject,
                    //Content = o.Content,
                    //CreatedOnUtc = o.CreatedOnUtc
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (GetLocationByPlaceIdResponse is null)
            {
                return Result.Failure<GetLocationByPlaceIdResponse>(new Error(
                    "GetLocationByPlaceId.Null",
                    "The todo with the specified ID was not found"));
            }

            return GetLocationByPlaceIdResponse;
        }
    }
}

public class GetLocationByPlaceIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/getlocationbyblaceid", async (string? pl, string? t, string? o) =>
        {
            //var query = new GetLocationByPlaceId.Query { Id = id };

            //var result = await sender.Send(query);

            //if (result.IsFailure)
            //{
            //    return Results.NotFound(result.Error);
            //}

            return Results.Ok();
        })
            .WithName("GetLocationByPlaceId")
            .WithOpenApi(o => new Microsoft.OpenApi.Models.OpenApiOperation(o)
            {
                Summary = "Get Location By ID",
                Description = "Return Location Details By ID",
                Tags = new List<OpenApiTag>
                {
                    new() { Name="Location" }
                }
            });
    }
}
