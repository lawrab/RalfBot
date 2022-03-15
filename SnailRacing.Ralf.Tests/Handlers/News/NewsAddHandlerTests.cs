using SnailRacing.Ralf.Handlers.News;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Tests.Builder;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SnailRacing.Ralf.Tests.Handlers.News
{
    public class NewsAddHandlerTests
    {
        [Fact]
        public async Task News_Add_Requests_Stores_News()
        {
            // arrange
            var request = new NewsAddRequest
            {
                GuildId = "1",
                Story = "My Story",
                Who = "abc",
                When = System.DateTime.MaxValue
            };
            var storage = StorageProviderBuilder.Create("1News_Add_Requests_Stores_News", true)
                .Build();

            var handler = new NewsAddHandler(storage);

            // act
            var response = await handler.Handle(request, CancellationToken.None);

            // assert
            var newsStore = StoreHelper.GetNewsStore(request.GuildId, storage);
            var actual = newsStore[response.Key];
            Assert.False(response.HasErrors());
            Assert.Equal(request.When, actual.When);
            Assert.Equal(request.Who, actual.Who);
            Assert.Equal(request.Story, actual.Story);
        }
    }
}
