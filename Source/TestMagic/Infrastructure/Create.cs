using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FakeItEasy;
using FakeItEasy.Core;

namespace TestMagic.Infrastructure
{
    // todo: document
    // todo: unit tests
    // todo: pull request for FakeItEasy
    [Obsolete]
    public static partial class Create
    {
        public static object Dummy(Type type)
        {
            var dummyMethod = typeof(A).GetMethod("Dummy");
            var dummyGenericMethod = dummyMethod.MakeGenericMethod(new Type[] { type });

            try
            {
                var value = dummyGenericMethod.Invoke(null, null);

                return value;
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException.GetType() != typeof(FakeCreationException))
                {
                    throw;
                }

                throw new Exception(string.Format("Unable to create fake value for {0}. Consider creating a DummyDefinition. See https://github.com/FakeItEasy/FakeItEasy/wiki/Dummies.", type));
            }
        }
    }
}
