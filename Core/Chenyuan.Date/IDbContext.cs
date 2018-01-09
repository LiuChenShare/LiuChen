using System;
using System.Collections.Generic;
using System.Data;
using Chenyuan.Data.Entity;

namespace Chenyuan.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IDbSetProxy<TEntity> Set<TEntity>() where TEntity : EntityObject;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IList<TEntity> ExecuteStoredProcedureForBaseEntity<TEntity>(string commandText, params object[] parameters) 
            where TEntity : EntityObject, new();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IList<TEntity> ExecuteStoredProcedure<TEntity>(string procedureName, params object[] parameters)
            where TEntity : class, new();

        /// <summary>
        /// Get multiple table result from stored procedure.
        /// </summary>
        /// <param name="types"></param>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IDictionary<Type, List<object>> ExecuteStoredProcedure(Type[] types, string procedureName, params object[] parameters);

        /// <summary>
        /// Get multiple table result from stored procedure for array.
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="types"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        dynamic[] ExecuteStoredProcedure(string procedureName, Type[] types, params object[] parameters);
        /// <summary>
        /// Batch insert datatable to sql server table
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tablename"></param>
        void ExecuteBatchInsert<T>(DataTable dt, string tablename);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        void ExecuteStoredProcedure(string procedureName, params object[] parameters);

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters);
 
        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        int ExecuteSqlCommand(string sql, int? timeout = null, params object[] parameters);

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">The command string</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        IList<TEntity> ExecuteSqlCommand<TEntity>(string sql, int? timeout = null, params object[] parameters) where TEntity : class, new();

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        object ExecuteSqlFunc(string sql, int? timeout = null, params object[] parameters);

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        List<object> ExecuteSqlFuncList(string sql, int? timeout = null, params object[] parameters);

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">The command string</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        IList<TEntity> ExecuteSqlCommandForBaseEntity<TEntity>(string sql, int? timeout = null, params object[] parameters) where TEntity : EntityObject, new();

		///// <summary>Executes sql by using SQL-Server Management Objects which supports GO statements.</summary>
		///// <remarks>codehint: sm-add</remarks>
		//int ExecuteSqlThroughSmo(string sql);

        // codehint: sm-add (required for UoW implementation)
        /// <summary>
        /// 
        /// </summary>
        string Alias { get; }

        // codehint: sm-add (increasing performance on bulk inserts)
        /// <summary>
        /// 
        /// </summary>
        bool ProxyCreationEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool AutoDetectChangesEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool ValidateOnSaveEnabled{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool HasChanges { get; }

        /// <summary>
        /// 附加实体对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Attatch<TEntity>(TEntity entity)
            where TEntity : EntityObject;
    }
}
