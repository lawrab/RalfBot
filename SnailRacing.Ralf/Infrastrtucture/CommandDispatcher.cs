using Microsoft.Extensions.DependencyInjection;

namespace SnailRacing.Ralf.Infrastrtucture
{
    public class CommandDispatcher : IDispatcher
    {
        private readonly IServiceProvider _services;

        public CommandDispatcher(IServiceProvider services)
        {
            _services = services;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> command)
                    where TResponse : IResponse
        {
            ////var t = command.GetType();
            ////var commandType = typeof(ICommand<,>).MakeGenericType(t, typeof(TResponse));
            ////var service = (ICommand<IRequest<TResponse>, TResponse>)_services.GetService(commandType);
            ///
            var service = _services.GetService<ICommand<IRequest<TResponse>, TResponse>>();

            ////if (service == null) throw new ArgumentException($"Service for {t} not found, make sure the Handler is configured");

            return await service.Handle(command);
        }

    }
}
