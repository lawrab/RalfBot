using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.Handlers
{
    public class RoleChangedHandler
    {
        private readonly IStorageProvider<string, object> storage;

        public RoleChangedHandler(IStorageProvider<string, object> storage)
        {
            this.storage = storage;
        }

        public async Task SyncRoles(string[] memberRoles, Func<string[], Task> updateMemberAction)
        {
            await updateMemberAction(memberRoles);
        }

        private string[] DeriveNewRoles(string[] roles, string[] rolesToAdd, string[] rolesToRemove)
        {
            return Array.Empty<string>();
        }

        private string[] GetRolesToAdd()
        {
            return Array.Empty<string>();
        }

        private string[] GetRolesToRemove()
        {
            return Array.Empty<string>();
        }

        ////public async Task SyncRoles(string[] memberRoles)
        ////{
        ////    var allMergeRoles
        ////    var rolesToAdd = e.RolesAfter.Select(r => r.Name).Except(storage.SyncRoles.Keys);
        ////}

        ////public async Task UpdateRoles(DiscordClient discord, GuildMemberUpdateEventArgs e)
        ////{



        ////    var hasTwitchAnnie = e.RolesAfter.Any(r => r.Name == "TwitchAnnie");
        ////    var hasTwitchLarry = e.RolesAfter.Any(r => r.Name == "TwitchLarry");

        ////    await SetTwitchRoles(discord, e, hasTwitchAnnie || hasTwitchLarry);
        ////}

        ////public static async Task SetTwitchRoles(GuildMemberUpdateEventArgs e, bool shouldHaveSubscribers)
        ////{
        ////    var hasSubscribers = e.Member.Roles.Any(r => r.Name == "Subscribers");

        ////    if (hasSubscribers && shouldHaveSubscribers) return;
        ////    if (hasSubscribers && !shouldHaveSubscribers) await RemoveRole(e, "Subscribers");
        ////    if (!hasSubscribers && shouldHaveSubscribers) await AddRole(e, "Subscribers");
        ////}

        ////public static async Task AddRole(GuildMemberUpdateEventArgs e, string roleToAdd)
        ////{
        ////    var subscriberRole = e.Guild.Roles.Single(r => r.Value.Name == roleToAdd);

        ////    var newRoles = e.Member?.Roles?.Append(subscriberRole.Value);
        ////    if (e.Member == null || newRoles == null) return;
        ////    await e.Member.ReplaceRolesAsync(newRoles);
        ////}

        ////public static async Task RemoveRole(GuildMemberUpdateEventArgs e, string roleToRemove)
        ////{
        ////    var subscriberRole = e.Guild.Roles.Single(r => r.Value.Name == roleToRemove);

        ////    var newRoles = e.Member?.Roles?.Where(r => r.Id != subscriberRole.Value.Id);
        ////    if (e.Member == null || newRoles == null) return;
        ////    await e.Member.ReplaceRolesAsync(newRoles);
        ////}
    }
}
