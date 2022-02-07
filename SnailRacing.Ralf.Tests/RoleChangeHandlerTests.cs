using DSharpPlus.EventArgs;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using SnailRacing.Ralf.Handlers;
using SnailRacing.Ralf.Providers;
using System.Threading.Tasks;

namespace SnailRacing.Ralf.Tests
{
    public class RoleChangeHandlerTests
    {
        [Fact]
        public async Task Updated_User_With_No_Matches_To_Be_Correct()
        {
            // arrange
            var store = CreateStorageWithRoles( new (string source, string target)[]
            {
                ("RoleB", "Sub"),
                ("RoleD", "Sub"),
                ("RoleE", "Sub2")
            });
            var handler = new RoleChangedHandler(store);
            var expected = new string[]
            {
                "RoleA",
                "RoleB",
                "RoleG",
                "RoleZ",
                "Sub"
            };

            // act
            string[]? actual = null;
            await handler.SyncRoles(expected, (r) =>
            {
                actual = r;
                return Task.CompletedTask;
            });

            // assert
            Assert.Equal(expected, actual);
        }

        private IStorageProvider<string, object> CreateStorageWithRoles((string source, string target)[] syncRoles)
        {
            var storageProvider = new StorageProvider<string, object>();
            foreach (var item in syncRoles)
            {
                storageProvider.AddRole(item.source, item.target);
            }
            return storageProvider;
        }
    }
}
