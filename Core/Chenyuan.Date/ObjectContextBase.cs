using Chenyuan.Data;
using Chenyuan.Data.Hooks;
using Chenyuan.Date.Entity;
using Chenyuan.Date.Extensions;
using Chenyuan.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace Chenyuan.Date
{
    /// <summary>
	/// Object context
	/// </summary>
	public abstract class ObjectContextBase : DbContext, IChenyuanDBContext
    {
        #region Fields

        private IEnumerable<IHook> _hooks;
        private IList<IPreActionHook> _preHooks;
        private IList<IPostActionHook> _postHooks;

        #endregion

        #region Ctor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        /// <param name="alias"></param>
        protected ObjectContextBase(string nameOrConnectionString, string alias = null)
            : base(nameOrConnectionString)
        {
            //((IObjectContextAdapter) this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
            //base.Configuration.ProxyCreationEnabled = false;

            _preHooks = new List<IPreActionHook>();
            _postHooks = new List<IPostActionHook>();

            this.Alias = null;
        }

        #endregion

        #region Hooks

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IHook> Hooks
        {
            get
            {
                return _hooks ?? Enumerable.Empty<IHook>();
            }
            set
            {
                if (value != null)
                {
                    _preHooks = value.OfType<IPreActionHook>().ToList();
                    _postHooks = value.OfType<IPostActionHook>().ToList();
                }
                else
                {
                    _preHooks.Clear();
                    _postHooks.Clear();
                }
                _hooks = value;
            }
        }

        /// <summary>
        /// Executes the pre action hooks, filtered by <paramref name="requiresValidation"/>.
        /// </summary>
        /// <param name="modifiedEntries">The modified entries to execute hooks for.</param>
        /// <param name="requiresValidation">if set to <c>true</c> executes hooks that require validation, otherwise executes hooks that do NOT require validation.</param>
        private void ExecutePreActionHooks(IEnumerable<HookedEntityEntry> modifiedEntries, bool requiresValidation)
        {
            foreach (var entityEntry in modifiedEntries)
            {
                var entry = entityEntry; // Prevents access to modified closure

                foreach (
                    var hook in
                        _preHooks.Where(x => x.HookStates == entry.PreSaveState && x.RequiresValidation == requiresValidation))
                {
                    var metadata = new HookEntityMetadata(entityEntry.PreSaveState);
                    hook.HookObject(entityEntry.Entity, metadata);

                    if (metadata.HasStateChanged)
                    {
                        entityEntry.PreSaveState = metadata.State;
                    }
                }
            }
        }

        /// <summary>
        /// Executes the post action hooks.
        /// </summary>
        /// <param name="modifiedEntries">The modified entries to execute hooks for.</param>
        private void ExecutePostActionHooks(IEnumerable<HookedEntityEntry> modifiedEntries)
        {
            foreach (var entityEntry in modifiedEntries)
            {
                foreach (var hook in _postHooks.Where(x => x.HookStates == entityEntry.PreSaveState))
                {
                    var metadata = new HookEntityMetadata(entityEntry.PreSaveState);
                    hook.HookObject(entityEntry.Entity, metadata);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual bool HooksEnabled
        {
            get { return true; }
        }

        #endregion

        private void PrepareDbParameter(DbCommand cmd, params object[] parameters)
        {
            this.PrepareDbParameter(parameters);
            if (parameters != null && parameters.Any())
            {
                foreach (var p in parameters)
                {
                    cmd.Parameters.Add(p);
                }
            }
        }

        private void PrepareDbParameter(params object[] parameters)
        {
            if (parameters != null && parameters.Any())
            {
                foreach (var p in parameters)
                {
                    if (p is DbParameter)
                    {
                        var dbp = p as DbParameter;
                        if (dbp.Value == null)
                        {
                            dbp.Value = Convert.DBNull;
                        }
                    }
                }
            }
        }

        #region IDbContext members

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string CreateDatabaseScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        public virtual void ExecuteStoredProcedure(string procedureName, params object[] parameters)
        {

            //var connection = context.Connection;
            var connection = this.Database.Connection;
            //Don't close the connection after command execution


            //open the connection for use
            bool closeConnectionOnExit = false;
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
                closeConnectionOnExit = true;
            }
            //create a command object
            using (var cmd = connection.CreateCommand())
            {
                //command to execute
                cmd.CommandText = procedureName;
                cmd.CommandType = CommandType.StoredProcedure;

                // move parameters to command object
                this.PrepareDbParameter(cmd, parameters);

                //database call
                cmd.ExecuteNonQuery();
            }
            if (closeConnectionOnExit)
            {
                connection.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual IList<TEntity> ExecuteStoredProcedure<TEntity>(string procedureName, params object[] parameters) where TEntity : class, new()
        {
            //HACK: Entity Framework Code First doesn't support output parameters
            //That's why we have to manually create command and execute it.
            //just wait until EF Code First starts support them
            //
            //More info: http://weblogs.asp.net/dwahlin/archive/2011/09/23/using-entity-framework-code-first-with-stored-procedures-that-have-output-parameters.aspx

            var detectChanges = this.AutoDetectChangesEnabled;
            this.AutoDetectChangesEnabled = false;

            IList<TEntity> result = null;
            try
            {
                var context = ((IObjectContextAdapter)(this)).ObjectContext;

                //var connection = context.Connection;
                var connection = this.Database.Connection;
                //Don't close the connection after command execution

                //open the connection for use
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                //create a command object
                using (var cmd = connection.CreateCommand())
                {
                    //command to execute
                    cmd.CommandText = procedureName;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // move parameters to command object
                    this.PrepareDbParameter(cmd, parameters);
                    //database call
                    var reader = cmd.ExecuteReader();
                    result = context.Translate<TEntity>(reader).ToList();
                    reader.Close();
                }
            }
            finally
            {
                this.AutoDetectChangesEnabled = detectChanges;
            }

            return result;
        }

        /// <summary>
        /// Get multiple table result from stored procedure.
        /// </summary>
        /// <param name="types"></param>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual IDictionary<Type, List<object>> ExecuteStoredProcedure(Type[] types, string procedureName, params object[] parameters)
        {
            var detectChanges = this.AutoDetectChangesEnabled;
            this.AutoDetectChangesEnabled = false;

            DataSet ds = new DataSet();
            IDictionary<Type, List<object>> dicResult = new Dictionary<Type, List<object>>();
            try
            {
                var context = ((IObjectContextAdapter)(this)).ObjectContext;

                //var connection = context.Connection;
                var connection = this.Database.Connection;
                //Don't close the connection after command execution

                //open the connection for use
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                //create a command object
                using (var cmd = connection.CreateCommand())
                {
                    var provider = DbProviderFactories.GetFactory(connection);
                    var adapter = provider.CreateDataAdapter();
                    //command to execute
                    cmd.CommandText = procedureName;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // move parameters to command object
                    this.PrepareDbParameter(cmd, parameters);
                    adapter.SelectCommand = cmd;
                    //database call

                    adapter.Fill(ds);

                    for (var i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        if (types.Length > i)
                        {
                            dicResult.Add(types[i], dt.FillEntity(types[i]));
                        }
                        else
                        {
                            dicResult.Add(typeof(Object), dt.FillEntity());
                        }
                    }
                }
            }
            finally
            {
                this.AutoDetectChangesEnabled = detectChanges;
            }

            return dicResult;
        }

        /// <summary>
        /// Get multiple table result from stored procedure for array.
        /// </summary>
        /// <param name="types"></param>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual dynamic[] ExecuteStoredProcedure(string procedureName, Type[] types, params object[] parameters)
        {
            var detectChanges = this.AutoDetectChangesEnabled;
            this.AutoDetectChangesEnabled = false;

            DataSet ds = new DataSet();
            dynamic[] dicResult = new dynamic[types.Length];
            try
            {
                var context = ((IObjectContextAdapter)(this)).ObjectContext;

                //var connection = context.Connection;
                var connection = this.Database.Connection;
                //Don't close the connection after command execution

                //open the connection for use
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                //create a command object
                using (var cmd = connection.CreateCommand())
                {
                    var provider = DbProviderFactories.GetFactory(connection);
                    var adapter = provider.CreateDataAdapter();
                    //command to execute
                    cmd.CommandText = procedureName;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // move parameters to command object
                    this.PrepareDbParameter(cmd, parameters);
                    adapter.SelectCommand = cmd;
                    //database call

                    adapter.Fill(ds);

                    for (var i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        if (types.Length > i)
                        {
                            dicResult[i] = dt.FillEntity(types[i]);
                        }
                        else
                        {
                            dicResult[i] = dt.FillEntity();
                        }
                    }
                }
            }
            finally
            {
                this.AutoDetectChangesEnabled = detectChanges;
            }

            return dicResult;
        }

        /// <summary>
        /// Batch insert datatable to sql server table
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tablename"></param>
        public virtual void ExecuteBatchInsert<T>(DataTable dt, string tablename)
        {
            try
            {
                var context = ((IObjectContextAdapter)(this)).ObjectContext;

                //var connection = context.Connection;
                var connection = this.Database.Connection;
                //Don't close the connection after command execution

                dt.BatchInsert<T>(tablename, connection.ConnectionString);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  
        /// The type can be any type that has properties that match the names of the columns returned from the query, 
        /// or can be a simple primitive type. The type does not have to be an entity type. 
        /// The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            this.PrepareDbParameter(parameters);
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public int ExecuteSqlCommand(string sql, int? timeout = null, params object[] parameters)
        {
            Guard.ArgumentNotEmpty(sql, "sql");

            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //store previous timeout
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            // remove the GO statements
            //sql = Regex.Replace(sql, @"\r{0,1}\n[Gg][Oo]\r{0,1}\n", "\n");

            if (timeout.HasValue)
            {
                //Set previous timeout back
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }

            this.PrepareDbParameter(parameters);
            var result = this.Database.ExecuteSqlCommand(sql, parameters);

            return result;
        }

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">The command string</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public IList<TEntity> ExecuteSqlCommand<TEntity>(string sql, int? timeout = null, params object[] parameters) where TEntity : class, new()
        {

            var detectChanges = this.AutoDetectChangesEnabled;
            this.AutoDetectChangesEnabled = false;

            IList<TEntity> result = null;
            try
            {
                var context = ((IObjectContextAdapter)(this)).ObjectContext;

                //var connection = context.Connection;
                var connection = this.Database.Connection;
                //Don't close the connection after command execution


                //open the connection for use
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                //create a command object
                using (var cmd = connection.CreateCommand())
                {
                    //command to execute
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    // move parameters to command object
                    this.PrepareDbParameter(cmd, parameters);

                    //database call
                    var reader = cmd.ExecuteReader();

                    result = context.Translate<TEntity>(reader).ToList();
                    reader.Close();
                }
            }
            finally
            {
                this.AutoDetectChangesEnabled = detectChanges;
            }

            return result;
        }

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="timeout"></param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public object ExecuteSqlFunc(string sql, int? timeout = null, params object[] parameters)
        {

            var detectChanges = this.AutoDetectChangesEnabled;
            this.AutoDetectChangesEnabled = false;

            object result = null;
            try
            {
                var context = ((IObjectContextAdapter)(this)).ObjectContext;

                //var connection = context.Connection;
                var connection = this.Database.Connection;
                //Don't close the connection after command execution


                //open the connection for use
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                //create a command object
                using (var cmd = connection.CreateCommand())
                {
                    //command to execute
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    // move parameters to command object
                    this.PrepareDbParameter(cmd, parameters);

                    //database call
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        result = reader[0];
                    }
                    reader.Close();
                }
            }
            finally
            {
                this.AutoDetectChangesEnabled = detectChanges;
            }

            return result;
        }

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public List<object> ExecuteSqlFuncList(string sql, int? timeout = null, params object[] parameters)
        {

            var detectChanges = this.AutoDetectChangesEnabled;
            this.AutoDetectChangesEnabled = false;

            DataSet ds = new DataSet();
            List<object> result = new List<object>();
            try
            {
                var context = ((IObjectContextAdapter)(this)).ObjectContext;

                //var connection = context.Connection;
                var connection = this.Database.Connection;
                //Don't close the connection after command execution

                //open the connection for use
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                //create a command object
                using (var cmd = connection.CreateCommand())
                {
                    var provider = DbProviderFactories.GetFactory(connection);
                    var adapter = provider.CreateDataAdapter();
                    //command to execute
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    // move parameters to command object
                    this.PrepareDbParameter(cmd, parameters);
                    adapter.SelectCommand = cmd;
                    //database call

                    adapter.Fill(ds);

                    for (var i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.Add(dt.Rows[0][0]);
                    }
                }
            }
            finally
            {
                this.AutoDetectChangesEnabled = detectChanges;
            }

            return result;
        }

        /// <summary>
        /// 执行查询返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="timeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExecuteSql(string sql, int? timeout = null, params object[] parameters)
        {
            var detectChanges = this.AutoDetectChangesEnabled;
            this.AutoDetectChangesEnabled = false;

            DataSet ds = new DataSet();
            try
            {
                var context = ((IObjectContextAdapter)(this)).ObjectContext;

                //var connection = context.Connection;
                var connection = this.Database.Connection;
                //Don't close the connection after command execution

                //open the connection for use
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                //create a command object
                using (var cmd = connection.CreateCommand())
                {
                    var provider = DbProviderFactories.GetFactory(connection);
                    var adapter = provider.CreateDataAdapter();
                    //command to execute
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    // move parameters to command object
                    this.PrepareDbParameter(cmd, parameters);
                    adapter.SelectCommand = cmd;
                    //database call

                    adapter.Fill(ds);

                    return ds.Tables[0];
                }
            }
            finally
            {
                this.AutoDetectChangesEnabled = detectChanges;
            }

        }
        ///// <summary>
        ///// Executes the given DDL/DML command against the database.
        ///// </summary>
        ///// <typeparam name="TEntity"></typeparam>
        ///// <param name="sql">The command string</param>
        ///// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        ///// <param name="parameters">The parameters to apply to the command string.</param>
        ///// <returns>The result returned by the database after executing the command.</returns>
        //public IList<TEntity> ExecuteSqlCommandForBaseEntity<TEntity>(string sql, int? timeout = null, params object[] parameters) where TEntity : BaseEntity, new()
        //{

        //	var detectChanges = this.AutoDetectChangesEnabled;
        //	this.AutoDetectChangesEnabled = false;

        //	IList<TEntity> result = null;
        //	try
        //	{
        //		var context = ((IObjectContextAdapter)(this)).ObjectContext;

        //		//var connection = context.Connection;
        //		var connection = this.Database.Connection;
        //		//Don't close the connection after command execution


        //		//open the connection for use
        //		if (connection.State == ConnectionState.Closed)
        //			connection.Open();
        //		//create a command object
        //		using (var cmd = connection.CreateCommand())
        //		{
        //			//command to execute
        //			cmd.CommandText = sql;
        //			cmd.CommandType = CommandType.Text;

        //			// move parameters to command object
        //			this.PrepareDbParameter(cmd, parameters);

        //			//database call
        //			var reader = cmd.ExecuteReader();

        //			result = context.Translate<TEntity>(reader).ToList();
        //			for (int i = 0; i < result.Count; i++)
        //				result[i] = AttachEntityToContext(result[i]);
        //			reader.Close();
        //		}
        //	}
        //	finally
        //	{
        //		this.AutoDetectChangesEnabled = detectChanges;
        //	}

        //	return result;
        //}

        ///// <summary>Executes sql by using SQL-Server Management Objects which supports GO statements.</summary>
        ///// <remarks>codehint: sm-add</remarks>
        //public virtual int ExecuteSqlThroughSmo(string sql)
        //{
        //	Guard.ArgumentNotEmpty(sql, "sql");

        //	var dataSettings = EngineContext.Current.Resolve<DataSettings>();

        //	int result = 0;
        //	bool isSqlServerCompact = dataSettings.DataProvider.IsCaseInsensitiveEqual("sqlce");

        //	if (isSqlServerCompact)
        //	{
        //		result = ExecuteSqlCommand(sql);
        //	}
        //	else
        //	{
        //		using (var sqlConnection = new SqlConnection(dataSettings.DataConnectionString))
        //		{
        //			var serverConnection = new ServerConnection(sqlConnection);
        //			var server = new Server(serverConnection);

        //			result = server.ConnectionContext.ExecuteNonQuery(sql);
        //		}
        //	}
        //	return result;
        //}

        // codehint: sm-add
        /// <summary>
        /// 
        /// </summary>
        public bool HasChanges
        {
            get
            {
                return this.ChangeTracker.Entries()
                           .Where(x => x.State != System.Data.Entity.EntityState.Unchanged && x.State != System.Data.Entity.EntityState.Detached)
                           .Any();
            }
        }

        // codehint: sm-edit (added Hooks)
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            HookedEntityEntry[] modifiedEntries = null;
            bool hooksEnabled = this.HooksEnabled && (_preHooks.Count > 0 || _postHooks.Count > 0);

            if (hooksEnabled)
            {
                modifiedEntries = this.ChangeTracker.Entries()
                                .Where(x => x.State != System.Data.Entity.EntityState.Unchanged && x.State != System.Data.Entity.EntityState.Detached)
                                .Select(x => new HookedEntityEntry()
                                {
                                    Entity = (EntityObject)x.Entity,
                                    PreSaveState = (EntityObjectState)((int)x.State)
                                })
                                .ToArray();

                if (_preHooks.Count > 0)
                {
                    // Regardless of validation (possible fixing validation errors too)
                    ExecutePreActionHooks(modifiedEntries, false);
                }
            }

            if (this.Configuration.ValidateOnSaveEnabled)
            {
                var results = from entry in this.ChangeTracker.Entries()
                              where this.ShouldValidateEntity(entry)
                              let validationResult = entry.GetValidationResult()
                              where !validationResult.IsValid
                              select validationResult;

                if (results.Any())
                {

                    var fail = new DbEntityValidationException(FormatValidationExceptionMessage(results), results);
                    //Debug.WriteLine(fail.Message, fail);
                    throw fail;
                }
            }

            if (hooksEnabled && _preHooks.Count > 0)
            {
                ExecutePreActionHooks(modifiedEntries, true);
            }

            bool validateOnSaveEnabled = this.Configuration.ValidateOnSaveEnabled;

            // SAVE NOW!!!
            this.Configuration.ValidateOnSaveEnabled = false;
            int result = this.Commit();
            this.Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;

            if (hooksEnabled && _postHooks.Count > 0)
            {
                ExecutePostActionHooks(modifiedEntries);
            }

            return result;
        }

        private int Commit()
        {
            int result = 0;
            bool commitFailed = false;
            do
            {
                commitFailed = false;

                try
                {
                    result = base.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    commitFailed = true;

                    foreach (var entry in ex.Entries)
                    {
                        entry.Reload();
                    }
                }
            }
            while (commitFailed);

            return result;
        }

        // codehint: sm-add (required for UoW implementation)
        /// <summary>
        /// 
        /// </summary>
        public string Alias { get; internal set; }

        // codehint: sm-add (performance on bulk inserts)
        /// <summary>
        /// 
        /// </summary>
        public bool AutoDetectChangesEnabled
        {
            get
            {
                return this.Configuration.AutoDetectChangesEnabled;
            }
            set
            {
                this.Configuration.AutoDetectChangesEnabled = value;
            }
        }

        // codehint: sm-add (performance on bulk inserts)
        /// <summary>
        /// 
        /// </summary>
        public bool ValidateOnSaveEnabled
        {
            get
            {
                return this.Configuration.ValidateOnSaveEnabled;
            }
            set
            {
                this.Configuration.ValidateOnSaveEnabled = value;
            }
        }

        // codehint: sm-add
        /// <summary>
        /// 
        /// </summary>
        public bool ProxyCreationEnabled
        {
            get
            {
                return this.Configuration.ProxyCreationEnabled;
            }
            set
            {
                this.Configuration.ProxyCreationEnabled = value;
            }
        }

        #endregion

        #region Utils

        /// <summary>
        /// Attach an entity to the context or return an already attached entity (if it was already attached)
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Attached entity</returns>
        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
        {
            if (entity == null)
            {
                return null;
            }
            //little hack here until Entity Framework really supports stored procedures
            //otherwise, navigation properties of loaded entities are not loaded until an entity is attached to the context
            var alreadyAttached = Set<TEntity>().Local.Where(x => x.Id == entity.Id).FirstOrDefault();
            if (alreadyAttached == null)
            {
                //attach new entity
                Set<TEntity>().Attach(entity);
                return entity;
            }
            else
            {
                //entity is already loaded.
                return alreadyAttached;
            }
        }

        private string FormatValidationExceptionMessage(IEnumerable<DbEntityValidationResult> results)
        {
            var sb = new StringBuilder();
            sb.Append("Entity validation failed" + Environment.NewLine);

            foreach (var res in results)
            {
                var baseEntity = res.Entry.Entity as BaseEntity;
                sb.AppendFormat("Entity Name: {0} - Id: {0} - State: {1}",
                    res.Entry.Entity.GetType().Name,
                    baseEntity != null ? baseEntity.Id.ToString() : "N/A",
                    res.Entry.State.ToString());
                sb.AppendLine();

                foreach (var validationError in res.ValidationErrors)
                {
                    sb.AppendFormat("\tProperty: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }


        public IList<TEntity> ExecuteStoredProcedureForBaseEntity<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            throw new NotImplementedException();
        }

        public IList<TEntity> ExecuteSqlCommandForBaseEntity<TEntity>(string sql, int? timeout = default(int?), params object[] parameters) where TEntity : BaseEntity, new()
        {
            throw new NotImplementedException();
        }

        public TEntity Attatch<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        #endregion





        ///// <summary>
        ///// 附加实体对象
        ///// </summary>
        ///// <typeparam name="TEntity"></typeparam>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public TEntity Attatch<TEntity>(TEntity entity) where TEntity : BaseEntity
        //{
        //	if (entity != null)
        //	{
        //		this.Set<TEntity>().Attach(entity);
        //	}
        //	return entity;
        //}
    }
}
