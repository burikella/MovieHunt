using FluentAssertions;
using NUnit.Framework;

namespace MovieHunt.Tests
{
    [TestFixture]
    public class ReportBuilderTests
    {
        private ReportBuilder _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new ReportBuilder();
        }

        [Test]
        public void AddAmount_NonZero_AddsRecord()
        {
            // Arrange
            decimal amount = 1m;

            // Act
            _sut.AddAmountIfNeeded(amount);

            // Assert
            _sut.Amounts.Should().HaveCount(1);
        }
    }
}