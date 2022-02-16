namespace SnailRacing.Ralf.Infrastrtucture

{
    public interface ICommand<TRequest, TResponse>
        where TResponse : IResponse
    {
        Task<TResponse> Handle(TRequest request);
    }
}
