using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Utility
{
    /// <summary>
    /// Memoizer class.
    /// </summary>
    public static class Memoizer
    {
        /// <summary>
        /// Memoize the specified function.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <returns>Return result delegate.</returns>
        public static Func<TSource, TResult> Memoize<TSource, TResult>(Func<TSource, TResult> func)
        {
            var cache = new Dictionary<TSource, TResult>();

            return v =>
            {
                if (cache.TryGetValue(v, out TResult result))
                {
                    return result;
                }

                result = func(v);
                cache.Add(v, result);
                return result;
            };
        }
    }
}
