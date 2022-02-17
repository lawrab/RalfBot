namespace SnailRacing.Ralf.Infrastrtucture

{
    public interface ICommand<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request);
    }
}
