using FluentValidation;
using MediatR;

namespace SnailRacing.Ralf.Infrastrtucture.PipelineBehaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : ResponseBase, new()
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext<TRequest>(request);

            var tasks = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context)));
            var failures = tasks
                .Where(x => !x.IsValid)
                .SelectMany(x => x.Errors)
                .ToList();

            var response = new TResponse
            {
                Errors = failures
            };

            return response.HasErrors() ? response : await next();
        }
    }
}
