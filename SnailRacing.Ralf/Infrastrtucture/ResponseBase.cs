using FluentValidation.Results;

namespace SnailRacing.Ralf.Infrastrtucture
{
    public abstract class ResponseBase
    {
        public IEnumerable<ValidationFailure> Errors { get; set; } = Enumerable.Empty<ValidationFailure>();

        public bool HasErrors()
        {
            return Errors.Any();
        }
    }
}
