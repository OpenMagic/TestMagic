using System;

namespace TestMagic
{
    // todo: document
    [Obsolete]
    public class WhenAssertions<TGiven>
    {
        internal GivenAssertions<TGiven> Given;
        internal Action<TGiven> Action;

        // todo: unit tests
        internal WhenAssertions(GivenAssertions<TGiven> given)
        {
            if (!given.ValidatingConstructor)
            {
                throw new InvalidOperationException("todo: message: cannot call when, testing constructor");
            }

            this.Given = given;
        }

        internal WhenAssertions(GivenAssertions<TGiven> given, Action<TGiven> action)
        {
            this.Given = given;
            this.Action = action;
        }

        // todo: document
        public ThenAssertions<TGiven, TException> Then<TException>() where TException : Exception
        {
            return new ThenAssertions<TGiven, TException>(this);
        }

        // todo: document
        public WhenAssertions<TGiven> IsConstructed()
        {
            return this;
        }
    }
}
