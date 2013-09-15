using System.Linq;

namespace TestMagic
{
    internal class ConstructorFor<T> : Constructor
    {
        internal ConstructorFor()
            : base(typeof(T).GetConstructors().Single())
        {
        }
    }
}
