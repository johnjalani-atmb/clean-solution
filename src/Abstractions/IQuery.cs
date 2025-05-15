using Clean.Solutions.Vertical.Shared;
using MediatR;

namespace Clean.Solutions.Vertical.Abstractions
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
