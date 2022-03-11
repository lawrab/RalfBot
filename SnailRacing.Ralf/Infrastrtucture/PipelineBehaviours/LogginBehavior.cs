using MediatR;
using Microsoft.Extensions.Logging;

namespace SnailRacing.Ralf.Infrastrtucture.PipelineBehaviours
{
    public class LogginBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : ResponseBase, new()
    {
        private readonly ILogger<Mediator> _logger;

        public LogginBehavior(ILogger<Mediator> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation("Mediatr request {@Request}", request);

            return await next();
        }
    }
}
