﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Chenyuan.Extensions;
using Chenyuan.Infrastructure;

namespace Chenyuan.Plugins
{
	/// <summary>
	/// 
	/// </summary>
	public class PluginDescriptor : IComparable<PluginDescriptor>
	{
		private string _resourceRootKey;
		private string _brandImageFileName;

		/// <summary>
		/// 
		/// </summary>
		public PluginDescriptor()
		{
			this.Version = new Version("1.0");
			this.MinAppVersion = AppVersion.DefaultAppVersion.FullVersion;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="referencedAssembly"></param>
		/// <param name="originalAssemblyFile"></param>
		/// <param name="pluginType"></param>
		public PluginDescriptor(Assembly referencedAssembly, FileInfo originalAssemblyFile,
			Type pluginType)
			: this()
		{
			this.ReferencedAssembly = referencedAssembly;
			this.OriginalAssemblyFile = originalAssemblyFile;
			this.PluginType = pluginType;
		}

		/// <summary>
		/// Plugin type
		/// </summary>
		public string PluginFileName { get; set; }

		// codehint: sm-add
		/// <summary>
		/// The physical path of the runtime plugin
		/// </summary>
		public string PhysicalPath
		{
			get
			{
				return OriginalAssemblyFile.Directory.FullName;
			}
		}

		// codehint: sm-add
		/// <summary>
		/// Gets the file name of the brand image (without path)
		/// or an empty string if no image is specified
		/// </summary>
		public string BrandImageFileName
		{
			get
			{
				if (_brandImageFileName == null)
				{
					// "null" means we haven't checked yet!
					var filesToCheck = new string[] { "branding.png", "branding.gif", "branding.jpg", "branding.jpeg" };
					var dir = this.PhysicalPath;
					foreach (var file in filesToCheck)
					{
						if (File.Exists(Path.Combine(dir, file)))
						{
							_brandImageFileName = file;
							break;
						}
					}

					// indicate that we have checked already (although no file was found)
					if (_brandImageFileName == null)
						_brandImageFileName = String.Empty;
				}

				return _brandImageFileName;
			}
		}

		/// <summary>
		/// Plugin type
		/// </summary>
		public Type PluginType { get; set; }

		/// <summary>
		/// The assembly that has been shadow copied that is active in the application
		/// </summary>
		public Assembly ReferencedAssembly { get; internal set; }

		/// <summary>
		/// The original assembly file that a shadow copy was made from it
		/// </summary>
		public FileInfo OriginalAssemblyFile { get; internal set; }

		/// <summary>
		/// Gets or sets the plugin group
		/// </summary>
		/// <remarks>codehint:sm-add</remarks>
		public string Group { get; internal set; }

		// codehint: sm-add
		/// <summary>
		/// 
		/// </summary>
		public bool IsInKnownGroup
		{
			get
			{
				return PluginFileParser.KnownGroups.Contains(this.Group, StringComparer.OrdinalIgnoreCase);
			}
		}

		/// <summary>
		/// Gets or sets the friendly name
		/// </summary>
		public string FriendlyName { get; set; }

		/// <summary>
		/// Gets or sets the system name
		/// </summary>
		public string SystemName { get; set; }

		/// <summary>
		/// Gets the plugin description
		/// </summary>
		/// <remarks>codehint:sm-add</remarks>
		public string Description { get; internal set; }

		/// <summary>
		/// Gets or sets the version
		/// </summary>
		public Version Version { get; set; }

		/// <summary>
		/// Gets or sets the minimum supported app version
		/// </summary>
		public Version MinAppVersion { get; set; }

		/// <summary>
		/// Gets or sets the author
		/// </summary>
		public string Author { get; set; }

		/// <summary>
		/// Gets or sets the display order
		/// </summary>
		public int DisplayOrder { get; set; }

		/// <summary>
		/// Gets or sets the value indicating whether plugin is installed
		/// </summary>
		public bool Installed { get; set; }

		/// <summary>
		/// Gets or sets the root key of string resources.
		/// </summary>
		/// <remarks>Tries to get it from first entry of resource XML file if not specified. In that case the first resource name should not contain a dot if it's not part of the root key.
		/// Otherwise you get the wrong root key.</remarks>
		/// <remarks>codehint: sm-add</remarks>
		public string ResourceRootKey
		{
			get
			{
				if (_resourceRootKey == null)
				{
					_resourceRootKey = "";

					try
					{
						// try to get root-key from first entry of XML file
						string localizationDir = Path.Combine(OriginalAssemblyFile.Directory.FullName, "Localization");

						if (System.IO.Directory.Exists(localizationDir))
						{
							string filePath = System.IO.Directory.EnumerateFiles(localizationDir, "*.xml").FirstOrDefault();
							if (filePath.HasValue())
							{
								XmlDocument doc = new XmlDocument();
								doc.Load(filePath);
								var node = doc.SelectSingleNode(@"//Language/LocaleResource");
								if (node != null)
								{
									string key = node.Attributes["Name"].InnerText;
									if (key.HasValue() && key.Contains('.'))
										_resourceRootKey = key.Substring(0, key.LastIndexOf('.'));
								}
							}
						}
					}
					catch (Exception exc)
					{
						exc.Dump();
					}
				}
				return _resourceRootKey;
			}
			set
			{
				_resourceRootKey = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T Instance<T>() where T : class, IPlugin
		{
			object instance;
			if (!EngineContext.Current.ContainerManager.TryResolve(PluginType, out instance))
			{
				//not resolved
				instance = EngineContext.Current.ContainerManager.ResolveUnregistered(PluginType);
			}
			var typedInstance = instance as T;
			if (typedInstance != null)
				typedInstance.PluginDescriptor = this;
			return typedInstance;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IPlugin Instance()
		{
			return Instance<IPlugin>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public int CompareTo(PluginDescriptor other)
		{
			if (DisplayOrder != other.DisplayOrder)
				return DisplayOrder.CompareTo(other.DisplayOrder);
			else
				return FriendlyName.CompareTo(other.FriendlyName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public string GetSettingKey(string name)
		{
			return "PluginSetting.{0}.{1}".FormatWith(SystemName, name);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return FriendlyName;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			var other = obj as PluginDescriptor;
			return other != null &&
				SystemName != null &&
				SystemName.Equals(other.SystemName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return SystemName.GetHashCode();
		}
	}
}
