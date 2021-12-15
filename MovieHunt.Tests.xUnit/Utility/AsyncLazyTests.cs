using System;
using System.Threading.Tasks;
using Moq;
using MovieHunt.Utility;
using Xunit;

namespace MovieHunt.Tests.xUnit.Utility
{
    public class AsyncLazyTests
    {
        [Fact]
        public async Task GetValue_FirstTime_ShouldCallFactory()
        {
            // Arrange
            var factoryMock = new Mock<Func<Task<int>>>();
            var sut = new AsyncLazy<int>(factoryMock.Object);

            // Act
            await sut.Value;

            // Assert
            factoryMock.Verify(f => f());
        }

        [Fact]
        public async Task GetValue_SecondTime_ShouldNotCallFactoryAgain()
        {
            // Arrange
            var factoryMock = new Mock<Func<Task<int>>>();
            var sut = new AsyncLazy<int>(factoryMock.Object);

            // Act
            await sut.Value;

            // Assert
            factoryMock.Verify(f => f(), Times.Once);
        }

        [Theory]
        [InlineData(-100)]
        [InlineData(0)]
        [InlineData(100)]
        public async Task GetValue_ShouldReturnValueCreatedByFactory(int value)
        {
            // Arrange
            var factoryMock = new Mock<Func<Task<int>>>();
            factoryMock
                .Setup(f => f())
                .ReturnsAsync(value);
            var sut = new AsyncLazy<int>(factoryMock.Object);

            // Act
            var result = await sut.Value;

            // Assert
            Assert.Equal(value, result);
        }
    }
}