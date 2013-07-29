using System;
using TestMagic.Imports.OpenMagic;

namespace TestMagic
{
    public class GivenAssertions<TGiven>
    {
        internal TGiven Value;

        internal GivenAssertions(TGiven givenValue)
        {
            givenValue.MustNotBeNull("givenValue");

            this.Value = givenValue;
        }

        public WhenAssertions<TGiven> When(Action<TGiven> action)
        {
            action.MustNotBeNull("action");

            return new WhenAssertions<TGiven>(this, action);
        }
    }
}
