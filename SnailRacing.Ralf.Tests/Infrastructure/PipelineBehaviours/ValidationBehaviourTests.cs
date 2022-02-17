using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SnailRacing.Ralf.Infrastrtucture;
using SnailRacing.Ralf.Infrastrtucture.PipelineBehaviours;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SnailRacing.Ralf.Tests.Infrastructure.PipelineBehaviours
{
    public class ValidationBehaviourTests
    {
        [Fact]
        public async Task ValidationBehaviour_Error_Stops_Pipeline()
        {
            // arrange
            var request = new TestRequest();

            var services = new ServiceCollection()
                    .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>))
                    .AddValidatorsFromAssemblyContaining<TestValidator>()
                    .AddMediatR(typeof(TestHandler).Assembly)
                    .BuildServiceProvider();

            var mediatr = services.GetService<IMediator>();

            // act
            var actual = await mediatr?.Send(request);

            // assert
            Assert.NotNull(actual);
            Assert.NotEmpty(actual.Errors);
        }
    }

    public class TestHandler : IRequestHandler<TestRequest, TestResponse>
    {
        public Task<TestResponse> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new TestResponse());
        }
    }

    public class TestRequest : IRequest<TestResponse>
    {
        public string Name { get; set; } = string.Empty;
    }

    public class TestResponse : ResponseBase
    {
    }

    public class TestValidator : AbstractValidator<TestRequest>
    {
        public TestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
