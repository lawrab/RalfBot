namespace SnailRacing.Ralf.Infrastrtucture
{
    public interface IDispatcher<TRequest, TResponse>
        where TResponse : IResponse
    {
        Task<TResponse> Send(TRequest command);
    }
}
