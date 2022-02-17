namespace SnailRacing.Ralf.Infrastrtucture
{
    public interface IDispatcher
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> command)
            where TResponse : IResponse;
    }
}
