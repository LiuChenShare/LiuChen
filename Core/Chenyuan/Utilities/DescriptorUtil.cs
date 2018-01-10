using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;
using Chenyuan.Components;

namespace Chenyuan.Utilities
{
    public static class DescriptorUtil
	{
		private static void AppendPartToUniqueIdBuilder(StringBuilder builder, object part)
		{
			if (part == null)
			{
				builder.Append("[-1]");
				return;
			}
			string text = Convert.ToString(part, CultureInfo.InvariantCulture);
			builder.AppendFormat("[{0}]{1}", text.Length, text);
		}

		public static void AppendUniqueId(StringBuilder builder, object part)
		{
			MemberInfo memberInfo = part as MemberInfo;
			if (memberInfo != null)
			{
                AppendPartToUniqueIdBuilder(builder, memberInfo.Module.ModuleVersionId);
                AppendPartToUniqueIdBuilder(builder, memberInfo.MetadataToken);
				return;
			}
			IUniquelyIdentifiable uniquelyIdentifiable = part as IUniquelyIdentifiable;
			if (uniquelyIdentifiable != null)
			{
                AppendPartToUniqueIdBuilder(builder, uniquelyIdentifiable.UniqueId);
				return;
			}
            AppendPartToUniqueIdBuilder(builder, part);
		}

		public static string CreateUniqueId(object part0, object part1)
		{
			StringBuilder stringBuilder = new StringBuilder();
            AppendUniqueId(stringBuilder, part0);
            AppendUniqueId(stringBuilder, part1);
			return stringBuilder.ToString();
		}

		public static string CreateUniqueId(object part0, object part1, object part2)
		{
			StringBuilder stringBuilder = new StringBuilder();
            AppendUniqueId(stringBuilder, part0);
            AppendUniqueId(stringBuilder, part1);
            AppendUniqueId(stringBuilder, part2);
			return stringBuilder.ToString();
		}

		public static TDescriptor[] LazilyFetchOrCreateDescriptors<TReflection, TDescriptor, TArgument>(ref TDescriptor[] cacheLocation, Func<TArgument, TReflection[]> initializer, Func<TReflection, TArgument, TDescriptor> converter, TArgument state)
		{
			TDescriptor[] descriptorArray = Interlocked.CompareExchange(ref cacheLocation, null, null);
			if (descriptorArray != null)
			{
				return descriptorArray;
			}
			TReflection[] reflectionArray = initializer(state);
			List<TDescriptor> list = new List<TDescriptor>(reflectionArray.Length);
			for (int i = 0; i < reflectionArray.Length; i++)
			{
				TDescriptor tDescriptor = converter(reflectionArray[i], state);
				if (tDescriptor != null)
				{
					list.Add(tDescriptor);
				}
			}
			TDescriptor[] array3 = list.ToArray();
			TDescriptor[] array4 = Interlocked.CompareExchange(ref cacheLocation, array3, null);
			return array4 ?? array3;
		}
	}
}
