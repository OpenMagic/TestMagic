﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TestMagic.Imports.OpenMagic
{
    /// <summary>
    /// Collection of argument testing methods.
    /// </summary>
    /// <remarks>
    /// This has been copied from 
    /// </remarks>
    internal static class Argument
    {
        public static T MustBeGreaterThan<T>(this T param, T greaterThan, string paramName) where T : IComparable<T>
        {
            // todo: unit tests

            if (param.CompareTo(greaterThan) > 0)
            {
                return param;
            }

            throw new ArgumentOutOfRangeException(paramName, param, string.Format("Value must be greater than {0}.", greaterThan));
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> when <paramref name="param"/> is null.
        /// </summary>
        /// <typeparam name="T">The <paramref name="param"/> type.</typeparam>
        /// <param name="param">The value to test for null.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <returns>Returns <paramref name="param"/> when the value is not null.</returns>
        public static T MustNotBeNull<T>(this T param, string paramName)
        {
            if (param == null)
            {
                throw new ArgumentNullException(paramName);
            }

            return param;
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> when <paramref name="param"/> is null or <see cref="ArgumentException"/> when <paramref name="param"/> is empty.
        /// </summary>
        /// <typeparam name="T">The <paramref name="param"/> type.</typeparam>
        /// <param name="param">The value to test for null or empty.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <returns>Returns <paramref name="param"/> when the value is not null.</returns>
        public static IEnumerable<T> MustNotBeNullOrEmpty<T>(this IEnumerable<T> param, string paramName)
        {
            Argument.MustNotBeNull(param, paramName);

            if (!param.Any())
            {
                throw new ArgumentException("Value cannot be empty.", paramName);
            }

            return param;
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> when <paramref name="param"/> is null or <see cref="ArgumentException"/> when <paramref name="param"/> is whitespace.
        /// </summary>
        /// <typeparam name="T">The <paramref name="param"/> type.</typeparam>
        /// <param name="param">The value to test for null or whitespace.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        /// <returns>Returns <paramref name="param"/> when the value is not null or whitespace.</returns>
        public static string MustNotBeNullOrWhiteSpace(this string param, string paramName)
        {
            Argument.MustNotBeNull(param, paramName);

            if (string.IsNullOrWhiteSpace(param))
            {
                throw new ArgumentException("Value cannot be whitespace.", paramName);
            }

            return param;
        }
    }
}
