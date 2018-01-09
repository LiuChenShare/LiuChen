using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Infrastructure;

namespace Chenyuan.Data
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class BaseEfDataProvider : IEfDataProvider
	{
		/// <summary>
		/// Get connection factory
		/// </summary>
		/// <returns>Connection factory</returns>
		public abstract IDbConnectionFactory GetConnectionFactory();

		/// <summary>
		/// Initialize connection factory
		/// </summary>
		public void InitConnectionFactory()
		{
			Database.DefaultConnectionFactory = GetConnectionFactory();
		}

		/// <summary>
		/// Set database initializer
		/// </summary>
		public abstract void SetDatabaseInitializer();

		/// <summary>
		/// Initialize database
		/// </summary>
		public virtual void InitDatabase()
		{
			InitConnectionFactory();
			SetDatabaseInitializer();
		}

		/// <summary>
		/// A value indicating whether this data provider supports stored procedures
		/// </summary>
		public abstract bool StoredProceduredSupported { get; }

		/// <summary>
		/// Gets a support database parameter object (used by stored procedures)
		/// </summary>
		/// <returns>Parameter</returns>
		public abstract DbParameter GetParameter();

		/// <summary>
		/// 映射路径
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		protected virtual string MapPath(string path)
		{
			return EngineContext.Current.Resolve<IWebHelper>().MapPath(path);
		}
	}
}
