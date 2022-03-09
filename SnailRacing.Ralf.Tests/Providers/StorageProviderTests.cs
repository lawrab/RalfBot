
using Moq;
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
            ////// arrange
            ////var key = "aaa";
            ////var expected = "9090";

            ////var storage = new StorageProvider(string.Empty, null);
            ////var jsonProvider = new Mock<IJsonFileStorageProvider>();
            ////await storage.SetFileStorageProvider(jsonProvider.Object);

            ////// act
            ////storage.Store[key] = expected;

            ////// assert
            ////var actual = storage.Store[key];

            ////Assert.Equal(expected, actual);

            ////jsonProvider.Verify(
            ////    x => x.SaveAsync(It.IsAny<object>()),
            ////    Times.Once());
            ///
            Assert.True(false);
        }
    }
}
