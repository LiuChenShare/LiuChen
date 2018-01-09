using System;
using System.Globalization;

namespace Chenyuan.Components
{
    public class SingleServiceResolver<TService> : IResolver<TService> where TService : class
	{
		private Lazy<TService> _currentValueFromResolver;
		private Func<TService> _currentValueThunk;
		private TService _defaultValue;
		private Func<IDependencyResolver> _resolverThunk;
		private string _callerMethodName;
		public TService Current
		{
			get
			{
				TService service;
				if ((service = _currentValueFromResolver.Value) == null && (service = _currentValueThunk()) == null)
				{
					service = _defaultValue;
				}
				return service;
			}
		}

		public SingleServiceResolver(Func<TService> currentValueThunk, TService defaultValue, string callerMethodName)
		{
			if (currentValueThunk == null)
			{
				throw new ArgumentNullException(nameof(currentValueThunk));
			}
			if (defaultValue == null)
			{
				throw new ArgumentNullException(nameof(defaultValue));
			}
			_resolverThunk = (() => DependencyResolver.Current);
			_currentValueFromResolver = new Lazy<TService>(new Func<TService>(this.GetValueFromResolver));
			_currentValueThunk = currentValueThunk;
			_defaultValue = defaultValue;
			_callerMethodName = callerMethodName;
		}
		internal SingleServiceResolver(Func<TService> staticAccessor, TService defaultValue, IDependencyResolver resolver, string callerMethodName) : this(staticAccessor, defaultValue, callerMethodName)
		{
			if (resolver != null)
			{
				_resolverThunk = (() => resolver);
			}
		}
		private TService GetValueFromResolver()
		{
			TService service = _resolverThunk().GetService<TService>();
			if (service != null && _currentValueThunk() != null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resource.SingleServiceResolver_CannotRegisterTwoInstances, new object[]
				{
					typeof(TService).Name.ToString(),
					_callerMethodName
				}));
			}
			return service;
		}
	}
}
