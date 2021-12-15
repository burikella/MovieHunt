using System;
using System.Threading.Tasks;
using FluentAssertions;
using MovieHunt.Utility;
using NSubstitute;
using NUnit.Framework;

namespace MovieHunt.Tests.Utility
{
    [TestFixture]
    public class AsyncLazyTests
    {
        [Test]
        public async Task GetValue_FirstTime_ShouldCallFactory()
        {
            // Arrange
            var factory = Substitute.For<Func<Task<int>>>();
            var sut = new AsyncLazy<int>(factory);

            // Act
            await sut.Value;

            // Assert
            await factory.Received()();
        }

        [Test]
        public async Task GetValue_SecondTime_ShouldNotCallFactoryAgain()
        {
            // Arrange
            var factory = Substitute.For<Func<Task<int>>>();
            var sut = new AsyncLazy<int>(factory);

            // Act
            await sut.Value;

            // Assert
            await factory.Received(1)();
        }

        [TestCase(-100)]
        [TestCase(0)]
        [TestCase(100)]
        public async Task GetValue_ShouldReturnValueCreatedByFactory(int value)
        {
            // Arrange
            var factory = Substitute.For<Func<Task<int>>>();
            factory().Returns(value);
            var sut = new AsyncLazy<int>(factory);

            // Act
            var result = await sut.Value;

            // Assert
            result.Should().Be(value);
        }
    }
}