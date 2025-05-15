using Clean.Solutions.Vertical.Shared;
using MediatR;
using MediatR.Pipeline;
using System.Diagnostics;

namespace Clean.Solutions.Vertical.Pipeline
{
    internal sealed class UnhandledExceptionPipeline<TRequest, TResponse, TException>
        : IRequestExceptionHandler<TRequest, TResponse, TException>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
        where TException : Exception
    {
        private readonly ILogger<LoggingPipeline<TRequest, TResponse>> _logger;
        public UnhandledExceptionPipeline(ILogger<LoggingPipeline<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
        {
            var ex = exception.Demystify();
            _logger.LogError(ex, "Unhandled Exception for Request {@Request}", typeof(TRequest));

            var errors = new List<Error> { new(ex.Message, ex.StackTrace) };

            state.SetHandled(Error.CreateValidationResult<TResponse>([.. errors]));

            return Task.CompletedTask;
        }
    }
}
