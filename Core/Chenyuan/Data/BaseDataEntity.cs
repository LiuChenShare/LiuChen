using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data
{
	// <summary>
	/// There are no comments for Chenyuan.Common.Domains.BaseDataEntity in the schema.
	/// </summary>
	[Serializable]
	public abstract partial class BaseDataEntity : BaseEntity
	{
		/// <summary>
		/// 
		/// </summary>
		public BaseDataEntity()
		{
			this.LastUpdatedOn = DateTime.Now;
		}

		#region Properties

		///// <summary>
		///// There are no comments for LastUpdatedOn in the schema.
		///// </summary>
		//public virtual DateTime LastUpdatedOn
		//{
		//	get;
		//	set;
		//}

		/// <summary>
		/// 是否可删除
		/// </summary>
		/// <LongDescription>
		/// 默认为是，指示数据内容是否可直接删除或标记为删除状态
		/// </LongDescription>
		[DefaultValue(true)]
		public virtual bool Deletable
		{
			get
			{
				return _deletable;
			}
			set
			{
				_deletable = value;
			}
		}
		private bool _deletable = false;

		#endregion

	}
}
