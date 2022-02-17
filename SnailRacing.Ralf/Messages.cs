using FluentValidation.Results;

namespace SnailRacing.Ralf
{
    public static class Messages
    {
        public const string INVALID_LEAGUE = "The {0} do not exist. Use !league for a list of active leagues you can join.";
        public const string ALREADY_MEMBER_OF_LEAGUE = "You are already a **{0}** member of **{1}**";
    }
}
