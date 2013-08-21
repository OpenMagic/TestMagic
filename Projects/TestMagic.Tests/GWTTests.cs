using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMagic.Tests
{
    [TestClass]
    public class GWTTests
    {
        [TestMethod]
        public void PassingExample()
        {
            GWT.Given(new Helper())
                .When(g => g.ArgumentNullTester(null))
                .Then<ArgumentNullException>().ShouldBeThrown().ForParameter("value");
        }

        [TestMethod]
        public void FailingExample()
        {
            try
            {
                GWT.Given(new Helper())
                    .When(g => g.ArgumentNullTester(null))
                    .Then<System.IO.FileNotFoundException>().ShouldBeThrown();
            }
            catch (Exception ex)
            {
                if (ex.Message != "Expected System.IO.FileNotFoundException, but found System.ArgumentNullException: Value cannot be null.\r\nParameter name: value.")
                {
                    throw;
                }
            }
        }

        [TestClass]
        public class Given_Tests
        {
            [TestMethod]
            public void ShouldReturnInstanceOfGWT()
            {
                // Given
                var givenValue = new Object();

                // When
                var given = GWT.Given(givenValue);

                // Then
                given.Should().NotBeNull().And.BeOfType<GivenAssertions<object>>();
            }

            [TestMethod]
            public void ShouldThrowArgumentNullExceptionWhen_givenValue_IsNull()
            {
                // Given

                // When
                Action action = () => GWT.Given<object>(givenValue: null);

                // Then
                action.ShouldThrow<ArgumentNullException>().
                    And.ParamName.Should().Be("givenValue");
            }
        }

        [TestClass]
        public class When
        {
            [TestMethod]
            public void ShouldReturnWhenAssertion()
            {
                // Given
                var given = GWT.Given(new Exception());

                // When
                var when = given.When(g => g.ToString());

                // Then
                when.Should().NotBeNull();
                when.Should().BeOfType<WhenAssertions<Exception>>();
            }

            [TestMethod]
            public void ShouldThrowArgumentNullExceptionWhen_givenValue_IsNull()
            {
                // Given
                var given = GWT.Given(new Exception());

                // When
                Action action = () => given.When(null);

                // Then
                action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("action");
            }
        }

        [TestClass]
        public class Then
        {
            [TestMethod]
            public void ShouldReturnThenAssertion()
            {
                // Given
                var given = GWT.Given(new Exception());
                var when = given.When(g => g.ToString());

                // When
                var then = when.Then<ArgumentNullException>();

                // Then
                then.Should().NotBeNull();
                then.Should().BeOfType<ThenAssertions<Exception, ArgumentNullException>>();
            }
        }

        [TestClass]
        public class ShouldBeThrown
        {
            [TestMethod]
            public void ShouldThrowExceptionIfWhenDoesNotThrowExceptedException()
            {
                // Given
                var given = GWT.Given(new Helper()).When(g => g.ArgumentNullTester(null)).Then<InvalidOperationException>();

                // When
                Action action = () => given.ShouldBeThrown();

                // Then
                action.ShouldThrow<Exception>().WithMessage("Expected System.InvalidOperationException, but found System.ArgumentNullException: Value cannot be null.\r\nParameter name: value.");
            }

            [TestMethod]
            public void ShouldThrowExceptionIfWhenDoesNotThrowAnException()
            {
                // Given
                var given = GWT.Given(new Helper()).When(g => g.ArgumentNullTester("fake")).Then<Exception>();

                // When
                Action action = () => given.ShouldBeThrown();

                // Then
                action.ShouldThrow<Exception>().WithMessage("Expected System.Exception to be thrown but not exception was thrown.");
            }

            [TestMethod]
            public void ShouldNotThrowExceptionIfWhenThrowsExceptedException()
            {
                // Given
                var given = GWT.Given(new Helper()).When(g => g.ArgumentNullTester(null)).Then<ArgumentNullException>();
                
                // When
                Action action = () => given.ShouldBeThrown();

                // Then
                action.ShouldNotThrow<Exception>();
            }

            [TestMethod]
            public void ShouldReturnSameInstanceOfGWT()
            {
                // Given
                var given = GWT.Given(new Helper()).When(h => h.ThrowException()).Then<Exception>();

                // When
                var givenShouldBeThrown = given.ShouldBeThrown();

                // Then
                givenShouldBeThrown.Should().BeSameAs(given);
            }
        }

        [TestClass]
        public class ForParameter
        {
            [TestMethod]
            public void ShouldNotThrowExceptionIfExpectedParameterNameIsThrown()
            {
                // Given
                var given = GWT.Given(new Helper()).When(g => g.ArgumentNullTester(null)).Then<ArgumentNullException>().ShouldBeThrown();
                
                // When
                Action action = () => given.ForParameter("value");

                // Then
                action.ShouldNotThrow<Exception>();
            }

            [TestMethod]
            public void ShouldThrowExceptionIfExpectedParameterNameIsNotThrown()
            {
                // Given
                var given = GWT.Given(new Helper()).When(g => g.ArgumentNullTester(null)).Then<ArgumentNullException>().ShouldBeThrown();
                
                // When
                Action action = () => given.ForParameter("wrong parameter name");

                // Then
                action.ShouldThrow<Exception>().WithMessage("Expected parameter name wrong parameter name, but found value.");
            }

            [TestMethod]
            public void ShouldThrowExceptionIfThrownExceptionMessageDoesNotIncludeParameterName()
            {
                // Given
                var given = GWT.Given(new Helper()).When(g => g.ThrowException()).Then<Exception>().ShouldBeThrown();

                // When
                Action action = () => given.ForParameter("fake name");

                // Then
                action.ShouldThrow<Exception>().WithMessage("Expected parameter name to be included exception message 'deliberately threw exception'.");
            }

            [TestMethod]
            public void ShouldThrowInvalidOperationExceptionIfForParameterIsCalledBeforeShouldBeThrown()
            {
                // Given
                var given = GWT.Given(new Helper()).When(g => g.ThrowException()).Then<Exception>();

                // When
                Action action = () => given.ForParameter("fake name");

                // Then
                action.ShouldThrow<InvalidOperationException>().WithMessage("ForParameter(paramName) cannot be called before ShouldBeThrown().");
            }
        }

        private class Helper
        {
            public void ThrowException()
            {
                throw new Exception("deliberately threw exception");
            }

            public void ArgumentNullTester(object value)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
            }
        }
    }
}
