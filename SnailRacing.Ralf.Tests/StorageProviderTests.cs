
using Xunit;
using SnailRacing.Ralf.Providers;
using Moq;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using SnailRacing.Ralf.Models;

namespace SnailRacing.Ralf.Tests
{
    public class StorageProviderTests
    {
        [Fact]
        public async Task UpdateStore_Calls_FileStorageProvider_SaveAsync()
        {
            // arrange
            var storage = new StorageProvider<string, object>();
            var jsonProvider = new Mock<IJsonFileStorageProvider<StorageProviderModel<string, object>>>();
            await storage.SetFileStorageProvider(jsonProvider.Object);

            // act

            storage["abc"] = "def";

            // assert
            jsonProvider.Verify(
                x => x.SaveAsync(It.IsAny<StorageProviderModel<string, object>>()),
                Times.Once());
        }

        [Fact]
        public async Task SetFileStorageProvider_Calls_FileStorageProvider_LoadAsync()
        {
            // arrange
            var storage = new StorageProvider<string, object>();
            var jsonProvider = new Mock<IJsonFileStorageProvider<StorageProviderModel<string, object>>>();
            await storage.SetFileStorageProvider(jsonProvider.Object);

            // act

            storage["abc"] = "def";

            // assert
            jsonProvider.Verify(x => x.LoadAsync(),Times.Once());
        }

        [Fact]
        public async Task Indexer_Set_And_Get_Returns_Correct_Value()
        {
            // arrange
            var expected = "def";
            var storage = new StorageProvider<string, object>();
            var jsonProvider = new Mock<IJsonFileStorageProvider<StorageProviderModel<string, object>>>();
            await storage.SetFileStorageProvider(jsonProvider.Object);

            // act
            storage["abc"] = "def";
            var actual = storage["abc"];

            // assert
            Assert.Equal(expected, actual);
        }
    }
}
