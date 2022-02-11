
using Xunit;
using SnailRacing.Ralf.Providers;
using Moq;
using System.Threading.Tasks;
using SnailRacing.Ralf.Models;

namespace SnailRacing.Ralf.Tests
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
            var jsonProvider = new Mock<IJsonFileStorageProvider<RolesStorageProviderModel>>();
            await storage.SetFileStorageProvider(jsonProvider.Object);

            // act
            storage.Store[key] = expected;

            // assert
            var actual = storage.Store[key];

            Assert.Equal(expected, actual);

            jsonProvider.Verify(
                x => x.SaveAsync(It.IsAny<RolesStorageProviderModel>()),
                Times.Once());
        }

        [Fact]
        public async Task UpdateStore_Calls_FileStorageProvider_SaveAsync()
        {
            // arrange
            var storage = new StorageProvider<RolesStorageProviderModel>();
            var jsonProvider = new Mock<IJsonFileStorageProvider<RolesStorageProviderModel>>();
            await storage.SetFileStorageProvider(jsonProvider.Object);

            // act

            storage.Store["abc"] = "def";

            // assert
            jsonProvider.Verify(
                x => x.SaveAsync(It.IsAny<RolesStorageProviderModel>()),
                Times.Once());
        }

        [Fact]
        public async Task SetFileStorageProvider_Calls_FileStorageProvider_LoadAsync()
        {
            // arrange
            var storage = new StorageProvider<RolesStorageProviderModel>();
            var jsonProvider = new Mock<IJsonFileStorageProvider<RolesStorageProviderModel>>();

            // act
            await storage.SetFileStorageProvider(jsonProvider.Object);

            // assert
            jsonProvider.Verify(x => x.LoadAsync(),Times.Once());
        }
    }
}
