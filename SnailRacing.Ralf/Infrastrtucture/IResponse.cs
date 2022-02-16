namespace SnailRacing.Ralf.Infrastrtucture
{
    public interface IResponse
    {
        bool HasErrors();
        IEnumerable<string> Errors { get; set; }
    }
}
