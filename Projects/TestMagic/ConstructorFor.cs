using System;
using System.Linq;

namespace TestMagic
{
    [Obsolete]
    internal class ConstructorFor<T> : Constructor
    {
        internal ConstructorFor()
            : base(typeof(T).GetConstructors().Single())
        {
        }
    }
}
