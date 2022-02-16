using DSharpPlus.Entities;
using SnailRacing.Ralf.Providers;
using System.Collections.Immutable;
using System.Linq;

namespace SnailRacing.Ralf.Discord.Handlers
{
    public class RoleChangedHandler
    {
        private readonly IStorageProvider<RolesStorageProviderModel> storage;

        public RoleChangedHandler(IStorageProvider<RolesStorageProviderModel>? storage)
        {
            this.storage = storage!;
        }

        #region static helpers to handle role change events
        public Task HandleRoleChange(GuildMemberUpdateEventArgs e)
        {
            var roles = e.Member.Roles.Select(r => r.Name).ToArray();
            var newRoles = SyncRoles(roles, async (r) => await ReplaceDiscordMemberRoles(e, r));

            return Task.CompletedTask;
        }

        private async Task ReplaceDiscordMemberRoles(GuildMemberUpdateEventArgs e, string[] r)
        {
            if (HasSameRoles(e.Member.Roles, r)) return;

            var discordRoles = e.Guild.Roles.IntersectBy(r, k => k.Value.Name);

            await e.Member.ReplaceRolesAsync(discordRoles.Select(r => r.Value));
        }
        private bool HasSameRoles(IEnumerable<DiscordRole> currentRoles, string[] newRoles)
        {
            var currentRolesArray = currentRoles.ToArray();

            if (currentRolesArray.Length != newRoles.Length) return false;
            return currentRolesArray.Select(dr => dr.Name).OrderBy(r => r).SequenceEqual(newRoles.OrderBy(r => r));
        }
        #endregion

        public async Task SyncRoles(string[] memberRoles, Func<string[], Task> updateMemberAction)
        {
            var rolesToAdd = GetRolesToAdd(memberRoles, storage.Store);
            var rolesToRemove = GetRolesToRemove(memberRoles, storage.Store);
            var newRoles = DeriveNewRoles(memberRoles, rolesToAdd, rolesToRemove);
            await updateMemberAction(newRoles);
        }

        private string[] DeriveNewRoles(string[] roles, string[] rolesToAdd, string[] rolesToRemove)
        {
            var currentRoles = ImmutableList.Create(roles);
            var rolesWithAdded = currentRoles.AddRange(rolesToAdd);
            var rolesWithAddedAndRemoved = rolesWithAdded.Except(rolesToRemove);

            return rolesWithAddedAndRemoved.ToArray();
        }

        private string[] GetRolesToAdd(string[] roles, RolesStorageProviderModel syncRoles)
        {
            var rolesList = roles.ToList();
            var rolesToAdd = rolesList.ToList()
                .Join(syncRoles, r => r, sr => sr.Key, (r, sr) => sr.Value)
                .Distinct();

            var hasRolesToAdd = !rolesList.Intersect(rolesToAdd).Any();

            return hasRolesToAdd ? rolesToAdd.ToArray() : Array.Empty<string>();
        }

        private string[] GetRolesToRemove(string[] roles, RolesStorageProviderModel syncRoles)
        {
            var rolesList = roles.ToList();
            var excludedRoles = syncRoles
                .ExceptBy(roles, (r) => r.Key)
                .Select(a => a.Value)
                .Distinct();
            var includedRoles = syncRoles
                .IntersectBy(roles, (r) => r.Key)
                .Select(a => a.Value)
                .Distinct();

            var rolesToRemove = excludedRoles.Except(includedRoles);

            return rolesToRemove.ToArray();
        }
    }
}
