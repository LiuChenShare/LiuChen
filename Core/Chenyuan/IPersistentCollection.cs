using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan
{
    /// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IPersistentCollection<T> : ICollection<T>, ICollection
            where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        Action<ICollection<T>> AfterAdd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        Action<ICollection<T>> AfterRemove { get; set; }
        /// <summary>
        /// 
        /// </summary>
        Func<ICollection<T>, T, bool> BeforeAdd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        Func<ICollection<T>, T, bool> BeforeRemove { get; set; }
    }
}
