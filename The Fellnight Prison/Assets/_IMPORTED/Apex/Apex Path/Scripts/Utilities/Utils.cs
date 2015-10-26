/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.Utilities
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// General utilities of various sorts
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Copies the public properties from one class instance to another. Only properties present on both types will be copied (obviously).
        /// </summary>
        /// <param name="from">The instance to copy from.</param>
        /// <param name="to">The instance to copy to.</param>
        public static void CopyProps(object from, object to)
        {
            Ensure.ArgumentNotNull(from, "from");
            Ensure.ArgumentNotNull(to, "to");
#if NETFX_CORE
            var fromType = from.GetType().GetTypeInfo();
            var toType = to.GetType().GetTypeInfo();

            //Process properties
            var fromProps = from p in fromType.DeclaredProperties
                        where p.CanRead
                        select p;

            var toProps = (from p in toType.DeclaredProperties
                        where p.CanWrite
                        select p).ToDictionary(p => p.Name);
#else
            var fromType = from.GetType();
            var toType = to.GetType();

            //Process properties
            var fromProps = from p in fromType.GetProperties()
                            where p.CanRead
                            select p;

            var toProps = (from p in toType.GetProperties()
                           where p.CanWrite
                           select p).ToDictionary(p => p.Name);
#endif
            foreach (var p in fromProps)
            {
                PropertyInfo toProp;
                if (toProps.TryGetValue(p.Name, out toProp))
                {
                    toProp.SetValue(to, p.GetValue(from, null), null);
                }
            }
        }

        /// <summary>
        /// Inserts spaces between each token in a Pascal cased string
        /// </summary>
        /// <param name="pascalString">The string to parse</param>
        /// <returns>The converted string</returns>
        public static string ExpandFromPascal(this string pascalString)
        {
            if (pascalString == null)
            {
                return null;
            }

            var transformer = new StringBuilder();

            if (pascalString.Length > 0)
            {
                transformer.Append(char.ToUpper(pascalString[0]));
                for (int i = 1; i < pascalString.Length; i++)
                {
                    if (char.IsUpper(pascalString, i))
                    {
                        transformer.Append(" ");
                    }

                    transformer.Append(pascalString[i]);
                }
            }

            return transformer.ToString();
        }

        /// <summary>
        /// Fuses the specified arrays.
        /// </summary>
        /// <typeparam name="T">The type of the arrays</typeparam>
        /// <param name="arrOne">The first array.</param>
        /// <param name="arrTwo">The second array.</param>
        /// <returns>An array containing all elements of the two source arrays in their origianl order. If either array is null the other is returned.</returns>
        public static T[] Fuse<T>(T[] arrOne, T[] arrTwo)
        {
            if (arrOne == null)
            {
                return arrTwo;
            }

            if (arrTwo == null)
            {
                return arrOne;
            }

            var newArr = new T[arrOne.Length + arrTwo.Length];
            Array.Copy(arrOne, newArr, arrOne.Length);
            Array.Copy(arrTwo, 0, newArr, arrOne.Length, arrTwo.Length);

            return newArr;
        }
    }
}
