using Clean.Solutions.Vertical.Shared;
using MediatR;

namespace Clean.Solutions.Vertical.Abstractions
{
    public interface ICommand : IRequest<Result>
    {
    }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
