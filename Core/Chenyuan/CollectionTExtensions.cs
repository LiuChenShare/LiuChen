using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan
{
    /// <summary>
	/// 
	/// </summary>
	public static class CollectionTExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="setParent"></param>
        /// <param name="setParentToNull"></param>
        /// <returns></returns>
        public static ICollection<T> SetupBeforeAndAfterActions<T>(
            this ICollection<T> value,
            Action<dynamic> setParent,
            Action<dynamic> setParentToNull)
            where T : class
        {
            //if (!CommonHelper.Default.OneToManyCollectionWrapperEnabled)
            //    return value;

            var list = value as IPersistentCollection<T> ?? new PersistentCollection<T>(value);
            list.BeforeAdd = (l, x) => l.BeforeAddItem(x, setParent);
            list.BeforeRemove = (l, x) => l.BeforeRemoveItem(x, setParentToNull);
            list.AfterAdd = AfterListChanges;
            list.AfterRemove = AfterListChanges;
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void AfterListChanges<T>(this ICollection<T> list) where T : class
        {
            // ...
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <param name="setParent"></param>
        /// <returns></returns>
        public static bool BeforeAddItem<T>(this ICollection<T> list, T item, Action<T> setParent) where T : class
        {
            // ...
            setParent(item);
            if (list.Any(item.Equals))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <param name="setParentToNull"></param>
        /// <returns></returns>
        public static bool BeforeRemoveItem<T>(this ICollection<T> list, T item, Action<T> setParentToNull) where T : class
        {
            setParentToNull(item);
            if (list.Any(item.Equals))
            {
                return true;
            }
            return false;
        }
    }
}
