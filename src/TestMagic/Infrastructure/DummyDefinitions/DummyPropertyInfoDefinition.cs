using System;
using System.Reflection;
using FakeItEasy;

namespace TestMagic.Infrastructure.DummyDefinitions
{
    public class DummyPropertyInfoDefinition : DummyDefinition<PropertyInfo>
    {
        protected override PropertyInfo CreateDummy()
        {
            return typeof(Exception).GetProperty("Message");
        }
    }
}
