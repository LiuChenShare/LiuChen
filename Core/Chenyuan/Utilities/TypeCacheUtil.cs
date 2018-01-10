using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Chenyuan.Components;

namespace Chenyuan.Utilities
{
    public static class TypeCacheUtil
	{
		private static IEnumerable<Type> FilterTypesInAssemblies(IBuildManager buildManager, Predicate<Type> predicate)
		{
			IEnumerable<Type> enumerable = Type.EmptyTypes;
			ICollection referencedAssemblies = buildManager.GetReferencedAssemblies();
			foreach (Assembly assembly in referencedAssemblies)
			{
				Type[] types;
				try
				{
					types = assembly.GetTypes();
				}
				catch (ReflectionTypeLoadException ex)
				{
					types = ex.Types;
				}
				enumerable = enumerable.Concat(types);
			}
			return 
				from type in enumerable
				where TypeIsPublicClass(type) && predicate(type)
				select type;
		}
		public static List<Type> GetFilteredTypesFromAssemblies(string cacheName, Predicate<Type> predicate, IBuildManager buildManager)
		{
			TypeCacheSerializer serializer = new TypeCacheSerializer();
			List<Type> list = ReadTypesFromCache(cacheName, predicate, buildManager, serializer);
			if (list != null)
			{
				return list;
			}
			list = FilterTypesInAssemblies(buildManager, predicate).ToList();
            SaveTypesToCache(cacheName, list, buildManager, serializer);
			return list;
		}
		internal static List<Type> ReadTypesFromCache(string cacheName, Predicate<Type> predicate, IBuildManager buildManager, TypeCacheSerializer serializer)
		{
			try
			{
				Stream stream = buildManager.ReadCachedFile(cacheName);
				if (stream != null)
				{
					using (StreamReader streamReader = new StreamReader(stream))
					{
						List<Type> list = serializer.DeserializeTypes(streamReader);
						if (list != null)
						{
							if (list.All((Type type) => TypeIsPublicClass(type) && predicate(type)))
							{
								return list;
							}
						}
					}
				}
			}
			catch
			{
			}
			return null;
		}
		internal static void SaveTypesToCache(string cacheName, IList<Type> matchingTypes, IBuildManager buildManager, TypeCacheSerializer serializer)
		{
			try
			{
				Stream stream = buildManager.CreateCachedFile(cacheName);
				if (stream != null)
				{
					using (StreamWriter streamWriter = new StreamWriter(stream))
					{
						serializer.SerializeTypes(matchingTypes, streamWriter);
					}
				}
			}
			catch
			{
			}
		}
		private static bool TypeIsPublicClass(Type type)
		{
			return type != null && type.IsPublic && type.IsClass && !type.IsAbstract;
		}
	}
}
