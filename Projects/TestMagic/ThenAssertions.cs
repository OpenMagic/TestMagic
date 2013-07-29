using System;
using TestMagic.Imports.OpenMagic;

namespace TestMagic
{
    // todo: document
    public class ThenAssertions<TGiven, TException> where TException : Exception
    {
        private WhenAssertions<TGiven> When;
        private Exception ThrownException;
        private bool ShouldBeThrownCalled;

        internal ThenAssertions(WhenAssertions<TGiven> when)
        {
            when.MustNotBeNull("when");

            this.When = when;
        }

        // todo: document
        public ThenAssertions<TGiven, TException> ShouldBeThrown()
        {
            ShouldBeThrownCalled = true;

            var given = When.Given.Value;
            var when = When.Action;

            try
            {
                when(given);
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

            return this;
        }

        // todo: document
        public ThenAssertions<TGiven, TException> ForParameter(string paramName)
        {
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
    }
}
