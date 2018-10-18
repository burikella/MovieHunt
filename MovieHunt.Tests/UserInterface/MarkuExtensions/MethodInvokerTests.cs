using System;
using System.Threading.Tasks;
using FluentAssertions;
using MovieHunt.UserInterface.MarkupExtensions;
using NSubstitute;
using NUnit.Framework;

namespace MovieHunt.Tests.UserInterface.MarkuExtensions
{
    [TestFixture]
    public class MethodInvokerTests
    {
        [Test]
        public void InvokeOnObject_TargetIsNull_ArgumentNullExceptionThrown()
        {
            // Arrange
            var sut = new MethodInvoker(nameof(SomeClass.ArgumentLessMethod));

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => sut.InvokeOnObject(null, _ => Array.Empty<object>()));
        }

        [Test]
        public void InvokeOnObject_UnknownMethod_ArgumentNullExceptionThrown()
        {
            // Arrange
            var target = Substitute.For<SomeClass>();
            var sut = new MethodInvoker("SurelyNotFoundMethod");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => sut.InvokeOnObject(target, _ => Array.Empty<object>()));
        }

        [Test]
        public async Task InvokeOnObject_NoArguments_TargetMethodCalled()
        {
            // Arrange
            var target = Substitute.For<SomeClass>();
            var sut = new MethodInvoker(nameof(SomeClass.ArgumentLessMethod));

            // Act
            await sut.InvokeOnObject(target, _ => Array.Empty<object>());

            // Assert
            target.Received().ArgumentLessMethod();
        }

        [Test]
        public async Task InvokeOnObject_FrequencyTooHight_TargetMethodCallSkipped()
        {
            // Arrange
            var target = Substitute.For<SomeClass>();
            var sut = new MethodInvoker(nameof(SomeClass.ArgumentLessMethod), TimeSpan.FromMilliseconds(200));

            // Act
            await sut.InvokeOnObject(target, _ => Array.Empty<object>());
            await sut.InvokeOnObject(target, _ => Array.Empty<object>());

            // Assert
            target.Received(1).ArgumentLessMethod();
        }

        [Test]
        public async Task InvokeOnObject_DelayFitsInLimit_TargetMethodCalledAgain()
        {
            // Arrange
            var target = Substitute.For<SomeClass>();
            var sut = new MethodInvoker(nameof(SomeClass.ArgumentLessMethod), TimeSpan.FromMilliseconds(200));

            // Act
            await sut.InvokeOnObject(target, _ => Array.Empty<object>());
            await Task.Delay(TimeSpan.FromMilliseconds(300));
            await sut.InvokeOnObject(target, _ => Array.Empty<object>());

            // Assert
            target.Received(2).ArgumentLessMethod();
        }

        [Test]
        public async Task InvokeOnObject_NoArgumentsAsync_TargetMethodCalled()
        {
            // Arrange
            var target = Substitute.For<SomeClass>();
            var sut = new MethodInvoker(nameof(SomeClass.ArgumentLessMethodAsync));

            // Act
            await sut.InvokeOnObject(target, _ => Array.Empty<object>());

            // Assert
            await target.Received().ArgumentLessMethodAsync();
        }

        [Test]
        public async Task InvokeOnObject_MethodDefinedInBaseClass_TargetMethodCalled()
        {
            // Arrange
            var target = Substitute.For<SomeClass>();
            var sut = new MethodInvoker(nameof(SomeClass.MethodInBaseClass));

            // Act
            await sut.InvokeOnObject(target, _ => Array.Empty<object>());

            // Assert
            target.MethodInBaseInvoked.Should().BeTrue();
        }

        [Test]
        public async Task InvokeOnObject_SingleArgument_TargetMethodCalled()
        {
            // Arrange
            var target = Substitute.For<SomeClass>();
            var sut = new MethodInvoker(nameof(SomeClass.SingleArgumentMethod));

            // Act
            await sut.InvokeOnObject(target, _ => new object[] {0});

            // Assert
            target.Received().SingleArgumentMethod(0);
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