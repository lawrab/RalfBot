using Microsoft.Extensions.DependencyInjection;
using SnailRacing.Ralf.Infrastrtucture;
using System.Threading.Tasks;
using Xunit;

namespace SnailRacing.Ralf.Tests.Handlers
{
    public class CommandDispatcherTests
    {
        [Fact]
        public async Task Calls_Correct_Handler_For_Request()
        {
            // arrange
            var services = new ServiceCollection()
                .AddTransient(typeof(IDispatcher<,>), typeof(CommandDispatcher<,>))
                .AddTransient<ICommand<TestRequest, TestResponse>, TestHandler>()
                .BuildServiceProvider();

            var handler = services.GetService<IDispatcher<TestRequest, TestResponse>>();

            // act
            var actual = await handler!.Send(new TestRequest());

            // assert
            Assert.NotNull(actual);
            Assert.IsType<TestResponse>(actual);
        }
    }

    class TestRequest
    { }

    class TestResponse : ResponseBase
    { }

    class TestHandler : ICommand<TestRequest, TestResponse>
    {
        public Task<TestResponse> Handle(TestRequest request)
        {
            return Task.FromResult(new TestResponse());
        }
    }
}
