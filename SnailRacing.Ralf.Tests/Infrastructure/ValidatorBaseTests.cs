using SnailRacing.Ralf.Infrastrtucture;
using Xunit;

namespace SnailRacing.Ralf.Tests.Infrastructure
{
    public class ValidatorBaseTests
    {
        [Fact]
        public void Validator_Returns_Errors()
        {
            // arrange
            var validator = new TestValidator();
            var model = new TestModel();

            // act
            var errors = validator.IsValid(model);

            // assert
            Assert.NotEmpty(errors);
            Assert.Contains(errors, (m) => m == "Name is required");
        }

        [Fact]
        public void Validator_Returns_No_Errors()
        {
            // arrange
            var validator = new TestValidator();
            var model = new TestModel
            {
                Name = "JoJo"
            };

            // act
            var errors = validator.IsValid(model);

            // assert
            Assert.Empty(errors);
        }
    }

    class TestModel
    {
        public string Name { get; set; } = string.Empty;
    }

    class TestValidator : ValidatorBase<TestModel>
    {
        public TestValidator()
        {
            AddRule(m => string.IsNullOrEmpty(m.Name) ? "Name is required" : string.Empty);
        }
    }
}
