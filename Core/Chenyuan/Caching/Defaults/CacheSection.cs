using System;
using System.ComponentModel;
using System.Configuration;

namespace Chenyuan.Caching.Defaults
{
    public sealed class CacheSection : ConfigurationSection
    {
        internal static TimeSpan DefaultPrivateBytesPollTime;
        private static ConfigurationPropertyCollection _properties;
        private static readonly ConfigurationProperty _propDisableMemoryCollection;
        private static readonly ConfigurationProperty _propDisableExpiration;
        private static readonly ConfigurationProperty _propPrivateBytesLimit;
        private static readonly ConfigurationProperty _propPercentagePhysicalMemoryUsedLimit;
        private static readonly ConfigurationProperty _propPrivateBytesPollTime;

        [ConfigurationProperty("disableMemoryCollection", DefaultValue = false)]
        public bool DisableMemoryCollection
        {
            get
            {
                return (bool)base[_propDisableMemoryCollection];
            }
            set
            {
                base[_propDisableMemoryCollection] = value;
            }
        }
        [ConfigurationProperty("disableExpiration", DefaultValue = false)]
        public bool DisableExpiration
        {
            get
            {
                return (bool)base[_propDisableExpiration];
            }
            set
            {
                base[_propDisableExpiration] = value;
            }
        }
        [ConfigurationProperty("privateBytesLimit", DefaultValue = 0L), LongValidator(MinValue = 0L)]
        public long PrivateBytesLimit
        {
            get
            {
                return (long)base[_propPrivateBytesLimit];
            }
            set
            {
                base[_propPrivateBytesLimit] = value;
            }
        }

        [ConfigurationProperty("percentagePhysicalMemoryUsedLimit", DefaultValue = 0), IntegerValidator(MinValue = 0, MaxValue = 100)]
        public int PercentagePhysicalMemoryUsedLimit
        {
            get
            {
                return (int)base[_propPercentagePhysicalMemoryUsedLimit];
            }
            set
            {
                base[_propPercentagePhysicalMemoryUsedLimit] = value;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _properties;
            }
        }

        [TypeConverter(typeof(InfiniteTimeSpanConverter)), ConfigurationProperty("privateBytesPollTime", DefaultValue = "00:02:00")]
        public TimeSpan PrivateBytesPollTime
        {
            get
            {
                return (TimeSpan)base[_propPrivateBytesPollTime];
            }
            set
            {
                base[_propPrivateBytesPollTime] = value;
            }
        }
        static CacheSection()
        {
            DefaultPrivateBytesPollTime = new TimeSpan(0, 2, 0);
            _propDisableMemoryCollection = new ConfigurationProperty("disableMemoryCollection", typeof(bool), false, ConfigurationPropertyOptions.None);
            _propDisableExpiration = new ConfigurationProperty("disableExpiration", typeof(bool), false, ConfigurationPropertyOptions.None);
            _propPrivateBytesLimit = new ConfigurationProperty("privateBytesLimit", typeof(long), 0L, null, new LongValidator(0L, 9223372036854775807L), ConfigurationPropertyOptions.None);
            _propPercentagePhysicalMemoryUsedLimit = new ConfigurationProperty("percentagePhysicalMemoryUsedLimit", typeof(int), 0, null, new IntegerValidator(0, 100), ConfigurationPropertyOptions.None);
            _propPrivateBytesPollTime = new ConfigurationProperty("privateBytesPollTime", typeof(TimeSpan), DefaultPrivateBytesPollTime, StdValidatorsAndConverters.InfiniteTimeSpanConverter, StdValidatorsAndConverters.PositiveTimeSpanValidator, ConfigurationPropertyOptions.None);
            _properties = new ConfigurationPropertyCollection();
            _properties.Add(_propDisableMemoryCollection);
            _properties.Add(_propDisableExpiration);
            _properties.Add(_propPrivateBytesLimit);
            _properties.Add(_propPercentagePhysicalMemoryUsedLimit);
            _properties.Add(_propPrivateBytesPollTime);
        }
    }
}
