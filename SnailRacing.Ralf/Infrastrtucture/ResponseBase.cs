namespace SnailRacing.Ralf.Infrastrtucture
{
    public abstract class ResponseBase : IResponse
    {
        protected List<string> _errors = new List<string>();
        public IEnumerable<string> Errors
        {
            get => _errors;
        }

        public bool HasErrors()
        {
            return _errors.Any();
        }
    }
}
