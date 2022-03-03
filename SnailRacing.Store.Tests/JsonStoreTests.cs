using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;

namespace SnailRacing.Store.Tests
{
    public class JsonStoreTests
    {
        [Fact]
        public void Adding_Data_Saves_Json()
        {
            // arrange
            var store = new JsonStore<string, string>("test.json");

            // act
            store.TryAdd("abc", "def");

            // assert
            var json = File.ReadAllText("test.json");
            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            var actual = data?["abc"];

            Assert.Equal("def", actual);
        }

        [Fact]
        public void Removing_Data_Saves_Json()
        {
            // arrange
            var store = new JsonStore<string, string>("test.json");
            store.TryAdd("abc", "def");

            // act
            store.TryRemove("abc");

            // assert
            var json = File.ReadAllText("test.json");
            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            Assert.Throws<KeyNotFoundException>(() => data?["abc"]);
        }

        ////public void Initialising_Loads_Data_From_Json()
        ////{
        ////    // arrange
        ////    var store = new JsonStore<string, string>(string.Empty);

        ////    // act

        ////    // assert
        ////}
    }
}
