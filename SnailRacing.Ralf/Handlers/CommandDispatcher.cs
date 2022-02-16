using Microsoft.Extensions.DependencyInjection;

namespace SnailRacing.Ralf.Handlers
{
    public class CommandDispatcher<TRequest, TResponse> : IDispatcher<TRequest, TResponse>
    {
        private readonly IServiceProvider _services;

        public CommandDispatcher(IServiceProvider services)
        {
            _services = services;
        }

        public async Task<TResponse> Send(TRequest command)
        {
            var service = _services.GetService<ICommand<TRequest, TResponse>>();

            if (service == null) throw new ArgumentException($"Service for {typeof(TRequest)} not found, make sure the Handler is configured");

            return await service.Handle(command);
        }
    }
}
