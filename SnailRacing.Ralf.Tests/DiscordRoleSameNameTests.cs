using DSharpPlus.Entities;
using Xunit;

/// <summary>
/// The test helper was created to allow us to use DSharpPlus Entities in unit tests.
/// The project at this stage do not justify the effor to abstract DSharpPlus.
/// Reflection is used to instantiate classes with internal constructors and to set
/// property values with internal or private setters
/// </summary>
namespace SnailRacing.Ralf.Tests
{
    public class DiscordRoleSameNameTests
    {
        [Fact]
        public void Equals_Returns_True()
        {
            // Arrange
            var left = TestHelper.CreateInstance <DiscordRole>();
            TestHelper.SetProperty(left, x => x!.Name, "ABC");
            var right = TestHelper.CreateInstance<DiscordRole>();
            TestHelper.SetProperty(right, x => x!.Name, "ABC");

            var comparer = new DiscordRoleSameName();

            // Act
            var result = comparer.Equals(left, right);

            // Assert
            Assert.True(result);
        }
    }
}
