using Xunit;

namespace SnailRacing.Store.Tests
{
    public class MemoryStoreTests
    {
        [Fact]
        public void Data_Can_Be_Retrieved_After_Storing_It()
        {
            // arrange
            var key = "abc";
            var store = new MemoryStore<string>();

            var value = "def";
            // act
            store.TryAdd(key, value);
            var actual = store[key];

            // assert
            Assert.Equal(value, actual);
        }
    }
}