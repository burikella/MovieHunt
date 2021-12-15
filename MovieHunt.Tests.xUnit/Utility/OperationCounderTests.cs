using System;
using System.Linq;
using MovieHunt.Utility;
using Xunit;

namespace MovieHunt.Tests.xUnit.Utility
{
    public class OperationCounterTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(23)]
        public void RunSeveralTimes_BecameFalseOnlyWhenAllDisposed(int count)
        {
            // Arrange
            bool state = false;
            var sut = new OperationsCounter(x => state = x);

            // Act
            var disposables = Enumerable.Range(0, count).Select(_ => sut.Run());

            // Assert
            foreach (var disposable in disposables)
            {
                Assert.True(state);
                disposable.Dispose();
            }
            Assert.False(state);
        }

        [Fact]
        public void RunRelated_DependentShouldBeTrue()
        {
            // Arrange
            var related = new OperationsCounter(_ => { });

            bool state = false;
            var sut = new OperationsCounter(x => state = x, related);

            // Act
            related.Run();

            // Assert
            Assert.True(state);
        }

        [Fact]
        public void ChangeStateFailed_ContinueUpdatingState()
        {
            // Arrange
            int counter = 0;
            void ChangeState(bool state)
            {
                counter++;
                if (state)
                {
                    throw new Exception();
                }
            }
            
            var sut = new OperationsCounter(ChangeState);
            counter = 0;

            // Act
            sut.Run().Dispose();
            sut.Run().Dispose();

            // Assert
            Assert.Equal(4, counter);
        }
    }
}
