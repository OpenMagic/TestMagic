using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FakeItEasy;
using TestMagic.Infrastructure;

namespace TestMagic
{
    // todo: document
    // todo: unit tests
    [Obsolete]
    internal class Constructor
    {
        private ConstructorInfo ConstructorInfo;
        private IEnumerable<ParameterInfo> Parameters;

        internal Constructor(ConstructorInfo constructorInfo)
        {
            this.ConstructorInfo = constructorInfo;
            this.Parameters = constructorInfo.GetParameters();
        }

        internal void ShouldThrowArgumentNullExceptionForAllParameters()
        {
            foreach (var parameter in this.Parameters)
            {
                this.ShouldThrowArgumentNullException(forParameter: parameter.Name);
            }
        }

        internal Constructor ShouldThrowArgumentNullException(string forParameter)
        {
            return this.ShouldThrowArgumentNullException(this.GetParameter(forParameter));
        }

        internal Constructor ShouldThrowArgumentNullException(ParameterInfo forParameter)
        {
            return this.ShouldThrowArgumentNullException(forParameter, this.GetParameterValues(forParameter));
        }

        internal Constructor ShouldThrowArgumentNullException(ParameterInfo forParameter, IEnumerable<object> parameters)
        {
            try
            {
                this.ConstructorInfo.Invoke(parameters.ToArray());
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException.GetType() != typeof(ArgumentNullException))
                {
                    throw new Exception(
                        string.Format(
                            "Expected {0} constructor to throw ArgumentNullException for {1} but {2} was thrown.",
                            this.ConstructorInfo.DeclaringType.FullName,
                            forParameter.Name,
                            ex.InnerException.GetType().FullName
                        )
                    );
                }

                var argumentNullException = (ArgumentNullException)ex.InnerException;

                if (argumentNullException.ParamName == forParameter.Name)
                {
                    return this;
                }

                throw new Exception(
                    string.Format(
                        "Expected {0} constructor to throw ArgumentNullException for {1} but it threw ArgumentNullException for {2}.",
                        this.ConstructorInfo.DeclaringType.FullName,
                        forParameter.Name,
                        argumentNullException.ParamName
                    )
                );
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format(
                        "Expected {0} constructor to throw ArgumentNullException for {1} but {2} was thrown.",
                        this.ConstructorInfo.DeclaringType.FullName,
                        forParameter.Name,
                        ex.GetType().FullName
                    ),
                    ex
                );
            }

            throw new Exception(
                string.Format(
                    "Expected {0} constructor to throw ArgumentNullException for {1} but no exception was thrown.",
                    this.ConstructorInfo.DeclaringType.FullName,
                    forParameter.Name
                )
            );
        }

        private ParameterInfo GetParameter(string parameterName)
        {
            var parameter = this.Parameters.SingleOrDefault(p => p.Name == parameterName);

            if (parameter != null)
            {
                return parameter;
            }

            throw new ArgumentException(string.Format("Cannot find '{0}' parameter.", parameterName));
        }

        private IEnumerable<object> GetParameterValues(ParameterInfo forParameter)
        {
            var parameterValues = new List<object>();

            foreach (var parameter in this.Parameters)
            {
                if (parameter.Equals(forParameter))
                {
                    parameterValues.Add(null);
                }
                else
                {
                    parameterValues.Add(Create.Dummy(parameter.ParameterType));
                }
            }

            return parameterValues;
        }
    }
}
