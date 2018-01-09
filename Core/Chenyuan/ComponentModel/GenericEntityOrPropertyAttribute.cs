using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.ComponentModel
{
	/// <summary>
	/// 泛型关联关系特性定义
	/// </summary>
	/// <remarks>
	/// 该特性仅作为泛型关联标识
	/// </remarks>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum)]
	public sealed class GenericEntityOrPropertyAttribute : Attribute
	{
	}
}
