using System;
using System.Configuration;

namespace Chenyuan.Caching.Defaults
{
    internal sealed class RegexMatchTimeoutValidator : TimeSpanValidator
    {
        private static readonly TimeSpan _minValue = TimeSpan.Zero;
        private static readonly TimeSpan _maxValue = TimeSpan.FromMilliseconds(2147483646.0);
        public RegexMatchTimeoutValidator() : base(_minValue, _maxValue)
        {
        }
    }
}