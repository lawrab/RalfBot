using FluentValidation;

namespace SnailRacing.Ralf.Handlers.News
{
    public class NewsQueryRequestValidator : AbstractValidator<NewsQueryRequest>
    {
        public NewsQueryRequestValidator()
        {
            RuleFor(m => m.GuildId).NotEmpty();
            RuleFor(m => m.Filter).NotEmpty();
        }
    }
}
