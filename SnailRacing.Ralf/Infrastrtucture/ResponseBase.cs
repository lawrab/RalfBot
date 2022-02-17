namespace SnailRacing.Ralf.Infrastrtucture
{
    public abstract class ResponseBase
    {
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

        public bool HasErrors()
        {
            return Errors.Any();
        }
    }
}
