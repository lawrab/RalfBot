namespace SnailRacing.Ralf.Handlers
{
    public interface IDispatcher<TRequest, TResponse>
    {
        Task<TResponse> Send(TRequest command);
    }
}
