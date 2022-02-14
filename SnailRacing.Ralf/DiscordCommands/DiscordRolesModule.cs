using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SnailRacing.Ralf.Providers;

namespace SnailRacing.Ralf.DiscordCommands
{
    [Group("sync_role"), Hidden]
    public class DiscordRolesModule : BaseCommandModule
    {
        public IStorageProvider<RolesStorageProviderModel>? StorageProvider { private get; set; }

        [Command("add")]
        public async Task AddSyncRoles(CommandContext ctx, string source, string target)
        {
            await ctx.TriggerTypingAsync();
            StorageProvider!.Store[source] = target;
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

        private IEnumerable<(string source, string target)> ListRoles(IStorageProvider<RolesStorageProviderModel> storageProvider)
        {
            return storageProvider.Store.Select(r => (r.Key, r.Value)) ?? Enumerable.Empty<(string source, string target)>();
        }
    }
}
