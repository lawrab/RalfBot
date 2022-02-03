namespace SnailRacing.Ralf
{
    internal static class RoleChangedHandler
    {
        internal static async Task UpdateRoles(DiscordClient discord, GuildMemberUpdateEventArgs e)
        {
            var hasTwitchAnnie = e.RolesAfter.Any(r => r.Name == "TwitchAnnie");
            var hasTwitchLarry = e.RolesAfter.Any(r => r.Name == "TwitchLarry");

            await SetTwitchRoles(discord, e, hasTwitchAnnie || hasTwitchLarry);
        }

        internal static async Task SetTwitchRoles(DiscordClient discord, GuildMemberUpdateEventArgs e, bool shouldHaveSubscribers)
        {
            var hasSubscribers = e.Member.Roles.Any(r => r.Name == "Subscribers");

            if (hasSubscribers && shouldHaveSubscribers) return;
            if (hasSubscribers && !shouldHaveSubscribers) await RemoveRole(discord, e, "Subscribers");
            if (!hasSubscribers && shouldHaveSubscribers) await AddRole(discord, e, "Subscribers");
        }

        internal static async Task AddRole(DiscordClient discord, GuildMemberUpdateEventArgs e, string roleToAdd)
        {
            var subscriberRole = e.Guild.Roles.Single(r => r.Value.Name == roleToAdd);

            var newRoles = e.Member?.Roles?.Append(subscriberRole.Value);
            if (e.Member == null || newRoles == null) return;
            await e.Member.ReplaceRolesAsync(newRoles);
        }

        internal static async Task RemoveRole(DiscordClient discord, GuildMemberUpdateEventArgs e, string roleToRemove)
        {
            var subscriberRole = e.Guild.Roles.Single(r => r.Value.Name == roleToRemove);

            var newRoles = e.Member?.Roles?.Where(r => r.Id != subscriberRole.Value.Id);
            if (e.Member == null || newRoles == null) return;
            await e.Member.ReplaceRolesAsync(newRoles);
        }
    }
}
