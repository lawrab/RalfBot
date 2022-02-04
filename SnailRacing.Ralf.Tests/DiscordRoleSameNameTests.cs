using DSharpPlus.Entities;
using Moq;
using Xunit;

namespace SnailRacing.Ralf.Tests
{
    public class DiscordRoleSameNameTests
    {
        [Fact]
        public void Equals_Returns_True()
        {
            // Arrange
            var left = TestHelper.CreateInstance <DiscordRole>();
            TestHelper.SetProperty(left, x => x.Name, "ABC");
            var right = TestHelper.CreateInstance<DiscordRole>();
            TestHelper.SetProperty(right, x => x.Name, "ABC");

            var comparer = new DiscordRoleSameName();

            // Act
            var result = comparer.Equals(left, right);

            // Assert
            Assert.True(result);
        }
    }
}
