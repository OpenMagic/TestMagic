using System;

namespace TestMagic
{
    // todo: document
    public class WhenAssertions<TGiven>
    {
        internal GivenAssertions<TGiven> Given;
        internal Action<TGiven> Action;

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
    }
}
