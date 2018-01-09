using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data
{
	/// <summary>
	/// 
	/// </summary>
	public interface IEfDataProvider : IDataProvider
	{
		/// <summary>
		/// Get connection factory
		/// </summary>
		/// <returns>Connection factory</returns>
		IDbConnectionFactory GetConnectionFactory();

		/// <summary>
		/// Initialize connection factory
		/// </summary>
		void InitConnectionFactory();

		/// <summary>
		/// Set database initializer
		/// </summary>
		void SetDatabaseInitializer();
	}
}
