using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SnailRacing.Ralf.Providers;
using SnailRacing.Store;

namespace SnailRacing.Ralf.Discord.Commands
{

    // ToDo: refactor storage so source can be used multiple times
    [Group("sync_role"), Hidden]
    public class DiscordRolesModule : BaseCommandModule
    {
        private readonly IStorageProvider _storageProvider;

        public DiscordRolesModule(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        [Command("add")]
        public async Task AddSyncRoles(CommandContext ctx, string source, string target)
        {
            await ctx.TriggerTypingAsync();
            var store = GetStore(ctx);
            store.TryAdd(source, target);
            await ctx.RespondAsync($"Role `{source}` will be monitored and synched with `{target}`.");
        }

        [Command("remove")]
        public async Task RemoveSyncRoles(CommandContext ctx, string source)
        {
            await ctx.TriggerTypingAsync();
            var store = GetStore(ctx);
            store.TryRemove(source);
            await ctx.RespondAsync($"Role `{source}` was removed and will no longer be monitored.");
        }

        [Command("list")]
        public async Task ListSyncRoles(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var store = GetStore(ctx);
            var roles = ListRoles(store).ToList();

            var embed = new DiscordEmbedBuilder
            {
                Title = "Synchronised roles"
            };
            roles.ForEach(i => embed.AddField(i.source, i.target));

            await ctx.RespondAsync(embed);
        }

        private static IEnumerable<(string source, string target)> ListRoles(IStore<string, string> store)
        {
            return store.Select(r => (r.Key, r.Value)) ?? Enumerable.Empty<(string source, string target)>();
        }

        private IStore<string, string> GetStore(CommandContext ctx)
        {
            return _storageProvider.Get<IStore<string, string>>(new StoreKey(ctx.Guild.ToString(), StorageProviderConstants.ROLES));
        }
    }
}
