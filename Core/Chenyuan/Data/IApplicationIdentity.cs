using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Messages;

namespace Chenyuan.Data
{
	/// <summary>
	/// 应用标识定义
	/// </summary>
	public interface IApplicationIdentity
	{
		/// <summary>
		/// 应用具备的有效模块集合
		/// </summary>
		IReadOnlyCollection<IApplicationModalIdentity> Modals { get; }

		/// <summary>
		/// 获取站点应用的安全模式
		/// </summary>
		/// <remarks>codehint: sm-add</remarks>
		HttpSecurityMode GetSecurityMode(bool? useSsl = null);

		/// <summary>
		/// 获取站点应用的安全Url(HTTPS)
		/// </summary>
		string SecureUrl { get; }

		/// <summary>
		/// 键
		/// </summary>
		Guid Id
		{
			get;
		}

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
		/// 应用主题
		/// </summary>
		string ThemeName
		{
			get;
		}

		/// <summary>
		/// 是否根应用
		/// </summary>
		bool IsRoot
		{
			get;
		}

		/// <summary>
		/// 获取父级应用
		/// </summary>
		IApplicationIdentity Parent
		{
			get;
		}

		/// <summary>
		/// 获取子级应用集合
		/// </summary>
		IReadOnlyCollection<IApplicationIdentity> Children
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tokens"></param>
		/// <returns></returns>
		TokenCollection LoadTokens(TokenCollection tokens = null);
	}
}
