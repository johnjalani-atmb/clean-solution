using Clean.Solutions.Vertical.Shared;
using FluentValidation;
using MediatR;
using Serilog.Context;

namespace Clean.Solutions.Vertical.Pipeline
{
    internal sealed class ValidationPipeline<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationPipeline(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {



            if (!_validators.Any())
            {
                return await next();
            }


            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var errors = validationResults.SelectMany(r => r.Errors)
                                            .Where(o => o is not null)
                                            .Select(f => new Error(f.PropertyName, f.ErrorMessage))
                                            .Distinct()
                                            .ToArray();

            if (errors.Any())
            {
                return Error.CreateValidationResult<TResponse>(errors);
            }


            return await next();
        }
    }
}
