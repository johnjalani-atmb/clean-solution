using Clean.Solutions.Vertical.Database;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Carter;
using FluentValidation;
using Clean.Solutions.Vertical.Shared;
using Clean.Solutions.Vertical.Contracts;
using Mapster;
using Microsoft.OpenApi.Models;
using Clean.Solutions.Vertical.Abstractions;
using Clean.Solutions.Vertical.Primitives;

namespace Clean.Solutions.Vertical.Features.Todo;

public static class CreateTodo
{
    public record Command(string Subject, string Content) : IRequest<Result<Guid>>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(o => o.Subject).NotEmpty().WithErrorCode(Constants.ErrorCode.Validation.CreateTodo);
            RuleFor(o => o.Content).NotEmpty().WithErrorCode(Constants.ErrorCode.Validation.CreateTodo);
        }
    }

    internal sealed class Handler(ApplicationDbContext context, IValidator<Command> validator, IUnitOfWork unitOfWork) : AggregateRoot, IRequestHandler<Command, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var todo = new Entities.Todo
            {
                Subject = request.Subject,
                Content = request.Content
            };

            context.Add(todo);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            RaiseDomainEvent(new DomainEvent(todo.Id));

            return await Task.FromResult(todo.Id);
        }
    }

    public sealed record DomainEvent(Guid Id) : IDomainEvent;

    internal sealed class DomainEventHandler : IDomainEventHandler<DomainEvent>
    {
        public DomainEventHandler()
        {
            //add needed services via IoC
        }
        public async Task Handle(DomainEvent notification, CancellationToken cancellationToken)
        {
            //This is triggered after the creation of todo
            throw new NotImplementedException();
        }
    }
}

public class CreateTodoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/todo", async (CreateTodoRequest request, ISender sender) =>
        {

            var command = request.Adapt<CreateTodo.Command>();

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return BaseApi.HandleFailure(result);
            }

            return Results.Created($"api/todo/{result.Value}", result);

        })
            .WithName("CreateTodo")
            .WithOpenApi(o => new Microsoft.OpenApi.Models.OpenApiOperation(o)
            {
                Summary = "Create Todo",
                Description = "Create a Todo",
                Tags = new List<OpenApiTag>
                {
                    new() { Name="Todo" }
                }
            });
    }
}
