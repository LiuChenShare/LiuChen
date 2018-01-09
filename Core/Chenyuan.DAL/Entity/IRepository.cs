using Chenyuan.Date;
using Chenyuan.Date.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.DAL.Entity
{
    public interface IRepository<TEntity> where TEntity : EntityObject
    {
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);
        /// <summary>
        /// batch insert
        /// </summary>
        /// <param name="entities"></param>
        void BastchInsert(IEnumerable<TEntity> entities);

        /// <summary>
        /// delete
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);
        /// <summary>
        /// batch delete
        /// </summary>
        /// <param name="entities"></param>
        void BatchDelete(IEnumerable<TEntity> entities);

        /// <summary>
        /// update
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);
        /// <summary>
        /// batch update
        /// </summary>
        /// <param name="entities"></param>
        void BatchUpdate(IEnumerable<TEntity> entities);
        /// <summary>
        /// get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Get(object id);
        /// <summary>
        /// query
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> Table { get; }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        //IQueryable<T> GetSet<T>() where T : EntityObject;
        /// <summary>
        /// ef query of page list
        /// </summary>
        /// <param name="sct"></param>
        /// <returns></returns>
        IPagedList<TEntity> Query(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> conditions);
        /// <summary>
        /// ef query of list
        /// </summary>
        /// <param name="sct"></param>
        /// <returns></returns>
        IList<TEntity> Query(Func<IQueryable<TEntity>, IQueryable<TEntity>> conditions);
        /// <summary>
        /// query result for page list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql command</param>
        /// <returns></returns>
        IPagedList<T> ExecuteQuery<T>(string sql, int pageIndex, int pageSize, IList<SqlParameter> parameters) where T : class, new();
        /// <summary>
        /// query result for list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql command</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IList<T> ExecuteQuery<T>(string sql, params SqlParameter[] parameters);
        /// <summary>
        /// sql query for datatable to list entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IList<T> DataQuery<T>(string sql, params SqlParameter[] parameters) where T : new();
        /// <summary>
        /// sql query for ds
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DataSet DataQuery(string sql, params SqlParameter[] parameters);
        /// <summary>
        /// execute command
        /// </summary>
        /// <param name="sql">sql command</param>
        /// <param name="parameters"></param>
        int ExecuteCommand(string sql, params SqlParameter[] parameters);
    }
}
