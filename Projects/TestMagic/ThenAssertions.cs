using System;

namespace TestMagic
{
    // todo: document
    [Obsolete]
    public class ThenAssertions<TGiven, TException> where TException : Exception
    {
        private WhenAssertions<TGiven> When;
        private Exception ThrownException;
        private bool ShouldBeThrownCalled;

        internal ThenAssertions(WhenAssertions<TGiven> when)
        {
            this.When = when;
        }

        // todo: document
        public ThenAssertions<TGiven, TException> ShouldBeThrown()
        {
            // todo: unit test is statement
            if (this.When.Given.ValidatingConstructor)
            {
                // Assuming ForAllParameters or ForParameter will be called.
                return this;
            }

            ShouldBeThrownCalled = true;

            var exceptionNotThrown = false;
            var given = this.When.Given.Value;
            var when = this.When.Action;

            try
            {
                when(given);
                exceptionNotThrown = true;
            }
            catch (TException ex)
            {
                // Expected exception was thrown.
                ThrownException = ex;
            }
            catch (Exception ex)
            {
                ThrownException = ex;

                var message = string.Format("Expected {0}, but found {1}: {2}.", typeof(TException), ex.GetType(), ex.Message);

                throw new Exception(message);
            }

            if (exceptionNotThrown)
            {
                throw new Exception(string.Format("Expected {0} to be thrown but not exception was thrown.", typeof(TException)));
            }

            return this;
        }

        // todo: document
        public ThenAssertions<TGiven, TException> ForParameter(string paramName)
        {
            // todo: this if requires unit test.
            if (this.When.Given.ValidatingConstructor)
            {
                new ConstructorFor<TGiven>().ShouldThrowArgumentNullException(forParameter: paramName);
                return this;
            }

            if (!ShouldBeThrownCalled)
            {
                throw new InvalidOperationException("ForParameter(paramName) cannot be called before ShouldBeThrown().");
            }

            if (ThrownException.Message.EndsWith("Parameter name: " + paramName))
            {
                return this;
            }

            var index = ThrownException.Message.IndexOf("Parameter name: ");

            if (index == -1)
            {
                throw new Exception(string.Format("Expected parameter name to be included exception message '{0}'.", ThrownException.Message));
            }

            var actualParamName = ThrownException.Message.Substring(index + 16);

            throw new Exception(string.Format("Expected parameter name {0}, but found {1}.", paramName, actualParamName));
        }

        // todo: document
        // todo: unit tests
        public void ForAllParameters()
        {
            new ConstructorFor<TGiven>().ShouldThrowArgumentNullExceptionForAllParameters();
        }
    }
}
