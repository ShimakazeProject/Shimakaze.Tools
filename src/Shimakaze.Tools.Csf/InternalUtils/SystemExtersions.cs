using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shimakaze.Tools.Csf.InternalUtils
{
    internal static class SystemExtersions
    {
        public static void Each<T>(this IEnumerable<T> ts, Action<T> action)
        {
            foreach (var item in ts)
                action(item);
        }
        /// <summary>
        /// ForEach Collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="action">Break the loop when this delegate result is false</param>
        public static void Each<T>(this IEnumerable<T> ts, Func<T, bool> action)
        {
            foreach (var item in ts)
                if (!action(item))
                    break;
        }
        public static async Task EachAsync<T>(this IEnumerable<T> ts, Func<T, Task> action)
        {
            foreach (var item in ts)
                await action(item);
        }
        /// <summary>
        /// ForEach Collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="action">Break the loop when this delegate result is false</param>
        public static async Task EachAsync<T>(this IEnumerable<T> ts, Func<T, Task<bool>> action)
        {
            foreach (var item in ts)
                if (!await action(item))
                    break;
        }
    }
}