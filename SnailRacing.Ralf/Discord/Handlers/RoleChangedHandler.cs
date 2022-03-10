using DSharpPlus.Entities;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Providers;
using SnailRacing.Store;
using System.Collections.Immutable;
using System.Linq;

namespace SnailRacing.Ralf.Discord.Handlers
{
    public class RoleChangedHandler
    {
        private readonly IStorageProvider _storageProvider;

        public RoleChangedHandler(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider!;
        }


        // ToDo refactor and move to mediatr
        #region static helpers to handle role change events
        public Task HandleRoleChange(GuildMemberUpdateEventArgs e)
        {
            var roles = e.Member.Roles.Select(r => r.Name).ToArray();
            var store = StoreHelper.GetRolesStore(e.Guild.Id.ToString(), _storageProvider);
            var newRoles = SyncRoles(roles, store, async (r) => await ReplaceDiscordMemberRoles(e, r));

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

        public async Task SyncRoles(string[] memberRoles, IStore<string> store,Func<string[], Task> updateMemberAction)
        {
            var rolesToAdd = GetRolesToAdd(memberRoles, store);
            var rolesToRemove = GetRolesToRemove(memberRoles, store);
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

        private string[] GetRolesToAdd(string[] roles, IStore<string> store)
        {
            var rolesList = roles.ToList();
            var rolesToAdd = rolesList.ToList()
                .Join(store, r => r, sr => sr.Key, (r, sr) => sr.Value)
                .Distinct();

            var hasRolesToAdd = !rolesList.Intersect(rolesToAdd).Any();

            return hasRolesToAdd ? rolesToAdd.ToArray() : Array.Empty<string>();
        }

        private string[] GetRolesToRemove(string[] roles, IStore<string> store)
        {
            var rolesList = roles.ToList();
            var excludedRoles = store
                .ExceptBy(roles, (r) => r.Key)
                .Select(a => a.Value)
                .Distinct();
            var includedRoles = store
                .IntersectBy(roles, (r) => r.Key)
                .Select(a => a.Value)
                .Distinct();

            var rolesToRemove = excludedRoles.Except(includedRoles);

            return rolesToRemove.ToArray();
        }
    }
}
