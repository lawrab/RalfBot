using FluentValidation;

namespace SnailRacing.Ralf.Handlers.News
{
    public class NewsAddRequestValidator : AbstractValidator<NewsAddRequest>
    {
        public NewsAddRequestValidator()
        {
            RuleFor(m => m.GuildId).NotEmpty();
            RuleFor(m => m.Who).NotEmpty();
            RuleFor(m => m.Story).NotEmpty();
        }
    }
}
