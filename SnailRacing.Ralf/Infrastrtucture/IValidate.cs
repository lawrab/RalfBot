namespace SnailRacing.Ralf.Infrastrtucture
{
    public interface IValidate<TModel>
    {
        IEnumerable<string> IsValid(TModel model);
    }
}
