using System.ComponentModel;
using System.Configuration;

namespace Chenyuan.Caching.Defaults
{
    internal static class StdValidatorsAndConverters
    {
        private static TypeConverter s_infiniteTimeSpanConverter;
        private static TypeConverter s_timeSpanMinutesConverter;
        private static TypeConverter s_timeSpanMinutesOrInfiniteConverter;
        private static TypeConverter s_timeSpanSecondsConverter;
        private static TypeConverter s_timeSpanSecondsOrInfiniteConverter;
        private static TypeConverter s_whiteSpaceTrimStringConverter;
        private static TypeConverter s_versionConverter;
        private static ConfigurationValidatorBase s_regexMatchTimeoutValidator;
        private static ConfigurationValidatorBase s_positiveTimeSpanValidator;
        private static ConfigurationValidatorBase s_nonEmptyStringValidator;
        private static ConfigurationValidatorBase s_nonZeroPositiveIntegerValidator;
        private static ConfigurationValidatorBase s_positiveIntegerValidator;
        internal static TypeConverter InfiniteTimeSpanConverter
        {
            get
            {
                if (s_infiniteTimeSpanConverter == null)
                {
                    s_infiniteTimeSpanConverter = new InfiniteTimeSpanConverter();
                }
                return s_infiniteTimeSpanConverter;
            }
        }
        internal static TypeConverter TimeSpanMinutesConverter
        {
            get
            {
                if (s_timeSpanMinutesConverter == null)
                {
                    s_timeSpanMinutesConverter = new TimeSpanMinutesConverter();
                }
                return s_timeSpanMinutesConverter;
            }
        }
        internal static TypeConverter TimeSpanMinutesOrInfiniteConverter
        {
            get
            {
                if (s_timeSpanMinutesOrInfiniteConverter == null)
                {
                    s_timeSpanMinutesOrInfiniteConverter = new TimeSpanMinutesOrInfiniteConverter();
                }
                return s_timeSpanMinutesOrInfiniteConverter;
            }
        }
        internal static TypeConverter TimeSpanSecondsConverter
        {
            get
            {
                if (s_timeSpanSecondsConverter == null)
                {
                    s_timeSpanSecondsConverter = new TimeSpanSecondsConverter();
                }
                return s_timeSpanSecondsConverter;
            }
        }
        internal static TypeConverter TimeSpanSecondsOrInfiniteConverter
        {
            get
            {
                if (s_timeSpanSecondsOrInfiniteConverter == null)
                {
                    s_timeSpanSecondsOrInfiniteConverter = new TimeSpanSecondsOrInfiniteConverter();
                }
                return s_timeSpanSecondsOrInfiniteConverter;
            }
        }
        internal static TypeConverter WhiteSpaceTrimStringConverter
        {
            get
            {
                if (s_whiteSpaceTrimStringConverter == null)
                {
                    s_whiteSpaceTrimStringConverter = new WhiteSpaceTrimStringConverter();
                }
                return s_whiteSpaceTrimStringConverter;
            }
        }
        internal static TypeConverter VersionConverter
        {
            get
            {
                if (s_versionConverter == null)
                {
                    s_versionConverter = new VersionConverter();
                }
                return s_versionConverter;
            }
        }
        internal static ConfigurationValidatorBase RegexMatchTimeoutValidator
        {
            get
            {
                if (s_regexMatchTimeoutValidator == null)
                {
                    s_regexMatchTimeoutValidator = new RegexMatchTimeoutValidator();
                }
                return s_regexMatchTimeoutValidator;
            }
        }
        internal static ConfigurationValidatorBase PositiveTimeSpanValidator
        {
            get
            {
                if (s_positiveTimeSpanValidator == null)
                {
                    s_positiveTimeSpanValidator = new PositiveTimeSpanValidator();
                }
                return s_positiveTimeSpanValidator;
            }
        }
        internal static ConfigurationValidatorBase NonEmptyStringValidator
        {
            get
            {
                if (s_nonEmptyStringValidator == null)
                {
                    s_nonEmptyStringValidator = new StringValidator(1);
                }
                return s_nonEmptyStringValidator;
            }
        }
        internal static ConfigurationValidatorBase NonZeroPositiveIntegerValidator
        {
            get
            {
                if (s_nonZeroPositiveIntegerValidator == null)
                {
                    s_nonZeroPositiveIntegerValidator = new IntegerValidator(1, 2147483647);
                }
                return s_nonZeroPositiveIntegerValidator;
            }
        }
        internal static ConfigurationValidatorBase PositiveIntegerValidator
        {
            get
            {
                if (s_positiveIntegerValidator == null)
                {
                    s_positiveIntegerValidator = new IntegerValidator(0, 2147483647);
                }
                return s_positiveIntegerValidator;
            }
        }
    }
}
