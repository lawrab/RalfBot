namespace SnailRacing.Ralf.Handlers
{
    public interface ICommand<TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request);
    }
}
