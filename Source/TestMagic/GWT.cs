using System;

namespace TestMagic
{
    // todo: document
    [Obsolete]
    public class GWT
    {
        // todo: document
        public static GivenAssertions<TGiven> Given<TGiven>(TGiven givenValue)
        {
            return new GivenAssertions<TGiven>(givenValue);
        }

        public static WhenAssertions<TGiven> When<TGiven>()
        {
            var given = new GivenAssertions<TGiven>();

            return given.When();
        }
    }
}
