using System;
using System.Threading.Tasks;
using Moq;
using MovieHunt.UserInterface.MarkupExtensions;
using Xunit;

namespace MovieHunt.Tests.xUnit.UserInterface.MarkupExtensions
{
    public class MethodInvokerTests
    {
        [Fact]
        public void InvokeOnObject_TargetIsNull_ArgumentNullExceptionThrown()
        {
            // Arrange
            var sut = new MethodInvoker(nameof(SomeClass.ArgumentLessMethod));

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => sut.InvokeOnObject(null, _ => Array.Empty<object>()));
        }

        [Fact]
        public void InvokeOnObject_UnknownMethod_ArgumentNullExceptionThrown()
        {
            // Arrange
            var targetMock = new Mock<SomeClass>();
            var sut = new MethodInvoker("SurelyNotFoundMethod");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => sut.InvokeOnObject(targetMock.Object, _ => Array.Empty<object>()));
        }

        [Fact]
        public async Task InvokeOnObject_NoArguments_TargetMethodCalled()
        {
            // Arrange
            var targetMock = new Mock<SomeClass>();
            var sut = new MethodInvoker(nameof(SomeClass.ArgumentLessMethod));

            // Act
            await sut.InvokeOnObject(targetMock.Object, _ => Array.Empty<object>());

            // Assert
            targetMock.Verify(t => t.ArgumentLessMethod(), Times.Once);
        }

        [Fact]
        public async Task InvokeOnObject_FrequencyTooHigh_TargetMethodCallSkipped()
        {
            // Arrange
            var targetMock = new Mock<SomeClass>();
            var sut = new MethodInvoker(nameof(SomeClass.ArgumentLessMethod), TimeSpan.FromMilliseconds(200));

            // Act
            await sut.InvokeOnObject(targetMock.Object, _ => Array.Empty<object>());
            await sut.InvokeOnObject(targetMock.Object, _ => Array.Empty<object>());

            // Assert
            targetMock.Verify(t => t.ArgumentLessMethod(), Times.Once);
        }

        [Fact]
        public async Task InvokeOnObject_DelayFitsInLimit_TargetMethodCalledAgain()
        {
            // Arrange
            var targetMock = new Mock<SomeClass>();
            var sut = new MethodInvoker(nameof(SomeClass.ArgumentLessMethod), TimeSpan.FromMilliseconds(200));

            // Act
            await sut.InvokeOnObject(targetMock.Object, _ => Array.Empty<object>());
            await Task.Delay(TimeSpan.FromMilliseconds(300));
            await sut.InvokeOnObject(targetMock.Object, _ => Array.Empty<object>());

            // Assert
            targetMock.Verify(t => t.ArgumentLessMethod(), Times.Exactly(2));
        }

        [Fact]
        public async Task InvokeOnObject_NoArgumentsAsync_TargetMethodCalled()
        {
            // Arrange
            var targetMock = new Mock<SomeClass>();
            var sut = new MethodInvoker(nameof(SomeClass.ArgumentLessMethodAsync));

            // Act
            await sut.InvokeOnObject(targetMock.Object, _ => Array.Empty<object>());

            // Assert
            targetMock.Verify(t => t.ArgumentLessMethodAsync(), Times.Once);
        }

        [Fact]
        public async Task InvokeOnObject_MethodDefinedInBaseClass_TargetMethodCalled()
        {
            // Arrange
            var targetMock = new Mock<SomeClass>();
            var sut = new MethodInvoker(nameof(SomeClass.MethodInBaseClass));

            // Act
            await sut.InvokeOnObject(targetMock.Object, _ => Array.Empty<object>());

            // Assert
            Assert.True(targetMock.Object.MethodInBaseInvoked);
        }

        [Fact]
        public async Task InvokeOnObject_SingleArgument_TargetMethodCalled()
        {
            // Arrange
            var targetMock = new Mock<SomeClass>();
            var sut = new MethodInvoker(nameof(SomeClass.SingleArgumentMethod));

            // Act
            await sut.InvokeOnObject(targetMock.Object, _ => new object[] {0});

            // Assert
            targetMock.Verify(t => t.SingleArgumentMethod(0), Times.Once);
        }

        public abstract class SomeClass
        {
            public abstract object ArgumentLessMethod();

            public abstract Task<object> ArgumentLessMethodAsync();

            public abstract object SingleArgumentMethod(int arg);

            public bool MethodInBaseInvoked { get; private set; }

            public object MethodInBaseClass() => MethodInBaseInvoked = true;
        }
    }
}