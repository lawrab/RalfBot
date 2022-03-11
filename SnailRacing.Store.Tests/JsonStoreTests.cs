using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SnailRacing.Store.Tests
{
    public class JsonStoreTests
    {
        [Fact]
        public void Adding_Data_Saves_Json()
        {
            // arrange
            var store = new JsonStore<string>("Adding_Data_Saves_Json.json");

            // act
            store.TryAdd("abc", "def");

            // assert
            var json = File.ReadAllText("Adding_Data_Saves_Json.json");
            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            var actual = data?["abc"];

            Assert.Equal("def", actual);
        }

        [Fact]
        public void Removing_Data_Saves_Json()
        {
            // arrange
            var store = new JsonStore<string>("Removing_Data_Saves_Json.json");
            store.TryAdd("abc", "def");

            // act
            store.TryRemove("abc");

            // assert
            var json = File.ReadAllText("Removing_Data_Saves_Json.json");
            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            Assert.Throws<KeyNotFoundException>(() => data?["abc"]);
        }

        [Fact]
        public async Task Initialising_Loads_Data_From_Json()
        {
            // arrange
            var expectedStore = new JsonStore<string>("Initialising_Loads_Data_From_Json.json");
            await expectedStore.Init();
            expectedStore.TryAdd("abc", "def");

            // act
            var actualStore = new JsonStore<string>("Initialising_Loads_Data_From_Json.json");
            await actualStore.Init();

            // assert
            Assert.Equal(expectedStore["abc"], actualStore["abc"]);
        }
    }
}
