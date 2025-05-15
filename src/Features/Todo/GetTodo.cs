using Carter;
using Clean.Solutions.Vertical.Contracts;
using Clean.Solutions.Vertical.Database;
using Clean.Solutions.Vertical.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Clean.Solutions.Vertical.Features.Todo;

public static class GetTodo
{
    public class Query : IRequest<Result<GetTodoResponse>>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Result<GetTodoResponse>>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<GetTodoResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var GetTodoResponse = await _dbContext
                .Todos
                .AsNoTracking()
                .Where(o => o.Id == request.Id)
                .Select(o => new GetTodoResponse
                {
                    Id = o.Id,
                    Subject = o.Subject,
                    Content = o.Content,
                    CreatedOnUtc = o.CreatedOnUtc
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (GetTodoResponse is null)
            {
                return Result.Failure<GetTodoResponse>(new Error(
                    "GetTodo.Null",
                    "The todo with the specified ID was not found"));
            }

            return GetTodoResponse;
        }
    }
}

public class GetTodoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/todo/{id}", async (Guid id, ISender sender) =>
        {
            var query = new GetTodo.Query { Id = id };

            var result = await sender.Send(query);

            if (result.IsFailure)
            {
                return Results.NotFound(result.Error);
            }

            return Results.Ok(result.Value);
        })
            .WithName("GetTodoById")
            .WithOpenApi(o => new Microsoft.OpenApi.Models.OpenApiOperation(o)
            {
                Summary = "Get Todo by ID",
                Description = "Returns all the Todo items based on the supplied ID",
                Tags = new List<OpenApiTag>
                {
                    new() { Name="Todo" }
                }
            });
    }
}
