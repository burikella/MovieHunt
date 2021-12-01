using FluentAssertions;
using NUnit.Framework;

namespace MovieHunt.Tests
{
    [TestFixture]
    public class ReportBuilderTests
    {
        [Test]
        public void AddAmount_NonZero_AddsRecord()
        {
            // Arrange
            var builder = new ReportBuilder(false);
            decimal amount = 1m;

            // Act
            builder.AddAmountIfNeeded(amount);

            // Assert
            builder.Amounts.Should().HaveCount(1);
        }
    }
}
