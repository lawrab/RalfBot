using SnailRacing.Ralf.Providers;
using SnailRacing.Store;
using Xunit;

namespace SnailRacing.Ralf.Tests.Providers
{
    public class StorageProviderTests
    {
        [Fact]
        public void Add_Storage_Returns_Same_Data()
        {
            // arrange
            var group = "1";
            var key = "abc";
            var storageProvider = StorageProvider.Create("Add_Storage_Returns_Same_Data", null);

            // act
            storageProvider.Add(group, key);
            var store = storageProvider.Get<string>(group, key);
            store.TryAdd(key, "123");

            // assert
            var actual = storageProvider.Get<string>(group, key);
            Assert.NotNull(actual);
            Assert.Equal("123", actual[key]);
        }
    }
}
