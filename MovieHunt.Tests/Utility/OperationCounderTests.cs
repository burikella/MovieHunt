using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using MovieHunt.Utility;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace MovieHunt.Tests.Utility
{
    [TestFixture]
    public class OperationCounderTests
    {
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(23)]
        public void RunSeveralTimes_BecamesFalseOnlyWhenAllDisposed(int count)
        {
            // Arrange
            bool state = false;
            var sut = new OperationsCounter(x => state = x);

            // Act
            var disposables = Enumerable.Range(0, count).Select(_ => sut.Run());

            // Assert
            foreach (var disposable in disposables)
            {
                state.Should().BeTrue();
                disposable.Dispose();
            }
            state.Should().BeFalse();
        }

        [Test]
        public void RunRelated_DependentShouldBeTrue()
        {
            // Arrange
            var related = new OperationsCounter(_ => { });

            bool state = false;
            var sut = new OperationsCounter(x => state = x, related);

            // Act
            related.Run();

            // Assert
            state.Should().BeTrue();
        }

        [Test]
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
            counter.Should().Be(4);
        }
    }
}
