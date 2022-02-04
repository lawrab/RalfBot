using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SnailRacing.Ralf.Providers;
using System.Collections.Concurrent;

namespace SnailRacing.Ralf.DiscordCommands
{
    internal class DiscordRolesModule : BaseCommandModule
    {
        // ToDo: create RolesProvider to encapsulate Roles storage
        public IStorageProvider<string, object>? StorageProvider { private get; set; }

        [Command("sync_role")]
        public async Task AddSyncRoles(CommandContext ctx, string source, string target)
        {
            AddRole(StorageProvider!, source, target);
            await ctx.RespondAsync($"Role {source} will be merged with {target}");
        }

        // ToDo: Move to external service to decouple
        private void AddRole(IStorageProvider<string, object> storageProvider, string source, string target)
        {
            // ToDo: define a better data structure (other than dictionary) to hold the roles data
            var roles = storageProvider["Roles"] as ConcurrentDictionary<string, string> ?? new ConcurrentDictionary<string, string>();

            roles[source] = target;
            storageProvider["Roles"] = roles;
        }
    }
}
