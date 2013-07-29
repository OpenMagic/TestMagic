using TestMagic.Imports.OpenMagic;

namespace TestMagic
{
    public class GWT
    {
        public static GivenAssertions<TGiven> Given<TGiven>(TGiven givenValue)
        {
            givenValue.MustNotBeNull("givenValue");

            return new GivenAssertions<TGiven>(givenValue);
        }
    }
}
