﻿
using Moq;
using SnailRacing.Ralf.Models;
using SnailRacing.Ralf.Providers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SnailRacing.Ralf.Tests.Providers
{
    public class StorageProviderTests
    {
        [Fact]
        public async Task Add_Role_Adds_To_SyncRole_Dictionary()
        {
            // arrange
            var key = "aaa";
            var expected = "9090";

            var storage = new StorageProvider<RolesStorageProviderModel>();
            var jsonProvider = new Mock<IJsonFileStorageProvider>();
            await storage.SetFileStorageProvider(jsonProvider.Object);

            // act
            storage.Store[key] = expected;

            // assert
            var actual = storage.Store[key];

            Assert.Equal(expected, actual);

            jsonProvider.Verify(
                x => x.SaveAsync(It.IsAny<object>()),
                Times.Once());
        }

        [Fact]
        public async Task UpdateStore_Calls_FileStorageProvider_SaveAsync()
        {
            // arrange
            var storage = new StorageProvider<RolesStorageProviderModel>();
            var jsonProvider = new Mock<IJsonFileStorageProvider>();
            await storage.SetFileStorageProvider(jsonProvider.Object);

            // act

            storage.Store["abc"] = "def";

            // assert
            jsonProvider.Verify(
                x => x.SaveAsync(It.IsAny<object>()),
                Times.Once());
        }

        [Fact]
        public async Task SetFileStorageProvider_Calls_FileStorageProvider_LoadAsync()
        {
            // arrange
            var storage = new StorageProvider<RolesStorageProviderModel>();
            var jsonProvider = new Mock<IJsonFileStorageProvider>();

            // act
            await storage.SetFileStorageProvider(jsonProvider.Object);

            // assert
            jsonProvider.Verify(x => x.LoadAsync(It.IsAny<Type>()), Times.Once());
        }
    }
}