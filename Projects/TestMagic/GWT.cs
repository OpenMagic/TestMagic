using TestMagic.Imports.OpenMagic;

namespace TestMagic
{
    // todo: document
    public class GWT
    {
        // todo: document
        public static GivenAssertions<TGiven> Given<TGiven>(TGiven givenValue)
        {
            givenValue.MustNotBeNull("givenValue");

            return new GivenAssertions<TGiven>(givenValue);
        }
    }
}
