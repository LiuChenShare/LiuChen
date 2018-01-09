using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Infrastructure;

namespace Chenyuan.Data
{
	/// <summary>
	/// 实体映射基类定义
	/// </summary>
	/// <typeparam name="TEntity">实体类型，必须派生自<seealso cref="Chenyuan.Data.BaseEntity"/></typeparam>
	public class BaseEntityMap<TEntity> : EntityTypeConfiguration<TEntity>
		where TEntity : BaseEntity
	{
		/// <summary>
		/// 构造函数，执行必要的初始化处理
		/// </summary>
		public BaseEntityMap()
		{
			_databaseGeneratedOption = DatabaseGeneratedOption.Identity;
			this.Initialization();
		}

		#region 实体架构属性

		/// <summary>
		/// 获取数据表名，默认为实体类型名
		/// </summary>
		protected virtual string TableName
		{
			get
			{
				return typeof(TEntity).Name;
			}
		}

		/// <summary>
		/// 获取数据架构名，默认为 dbo
		/// </summary>
		protected virtual string SchemaName
		{
			get
			{
				return "dbo";
			}
		}

		#endregion

		#region 初始化配置信息

		private DatabaseGeneratedOption _databaseGeneratedOption;
		/// <summary>
		/// 键值生成方案
		/// </summary>
		protected virtual DatabaseGeneratedOption KeyDatabaseGeneratedOption
		{
			get
			{
				return _databaseGeneratedOption;
			}
		}

		/// <summary>
		/// 主键初始化操作
		/// </summary>
		protected virtual void InitKey()
		{
			this.HasKey(x => x.Id);
			this.Property(x => x.Id).HasDatabaseGeneratedOption(KeyDatabaseGeneratedOption);
			//this.Property(x => x.IID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
		}

		/// <summary>
		/// 初始化时间戳属性
		/// </summary>
		protected virtual void InitTimestamp()
		{
			this.Property(x => x.Timestamp)
				.IsRequired()
				.HasMaxLength(8)
				.IsRowVersion()
				.IsFixedLength();
		}

		/// <summary>
		/// 初始化创建时间属性
		/// </summary>
		protected virtual void InitCreatedOn()
		{
			this.Property(x => x.CreatedOn)
				.IsRequired();
		}
		/// <summary>
		/// 初始化最后更新时间属性
		/// </summary>
		protected virtual void InitLastUpdatedOn()
		{
			this.Property(x => x.LastUpdatedOn)
				.IsRequired();
		}
		/// <summary>
		/// 初始化数据描述
		/// </summary>
		protected virtual void InitDescription()
		{
			this.Property(x => x.Description)
				.IsUnicode()
				.IsOptional();
		}

		/// <summary>
		/// 初始化实体属性
		/// </summary>
		protected virtual void InitProperties()
		{
		}

		/// <summary>
		/// 初始化实体关系
		/// </summary>
		protected virtual void InitAssociations()
		{
		}

		/// <summary>
		/// 初始化表名
		/// </summary>
		protected virtual void InitTableName()
		{
			this.ToTable(this.TableName, this.SchemaName);
		}

		/// <summary>
		/// 初始化系统
		/// </summary>
		protected virtual void Initialization()
		{
			this.InitTableName();
			this.InitKey();
			this.InitTimestamp();
			this.InitCreatedOn();
			this.InitLastUpdatedOn();
			this.InitDescription();
			this.InitProperties();
			this.InitAssociations();
		}

		#endregion

		#region 映射辅助方法

		/// <summary>
		/// 可空对象映射
		/// </summary>
		/// <typeparam name="TTargetEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="navigationPropertyExpression"></param>
		/// <param name="foreignKeyExpression"></param>
		/// <param name="navigationCollectionPropertyExpression"></param>
		/// <param name="willCascadeOnDelete"></param>
		protected virtual void OptionalMap<TTargetEntity, TKey>(Expression<Func<TEntity, TTargetEntity>> navigationPropertyExpression, Expression<Func<TEntity, TKey>> foreignKeyExpression, Expression<Func<TTargetEntity, ICollection<TEntity>>> navigationCollectionPropertyExpression = null, bool willCascadeOnDelete = true)
			where TTargetEntity : class
		{
			this.OneToManyMap(navigationPropertyExpression, foreignKeyExpression, navigationCollectionPropertyExpression, willCascadeOnDelete, false);
		}

		/// <summary>
		/// 可空对象映射
		/// </summary>
		/// <typeparam name="TTargetEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="navigationCollectionPropertyExpression"></param>
		/// <param name="foreignKeyExpression"></param>
		/// <param name="navigationPropertyExpression"></param>
		/// <param name="willCascadeOnDelete"></param>
		protected virtual void OptionalMap<TTargetEntity, TKey>(Expression<Func<TEntity, ICollection<TTargetEntity>>> navigationCollectionPropertyExpression, Expression<Func<TTargetEntity, TKey>> foreignKeyExpression, Expression<Func<TTargetEntity, TEntity>> navigationPropertyExpression = null, bool willCascadeOnDelete = true)
			where TTargetEntity : class
		{
			this.ManyToOneMap(navigationCollectionPropertyExpression, foreignKeyExpression, navigationPropertyExpression, willCascadeOnDelete, false);
		}

		/// <summary>
		/// 必须对象映射
		/// </summary>
		/// <typeparam name="TTargetEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="navigationPropertyExpression"></param>
		/// <param name="foreignKeyExpression"></param>
		/// <param name="navigationCollectionPropertyExpression"></param>
		/// <param name="willCascadeOnDelete"></param>
		protected virtual void RequiredMap<TTargetEntity, TKey>(Expression<Func<TEntity, TTargetEntity>> navigationPropertyExpression, Expression<Func<TEntity, TKey>> foreignKeyExpression, Expression<Func<TTargetEntity, ICollection<TEntity>>> navigationCollectionPropertyExpression = null, bool willCascadeOnDelete = true)
			where TTargetEntity : class
		{
			this.OneToManyMap(navigationPropertyExpression, foreignKeyExpression, navigationCollectionPropertyExpression, willCascadeOnDelete, true);
		}

		/// <summary>
		/// 必须对象映射
		/// </summary>
		/// <typeparam name="TTargetEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="navigationCollectionPropertyExpression"></param>
		/// <param name="foreignKeyExpression"></param>
		/// <param name="navigationPropertyExpression"></param>
		/// <param name="willCascadeOnDelete"></param>
		protected virtual void RequiredMap<TTargetEntity, TKey>(Expression<Func<TEntity, ICollection<TTargetEntity>>> navigationCollectionPropertyExpression, Expression<Func<TTargetEntity, TKey>> foreignKeyExpression, Expression<Func<TTargetEntity, TEntity>> navigationPropertyExpression = null, bool willCascadeOnDelete = true)
			where TTargetEntity : class
		{
			this.ManyToOneMap(navigationCollectionPropertyExpression, foreignKeyExpression, navigationPropertyExpression, willCascadeOnDelete, true);
		}

		/// <summary>
		/// 多对一映射
		/// </summary>
		/// <typeparam name="TTargetEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="navigationCollectionPropertyExpression"></param>
		/// <param name="foreignKeyExpression"></param>
		/// <param name="navigationPropertyExpression"></param>
		/// <param name="willCascadeOnDelete"></param>
		/// <param name="required"></param>
		protected virtual void ManyToOneMap<TTargetEntity, TKey>(Expression<Func<TEntity, ICollection<TTargetEntity>>> navigationCollectionPropertyExpression, Expression<Func<TTargetEntity, TKey>> foreignKeyExpression, Expression<Func<TTargetEntity, TEntity>> navigationPropertyExpression = null, bool willCascadeOnDelete = true, bool required = true)
			where TTargetEntity : class
		{
			Guard.ArgumentNotNull(() => navigationCollectionPropertyExpression);
			Guard.ArgumentNotNull(() => foreignKeyExpression);
			var t = this.HasMany(navigationCollectionPropertyExpression);
			DependentNavigationPropertyConfiguration<TTargetEntity> dependentNavigationPropertyConfiguration = null;
			if (navigationPropertyExpression == null)
			{
				dependentNavigationPropertyConfiguration = required ? t.WithRequired() : t.WithOptional();
			}
			else
			{
				dependentNavigationPropertyConfiguration = required ? t.WithRequired(navigationPropertyExpression) : t.WithOptional(navigationPropertyExpression);
			}
			dependentNavigationPropertyConfiguration.HasForeignKey(foreignKeyExpression).WillCascadeOnDelete(willCascadeOnDelete);
		}

		/// <summary>
		/// 一对多映射
		/// </summary>
		/// <typeparam name="TTargetEntity"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="navigationPropertyExpression"></param>
		/// <param name="foreignKeyExpression"></param>
		/// <param name="navigationCollectionPropertyExpression"></param>
		/// <param name="willCascadeOnDelete"></param>
		/// <param name="required"></param>
		protected virtual void OneToManyMap<TTargetEntity, TKey>(Expression<Func<TEntity, TTargetEntity>> navigationPropertyExpression, Expression<Func<TEntity, TKey>> foreignKeyExpression, Expression<Func<TTargetEntity, ICollection<TEntity>>> navigationCollectionPropertyExpression = null, bool willCascadeOnDelete = true, bool required = true)
			where TTargetEntity : class
		{
			Guard.ArgumentNotNull(() => navigationPropertyExpression);
			Guard.ArgumentNotNull(() => foreignKeyExpression);
			dynamic t = null;
			if (required)
			{
				t = this.HasRequired(navigationPropertyExpression);
			}
			else
			{
				t = this.HasOptional(navigationPropertyExpression);
			}
			DependentNavigationPropertyConfiguration<TEntity> dependentNavigationPropertyConfiguration = null;
			if (navigationCollectionPropertyExpression == null)
			{
				dependentNavigationPropertyConfiguration = t.WithMany();
			}
			else
			{
				dependentNavigationPropertyConfiguration = t.WithMany(navigationCollectionPropertyExpression);
			}
			dependentNavigationPropertyConfiguration.HasForeignKey(foreignKeyExpression).WillCascadeOnDelete(willCascadeOnDelete);
		}

		#endregion
	}
}
