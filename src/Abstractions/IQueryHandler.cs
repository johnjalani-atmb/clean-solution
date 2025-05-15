using Clean.Solutions.Vertical.Shared;
using MediatR;

namespace Clean.Solutions.Vertical.Abstractions
{
    public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
    {
    }
}
