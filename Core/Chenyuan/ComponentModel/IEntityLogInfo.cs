using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Chenyuan.Data;

namespace Chenyuan.ComponentModel
{
	/// <summary>
	/// 实体日志行为类型定义
	/// </summary>
	public enum EntityLogActionType
	{
		/// <summary>
		/// 未知行为
		/// </summary>
		Unknown,
		/// <summary>
		/// 新建
		/// </summary>
		Create,
		/// <summary>
		/// 更新
		/// </summary>
		Update,
		/// <summary>
		/// 删除
		/// </summary>
		Delete,
	}

	/// <summary>
	/// 实体日志信息接口定义
	/// </summary>
	public interface IEntityLogInfo<TEntity>
		where TEntity : BaseEntity
	{
		/// <summary>
		/// 更新实体日志
		/// </summary>
		void Update(bool autoCommit);

		///// <summary>
		///// 初始化数据
		///// </summary>
		//void Init();

		/// <summary>
		/// 获取当前实体日志信息是否已经更新了
		/// </summary>
		bool Updated { get; }

		/// <summary>
		/// 获取对应实体对象
		/// </summary>
		TEntity Entity
		{
			get;
		}

		/// <summary>
		/// 实体日志行为
		/// </summary>
		EntityLogActionType LogAction
		{
			get;
		}

		/// <summary>
		/// 日志数据内容
		/// </summary>
		XElement Data
		{
			get;
		}

		/// <summary>
		/// 是否空对象
		/// </summary>
		bool IsEmpty
		{
			get;
		}

		/// <summary>
		/// 自动记录日志
		/// </summary>
		bool AutoLog
		{
			get;
		}

		/// <summary>
		/// 获取日志行为名称
		/// </summary>
		string LogActionName
		{
			get;
		}
	}
}
