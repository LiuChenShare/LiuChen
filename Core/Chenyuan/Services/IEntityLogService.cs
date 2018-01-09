using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.ComponentModel;
using Chenyuan.Data;

namespace Chenyuan.Services
{
	/// <summary>
	/// 实体日志处理服务接口定义
	/// </summary>
	public interface IEntityLogService
	{
		/// <summary>
		/// 创建实体对象日志登记
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="engityLogInfo"></param>
		/// <param name="autoCommit"></param>
		void Create<TEntity>(IEntityLogInfo<TEntity> engityLogInfo, bool autoCommit)
			where TEntity : BaseEntity;

		/// <summary>
		/// 更新实体对象日志登记
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="engityLogInfo"></param>
		/// <param name="autoCommit"></param>
		void Update<TEntity>(IEntityLogInfo<TEntity> engityLogInfo, bool autoCommit)
		 where TEntity : BaseEntity;

		/// <summary>
		/// 删除实体对象日志登记
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="engityLogInfo"></param>
		/// <param name="autoCommit"></param>
		void Delete<TEntity>(IEntityLogInfo<TEntity> engityLogInfo, bool autoCommit)
		where TEntity : BaseEntity;
	}
}
