using Autofac;
using Chenyuan.Autofac;
using Chenyuan.Date;
using Chenyuan.Date.Entity;
using Chenyuan.Date.V2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Chenyuan.Date.Extensions;
using System.Data.Entity;
using Chenyuan.Log;
using Chenyuan.Data;

namespace Chenyuan.DAL
{
    /// <summary>
    /// 实体仓储服务
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : EntityObject
    {
        protected readonly DbContext _dbContext;
        protected readonly IDbSet<TEntity> _entities;
        protected IDataLog _dataLog;
        public EntityRepository()
        {
            Type entityType = typeof(TEntity);
            Type dbType = entityType.Assembly.GetTypes().Where(x => x.GetInterface(typeof(IDatabase).Name) != null).SingleOrDefault();
            if (dbType == null)
            {
                throw new Exception($"Can't find class inherit of IDatabase in {entityType.Name} Entity Library ");
            }
            if (DependencyInjection.Current.ScopeContainer.IsRegisteredWithKey(dbType, typeof(DataContext)))
            {
                try
                {
                    _dbContext = DependencyInjection.Current.Scope().ResolveKeyed(dbType, typeof(DataContext)) as DataContext;
                }
                catch
                {
                    var db = Activator.CreateInstance(dbType) as IDatabase;
                    var dataContextType = Assembly.Load(db.DataContextName).GetTypes().Where(x => x.BaseType == typeof(DataContext)).FirstOrDefault();
                    _dbContext = Activator.CreateInstance(dataContextType, db.ConnString, db.MappingsAssembly) as DataContext;
                }
            }
            else
            {
                throw new Exception($"Not register DataContext of {dbType.Name}");
            }
            _entities = _dbContext.Set<TEntity>();
            this.InitLogService();
        }

        public EntityRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
            _entities = _dbContext.Set<TEntity>();
            this.InitLogService();
        }
        /// <summary>
        /// 初始化log服务
        /// </summary>
        private void InitLogService()
        {
            try
            {
                _dataLog = DependencyInjection.Current.ScopeContainer.Resolve<IDataLog>();
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// insert
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(TEntity entity)
        {
            _entities.Add(entity);
            _dbContext.SaveChanges();

            if (_dataLog != null && _dataLog.NeedLog<TEntity>())
            {
                _dataLog.EntityLog<TEntity>(typeof(TEntity).GetProperty("Id").GetValue(entity).ToString(), null, InternalJSON(entity), "C"); //entity change log
            }
        }

        /// <summary>
        /// batch insert
        /// </summary>
        /// <param name="entities"></param>
        public void BastchInsert(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                _entities.Add(entity);
                if (_dataLog != null && _dataLog.NeedLog<TEntity>())
                {
                    _dataLog.EntityLog<TEntity>(typeof(TEntity).GetProperty("Id").GetValue(entity).ToString(), null, InternalJSON(entity), "C"); //entity change log
                }
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// delete
        /// deleted status delete
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(TEntity entity)
        {
            //_entities.Remove(entity);
            entity.Deleted = true;
            entity.LastUpdatedOn = DateTime.Now;
            _entities.Attach(entity);
            _dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            _dbContext.SaveChanges();

            if (_dataLog != null && _dataLog.NeedLog<TEntity>())
            {
                _dataLog.EntityLog<TEntity>(typeof(TEntity).GetProperty("Id").GetValue(entity).ToString(), InternalJSON(entity), null, "D"); //entity change log
            }
        }

        /// <summary>
        /// batch delete
        /// shift delete
        /// </summary>
        /// <param name="entities"></param>
        public void BatchDelete(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                _entities.Remove(entity);
                if (_dataLog != null && _dataLog.NeedLog<TEntity>())
                {
                    _dataLog.EntityLog<TEntity>(typeof(TEntity).GetProperty("Id").GetValue(entity).ToString(), InternalJSON(entity), null, "D"); //entity change log
                }
            }
            _dbContext.SaveChanges();
        }
        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id"></param>
        public void Delete(object id)
        {
            var entity = this.Get(id);
            _entities.Remove(entity);
            _dbContext.SaveChanges();
            if (_dataLog != null && _dataLog.NeedLog<TEntity>())
            {
                _dataLog.EntityLog<TEntity>(typeof(TEntity).GetProperty("Id").GetValue(entity).ToString(), InternalJSON(entity), null, "D"); //entity change log
            }
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TEntity entity)
        {
            if (_dataLog != null && _dataLog.NeedLog<TEntity>())
            {
                _dataLog.EntityLog<TEntity>(typeof(TEntity).GetProperty("Id").GetValue(entity).ToString(), InternalJSON(_dbContext.Entry<TEntity>(entity).Entity), InternalJSON(entity), "U"); //entity change log
            }

            entity.LastUpdatedOn = DateTime.Now;
            _entities.Attach(entity);
            _dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// batch update
        /// </summary>
        /// <param name="entities"></param>
        public void BatchUpdate(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (_dataLog != null && _dataLog.NeedLog<TEntity>())
                {
                    _dataLog.EntityLog<TEntity>(typeof(TEntity).GetProperty("Id").GetValue(entity).ToString(), InternalJSON(_dbContext.Entry<TEntity>(entity).Entity), InternalJSON(entity), "U"); //entity change log
                }

                entity.LastUpdatedOn = DateTime.Now;
                _entities.Attach(entity);
                _dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            }
            _dbContext.SaveChanges();
        }
        /// <summary>
        /// get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity Get(object id)
        {
            return _entities.Find(id);
        }
        /// <summary>
        /// query
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> Table
        {
            get
            {
                return _entities.Where(x => !x.Deleted);
            }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        //public IQueryable<T> GetSet<T>() where T:EntityObject
        //{
        //    return _dbContext.Set<T>();
        //}
        /// <summary>
        /// ef query of page list
        /// </summary>
        /// <param name="sct"></param>
        /// <returns></returns>
        public IPagedList<TEntity> Query(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TEntity>> conditions)
        {
            var query = this.Table;
            if (conditions != null)
            {
                query = conditions(query);
            }
            return new PagedList<TEntity>(query, pageIndex, pageSize);
        }
        /// <summary>
        /// ef query of list
        /// </summary>
        /// <param name="sct"></param>
        /// <returns></returns>
        public IList<TEntity> Query(Func<IQueryable<TEntity>, IQueryable<TEntity>> conditions)
        {
            var query = this.Table;
            if (conditions != null)
            {
                query = conditions(query);
            }
            return query.ToList();
        }
        /// <summary>
        /// query result for page list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public IPagedList<T> ExecuteQuery<T>(string sql, int pageIndex, int pageSize, IList<SqlParameter> parameters) where T : class, new()
        {
            //out put total count parameter
            SqlParameter s_totaChenyuanount = new SqlParameter("TotaChenyuanount", 0);
            s_totaChenyuanount.Direction = System.Data.ParameterDirection.Output;
            parameters.Add(s_totaChenyuanount);

            var list = _dbContext.Database.SqlQuery<T>(sql, parameters.ToArray()).ToList();
            int totaChenyuanount = Convert.ToInt32(s_totaChenyuanount.Value);
            return new PagedList<T>(list, pageIndex, pageSize, totaChenyuanount);
        }
        /// <summary>
        /// query result for list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IList<T> ExecuteQuery<T>(string sql, params SqlParameter[] parameters)
        {
            return _dbContext.Database.SqlQuery<T>(sql, parameters).ToList();
        }
        /// <summary>
        /// sql query for datatable to list entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IList<T> DataQuery<T>(string sql, params SqlParameter[] parameters) where T : new()
        {
            SqlCommand cmd = new SqlCommand(sql, _dbContext.Database.Connection as SqlConnection);
            cmd.Parameters.AddRange(parameters);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            sda.Dispose();
            var list = dt.FillEntity<T>();
            return list;
        }
        /// <summary>
        /// sql query for ds
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet DataQuery(string sql, params SqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand(sql, _dbContext.Database.Connection as SqlConnection);
            cmd.Parameters.AddRange(parameters);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(ds);
            sda.Dispose();
            return ds;
        }
        /// <summary>
        /// execute command
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="parameters"></param>
        public int ExecuteCommand(string sql, params SqlParameter[] parameters)
        {
            return _dbContext.Database.ExecuteSqlCommand(sql, parameters);
        }
        /// <summary>
        /// log
        /// </summary>
        /// <param name="ex"></param>
        public void LogException(Exception ex)
        {

        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string InternalJSON(EntityObject obj)
        {
            try
            {
                if (_dataLog.NeedLog<TEntity>())
                {
                    return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    });
                }
                return "";
            }
            catch
            {
                return $"{obj.GetType().Name}";
            }
        }
    }
}
