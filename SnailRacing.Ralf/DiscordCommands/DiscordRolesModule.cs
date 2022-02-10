using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.DiscordCommands
{
    [Group("sync_role"), Hidden]
    internal class DiscordRolesModule : BaseCommandModule
    {
        public IStorageProvider<string, object>? StorageProvider { private get; set; }

        [Command("add")]
        public async Task AddSyncRoles(CommandContext ctx, string source, string target)
        {
            await ctx.TriggerTypingAsync();
            StorageProvider!.AddRole(source, target);
            await ctx.RespondAsync($"Role `{source}` will be monitored and synched with `{target}`.");
        }

        [Command("list")]
        public async Task ListSyncRoles(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var roles = ListRoles(StorageProvider!).ToList();

            var embed = new DiscordEmbedBuilder
            {
                Title = "Synchronised roles"
            };
            roles.ForEach(i => embed.AddField(i.source, i.target));

            await ctx.RespondAsync(embed);
        }

        private IEnumerable<(string source, string target)> ListRoles(IStorageProvider<string, object> storageProvider)
        {
            return storageProvider.SyncRoles.Select(r => (r.Key, r.Value)) ?? Enumerable.Empty<(string source, string target)>();
        }
    }
}
