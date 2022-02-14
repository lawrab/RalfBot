﻿using DSharpPlus.EventArgs;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using SnailRacing.Ralf.Handlers;
using SnailRacing.Ralf.Providers;
using System.Threading.Tasks;
using SnailRacing.Ralf.Models;

namespace SnailRacing.Ralf.Tests
{
    /// <summary>
    /// Scenario 1:
    /// User added to 1 source role should have the correct target role
    /// 
    /// Scenario 2:
    /// User removed from source role and have no other source roles with 
    /// the target role should get the target role removed
    /// 
    /// Scenario 3:
    /// User removed from source role and have another source role with the
    /// same target role should have no changes
    /// 
    /// Scenarion 4:
    /// User added to source role and already have the target role should 
    /// have no changes
    /// </summary>
    public class RoleChangeHandlerTests
    {

        [Fact]
        public async Task Source_Role_Added_No_Target_Roles_Should_Add_Target_Role()
        {
            // arrange
            var store = CreateStorageWithRoles(new (string source, string target)[]
            {
                ("RoleB", "Sub"),
                ("RoleD", "Sub"),
                ("RoleE", "Sub2")
            });
            var userRoles = new string[]
            {
                "RoleA",
                "RoleG",
                "RoleZ"
            };

            var handler = new RoleChangedHandler(store);

            // act
            string[]? actual = null; 
            var newRoles = userRoles.Union(new [] { "RoleB" }).ToArray();
            await handler.SyncRoles(newRoles, (r) =>
            {
                actual = r;
                return Task.CompletedTask;
            });


            // assert
            var expected = newRoles.Union(new[] { "Sub" }).ToArray();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Source_Role_Added_With_Target_Role_Should_Keep_Target_Role()
        {
            // arrange
            var store = CreateStorageWithRoles(new (string source, string target)[]
            {
                ("RoleB", "Sub"),
                ("RoleD", "Sub"),
                ("RoleE", "Sub2")
            });
            var userRoles = new string[]
            {
                "RoleA",
                "RoleG",
                "RoleD",
                "RoleZ",
                "Sub"
            };

            var handler = new RoleChangedHandler(store);

            // act
            string[]? actual = null;
            var newRoles = userRoles.Union(new[] { "RoleB" }).ToArray();
            await handler.SyncRoles(newRoles, (r) =>
            {
                actual = r;
                return Task.CompletedTask;
            });


            // assert
            var expected = newRoles;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Source_Role_Removed_With_Target_Role_Should_Not_Have_Target_Role()
        {
            // arrange
            var store = CreateStorageWithRoles(new (string source, string target)[]
            {
                ("RoleB", "Sub"),
                ("RoleD", "Sub"),
                ("RoleE", "Sub2")
            });
            var userRoles = new string[]
            {
                "RoleA",
                "RoleC",
                "RoleD",
                "RoleZ",
                "Sub"
            };

            var handler = new RoleChangedHandler(store);

            // act
            string[]? actual = null;
            var newRoles = userRoles.Where(r => r != "RoleD").ToArray();
            await handler.SyncRoles(newRoles, (r) =>
            {
                actual = r;
                return Task.CompletedTask;
            });


            // assert
            var expected = newRoles.Where(r => r != "Sub").ToArray();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Source_Role_Removed_With_Shared_Target_Role_Should_Have_Target_Role()
        {
            // arrange
            var store = CreateStorageWithRoles(new (string source, string target)[]
            {
                ("RoleB", "Sub"),
                ("RoleD", "Sub"),
                ("RoleE", "Sub2")
            });
            var userRoles = new string[]
            {
                "RoleA",
                "RoleB", // --> Shared target role
                "RoleD",
                "RoleZ",
                "Sub"
            };

            var handler = new RoleChangedHandler(store);

            // act
            string[]? actual = null;
            var newRoles = userRoles.Where(r => r != "RoleD").ToArray();
            await handler.SyncRoles(newRoles, (r) =>
            {
                actual = r;
                return Task.CompletedTask;
            });


            // assert
            var expected = newRoles;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Updated_User_With_No_Matches_Should_Be_Correct()
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

        private IStorageProvider<RolesStorageProviderModel> CreateStorageWithRoles((string source, string target)[] syncRoles)
        {
            var storageProvider = new StorageProvider<RolesStorageProviderModel>();
            foreach (var item in syncRoles)
            {
                storageProvider.Store[item.source] = item.target;
            }
            return storageProvider;
        }
    }
}
