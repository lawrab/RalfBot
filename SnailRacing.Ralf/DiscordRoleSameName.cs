using DSharpPlus.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SnailRacing.Ralf
{
    internal class DiscordRoleSameName : EqualityComparer<DiscordRole>
    {
        public override bool Equals(DiscordRole? x, DiscordRole? y)
        {
            return x?.Name == y?.Name;
        }

        public override int GetHashCode([DisallowNull] DiscordRole obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
