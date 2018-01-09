using Chenyuan.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Infrastructure
{
    internal static class Misc
    {

        public static bool TryAction<T>(Func<T> func, out T output)
        {
            Assert.NotNull(func, nameof(func));

            try
            {
                output = func();
                return true;
            }
            catch
            {
                output = default(T);
                return false;
            }
        }

        /// <summary>
        /// Perform an action if the string is not null or empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="action">The action to perform.</param>
        public static void IfNotNullOrEmpty(string value, Action<string> action)
        {
            IfNotNullOrEmpty(value, action, null);
        }

        private static void IfNotNullOrEmpty(string value, Action<string> trueAction, Action<string> falseAction)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (trueAction != null)
                    trueAction(value);
            }
            else
            {
                if (falseAction != null)
                    falseAction(value);
            }
        }

    }
}
