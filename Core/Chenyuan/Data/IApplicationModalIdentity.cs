using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data
{
	/// <summary>
	/// 应用模块接口对象标识定义
	/// </summary>
	public interface IApplicationModalIdentity
	{
		/// <summary>
		/// 获取模块的子模块集合
		/// </summary>
		IReadOnlyCollection<IApplicationModalIdentity> Children { get; }

		/// <summary>
		/// 键
		/// </summary>
		Guid Id
		{
			get;
		}

		/// <summary>
		/// 应用对象
		/// </summary>
		/// <remarks>
		/// </remarks>
		IApplicationIdentity Application
		{
			get;
		}

		/// <summary>
		/// 上级模块对象
		/// </summary>
		/// <remarks>
		/// 实现多级级联，获取上级实体对象id
		/// </remarks>
		IApplicationModalIdentity ParentModal
		{
			get;
		}
		/// <summary>
		/// 上级模块Id
		/// </summary>
		Guid? ParentId { get; }

		/// <summary>
		/// 应用名称
		/// </summary>
		/// <remarks>
		/// 应用名称，用于友好识别应用。
		/// </remarks>
		//[MaxLength(100)]
		string Name
		{
			get;
		}

		/// <summary>
		/// 应用代码
		/// </summary>
		/// <remarks>
		/// 应用代码定义，全局唯一，用于识别应用。
		/// </remarks>
		//[MaxLength(50)]
		string Code
		{
			get;
		}

		/// <summary>
		/// 模块入口
		/// </summary>
		/// <remarks>
		/// 记录模块的默认站点入口
		/// </remarks>
		//[MaxLength(512)]
		string Entry
		{
			get;
		}

		/// <summary>
		/// 模块入口地址集合
		/// </summary>
		/// <remarks>
		/// 以半角逗号间隔的多个模块入口地址的字符串集
		/// </remarks>
		IReadOnlyCollection<string> Entries
		{
			get;
		}

		/// <summary>
		/// 标题
		/// </summary>
		/// <remarks>
		/// 标题文本内容，用于显示
		/// </remarks>
		//[MaxLength(50)]
		string Title
		{
			get;
		}

		/// <summary>
		/// 是否应用下的根模块
		/// </summary>
		bool IsRoot
		{
			get;
		}

		/// <summary>
		/// 层级深度
		/// </summary>
		int Deep
		{
			get;
		}

		/// <summary>
		/// 系统识别关键词
		/// </summary>
		string SystemKeywords
		{
			get;
		}

		/// <summary>
		/// 是否快捷连接容器模块
		/// </summary>
		bool IsShortcutContainer
		{
			get;
		}

		/// <summary>
		/// 项图标样式名
		/// </summary>
		string IconClassname
		{
			get;
		}

		/// <summary>
		/// 是否自动展开
		/// </summary>
		bool Expanded
		{
			get;
		}

		/// <summary>
		/// 是否可见
		/// </summary>
		bool Visibility
		{
			get;
		}

		/// <summary>
		/// 是否已启用
		/// </summary>
		bool Enabled
		{
			get;
		}

		/// <summary>
		/// 是否已删除
		/// </summary>
		bool Deleted
		{
			get;
		}

		/// <summary>
		/// 获取子Id
		/// </summary>
		int SubId
		{
			get;
		}

		/// <summary>
		/// 是否可继承
		/// </summary>
		bool Inheritable
		{
			get;
		}
	}
}
