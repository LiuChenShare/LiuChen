using Chenyuan.Extensions;
using Chenyuan.Fasterflect.Extensions.Core;
using Chenyuan.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public static class DictionaryConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public static bool CanCreateType(Type itemType)
        {
            return itemType.IsClass && itemType.GetConstructor(Type.EmptyTypes) != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="problems"></param>
        /// <returns></returns>
        public static T CreateAndPopulate<T>(IDictionary<string, object> source, out ICollection<ConvertProblem> problems)
            where T : class, new()
        {
            return (T)CreateAndPopulate(typeof(T), source, out problems);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="source"></param>
        /// <param name="problems"></param>
        /// <returns></returns>
        public static object CreateAndPopulate(Type targetType, IDictionary<string, object> source, out ICollection<ConvertProblem> problems)
        {
            Assert.NotNull(targetType, nameof(targetType));

            var target = targetType.CreateInstance(); //Activator.CreateInstance(targetType);

            Populate(source, target, out problems);

            return target;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static object SafeCreateAndPopulate(Type targetType, IDictionary<string, object> source)
        {
            ICollection<ConvertProblem> problems;
            var item = CreateAndPopulate(targetType, source, out problems);

            if (problems.Count > 0)
                throw new DictionaryConvertException(problems);

            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T SafeCreateAndPopulate<T>(IDictionary<string, object> source)
            where T : class, new()
        {
            return (T)SafeCreateAndPopulate(typeof(T), source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="populated"></param>
        public static void Populate(IDictionary<string, object> source, object target, params object[] populated)
        {
            ICollection<ConvertProblem> problems;

            Populate(source, target, out problems, populated);

            if (problems.Count > 0)
                throw new DictionaryConvertException(problems);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="problems"></param>
        /// <param name="populated"></param>
        public static void Populate(IDictionary<string, object> source, object target, out ICollection<ConvertProblem> problems, params object[] populated)
        {
            Assert.NotNull(source, nameof(source));
            Assert.NotNull(target, nameof(target));

            problems = new List<ConvertProblem>();

            if (populated.Any(x => x == target))
                return;

            Type t = target.GetType();

            if (source != null)
            {
                foreach (var pi in t.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {

                    object value;

                    if (!pi.PropertyType.IsPredefinedSimpleType() && source.TryGetValue(pi.Name, out value) && value is IDictionary<string, object>)
                    {
                        var nestedValue = target.GetPropertyValue(pi.Name);
                        ICollection<ConvertProblem> nestedProblems;

                        populated = populated.Concat(new object[] { target }).ToArray();
                        Populate((IDictionary<string, object>)value, nestedValue, out nestedProblems, populated);

                        if (nestedProblems != null && nestedProblems.Any())
                        {
                            problems.AddRange(nestedProblems);
                        }
                        WriteToProperty(target, pi, nestedValue, problems);
                    }
                    else if (pi.PropertyType.IsArray && !source.ContainsKey(pi.Name))
                    {
                        WriteToProperty(target, pi, RetrieveArrayValues(pi, source, problems), problems);
                    }
                    else
                    {
                        if (source.TryGetValue(pi.Name, out value))
                        {
                            WriteToProperty(target, pi, value, problems);
                        }
                    }
                }
            }
        }

        private static object RetrieveArrayValues(PropertyInfo arrayProp, IDictionary<string, object> source, ICollection<ConvertProblem> problems)
        {
            Type elemType = arrayProp.PropertyType.GetElementType();
            bool anyValuesFound = true;
            int idx = 0;

            var elements = (IList)typeof(List<>).MakeGenericType(elemType).CreateInstance();

            var properties = elemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            while (anyValuesFound)
            {
                object curElement = null;
                anyValuesFound = false;

                foreach (var pi in properties)
                {
                    var key = $"{arrayProp.Name}[{idx}].{pi.Name}";
                    object value;

                    if (source.TryGetValue(key, out value))
                    {
                        anyValuesFound = true;

                        if (curElement == null)
                        {
                            curElement = elemType.CreateInstance();
                            elements.Add(curElement);
                        }

                        SetPropFromValue(value, curElement, pi, problems);
                    }
                }

                idx++;
            }

            var elementArray = Array.CreateInstance(elemType, elements.Count);
            elements.CopyTo(elementArray, 0);

            return elementArray;
        }

        private static void SetPropFromValue(object value, object item, PropertyInfo pi, ICollection<ConvertProblem> problems)
        {
            WriteToProperty(item, pi, value, problems);
        }

        private static void WriteToProperty(object item, PropertyInfo pi, object value, ICollection<ConvertProblem> problems)
        {
            if (!pi.CanWrite)
                return;

            try
            {
                if (value != null && !Equals(value, ""))
                {
                    Type destType = pi.PropertyType;

                    if (destType == typeof(bool) && Equals(value, pi.Name))
                    {
                        value = true;
                    }

                    if (pi.PropertyType.IsAssignableFrom(value.GetType()))
                    {
                        item.SetPropertyValue(pi.Name, value);
                        return;
                    }

                    if (pi.PropertyType.IsNullable())
                    {
                        destType = pi.PropertyType.GetGenericArguments()[0];
                    }

                    item.SetPropertyValue(pi.Name, value.Convert(destType));
                }
            }
            catch (Exception ex)
            {
                problems.Add(new ConvertProblem { Item = item, Property = pi, AttemptedValue = value, Exception = ex });
            }
        }

    }
}
