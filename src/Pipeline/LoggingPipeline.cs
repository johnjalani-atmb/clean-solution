using Clean.Solutions.Vertical.Shared;
using MediatR;
using Serilog.Context;
using System.Diagnostics;

namespace Clean.Solutions.Vertical.Pipeline
{
    internal sealed class LoggingPipeline<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result

    {
        private readonly ILogger<LoggingPipeline<TRequest, TResponse>> _logger;

        public LoggingPipeline(ILogger<LoggingPipeline<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = new();
            _logger.LogInformation("Handling {RequestName}", typeof(TRequest).Name);
            stopwatch.Start();

            var response = await next();
            //if (response.IsFailure)
            //{
            //    using (LogContext.PushProperty("Error", response., true))
            //    {
            //        _logger.LogError("Completed request {RequestName} with error", typeof(TRequest).Name);
            //    }

            //}

            stopwatch.Stop();

            _logger.LogInformation("Handled {ResponseName} in {ElapsedMilliseconds} ms", typeof(TResponse).Name, stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();

            return response;
        }
    }
}
