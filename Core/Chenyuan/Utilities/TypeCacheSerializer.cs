using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Chenyuan.Utilities
{
    internal sealed class TypeCacheSerializer
	{
		private static readonly Guid _mvcVersionId = typeof(TypeCacheSerializer).Module.ModuleVersionId;
		private DateTime CurrentDate
		{
			get
			{
				DateTime? currentDateOverride = this.CurrentDateOverride;
				if (!currentDateOverride.HasValue)
				{
					return DateTime.Now;
				}
				return currentDateOverride.GetValueOrDefault();
			}
		}
		internal DateTime? CurrentDateOverride
		{
			get;
			set;
		}
		public List<Type> DeserializeTypes(TextReader input)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(input);
			XmlElement documentElement = xmlDocument.DocumentElement;
			Guid a = new Guid(documentElement.Attributes["mvcVersionId"].Value);
			if (a != TypeCacheSerializer._mvcVersionId)
			{
				return null;
			}
			List<Type> list = new List<Type>();
			foreach (XmlNode xmlNode in documentElement.ChildNodes)
			{
				string value = xmlNode.Attributes["name"].Value;
				Assembly assembly = Assembly.Load(value);
				foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
				{
					Guid b = new Guid(xmlNode2.Attributes["versionId"].Value);
					foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
					{
						string innerText = xmlNode3.InnerText;
						Type type = assembly.GetType(innerText);
						if (type == null || type.Module.ModuleVersionId != b)
						{
							return null;
						}
						list.Add(type);
					}
				}
			}
			return list;
		}
		public void SerializeTypes(IEnumerable<Type> types, TextWriter output)
		{
			IEnumerable<IGrouping<Assembly, IGrouping<Module, Type>>> enumerable = 
				from type in types
				group type by type.Module into groupedByModule
				group groupedByModule by groupedByModule.Key.Assembly;
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.AppendChild(xmlDocument.CreateComment(Resource.TypeCache_DoNotModify));
			XmlElement xmlElement = xmlDocument.CreateElement("typeCache");
			xmlDocument.AppendChild(xmlElement);
			xmlElement.SetAttribute("lastModified", this.CurrentDate.ToString());
			xmlElement.SetAttribute("mvcVersionId", TypeCacheSerializer._mvcVersionId.ToString());
			foreach (IGrouping<Assembly, IGrouping<Module, Type>> current in enumerable)
			{
				XmlElement xmlElement2 = xmlDocument.CreateElement("assembly");
				xmlElement.AppendChild(xmlElement2);
				xmlElement2.SetAttribute("name", current.Key.FullName);
				foreach (IGrouping<Module, Type> current2 in current)
				{
					XmlElement xmlElement3 = xmlDocument.CreateElement("module");
					xmlElement2.AppendChild(xmlElement3);
					xmlElement3.SetAttribute("versionId", current2.Key.ModuleVersionId.ToString());
					foreach (Type current3 in current2)
					{
						XmlElement xmlElement4 = xmlDocument.CreateElement("type");
						xmlElement3.AppendChild(xmlElement4);
						xmlElement4.AppendChild(xmlDocument.CreateTextNode(current3.FullName));
					}
				}
			}
			xmlDocument.Save(output);
		}
	}
}
