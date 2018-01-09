using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Chenyuan.Extensions;
using Chenyuan.Infrastructure;

namespace Chenyuan.ComponentModel
{
	/// <summary>
	/// 
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public class ResourceDisplayNameAttribute : DisplayNameAttribute, IModelAttribute, IResourceDisplayAttribute
	{
		/// <summary>
		/// 
		/// </summary>
		public const string c_IdentityName = "KeyedResourceDisplayName";
		private readonly string _callerPropertyName;
		private readonly object s_dataLoaderLocker = new object();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="resourceKey"></param>
		/// <param name="propertyName"></param>
		public ResourceDisplayNameAttribute(string resourceKey, [CallerMemberName] string propertyName = null)
			: base(resourceKey)
		{
			ResourceKey = resourceKey;
			_callerPropertyName = propertyName;
		}

		/// <summary>
		/// 
		/// </summary>
		public string ResourceKey { get; set; }

		private string _displayName = null;
		private bool _displayNameLoaded = false;

		/// <summary>
		/// 
		/// </summary>
		public override string DisplayName
		{
			get
			{
				if (!_displayNameLoaded)
				{
					lock (s_dataLoaderLocker)
					{
						_displayNameLoaded = true;
						string value = null;
						try
						{
							value = EngineContext.Current.Resolve<IResourceService>().GetResource(ResourceKey);
						}
						catch { }
						if (value.IsEmpty() && _callerPropertyName.HasValue())
						{
							value = _callerPropertyName.SplitPascalCase();
						}
						if (value.IsEmpty())
						{
							value = base.DisplayName;
						}
						_displayName = value;
					}
				}
				return _displayName;
			}
		}

		private string _hint = null;
		private bool _hintLoaded = false;

		/// <summary>
		/// 
		/// </summary>
		public virtual string Hint
		{
			get
			{
				if (!_hintLoaded)
				{
					lock (s_dataLoaderLocker)
					{
						_hintLoaded = true;
						_hint = EngineContext.Current.Resolve<IResourceService>().GetResource(ResourceKey + ".Hint");
					}
				}
				return _hint;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			get { return c_IdentityName; }
		}
	}
}
