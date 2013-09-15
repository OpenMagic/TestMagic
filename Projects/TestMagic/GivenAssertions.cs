using System;
using TestMagic.Imports.OpenMagic;

namespace TestMagic
{
    // todo: document
    public class GivenAssertions<TGiven>
    {
        internal TGiven Value;
        internal readonly bool ValidatingConstructor;

        internal GivenAssertions()
        {
            this.ValidatingConstructor = true;
        }

        internal GivenAssertions(TGiven givenValue)
        {
            givenValue.MustNotBeNull("givenValue");

            this.Value = givenValue;
        }

        // todo: document
        // todo: unit tests
        public WhenAssertions<TGiven> When()
        {
            if (!this.ValidatingConstructor)
            {
                throw new InvalidOperationException("todo: message: cannot call when, testing constructor");
            }

            return new WhenAssertions<TGiven>(this);
        }

        // todo: document
        public WhenAssertions<TGiven> When(Action<TGiven> action)
        {
            action.MustNotBeNull("action");

            // todo: unit this if.
            if (this.ValidatingConstructor)
            {
                throw new InvalidOperationException("todo: message: cannot call when, testing constructor");
            }

            return new WhenAssertions<TGiven>(this, action);
        }
    }
}
