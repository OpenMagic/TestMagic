using System;
using TestMagic.Imports.OpenMagic;

namespace TestMagic
{
    // todo: document
    public class GivenAssertions<TGiven>
    {
        internal TGiven Value;

        internal GivenAssertions(TGiven givenValue)
        {
            givenValue.MustNotBeNull("givenValue");

            this.Value = givenValue;
        }

        // todo: document
        public WhenAssertions<TGiven> When(Action<TGiven> action)
        {
            action.MustNotBeNull("action");

            return new WhenAssertions<TGiven>(this, action);
        }
    }
}
