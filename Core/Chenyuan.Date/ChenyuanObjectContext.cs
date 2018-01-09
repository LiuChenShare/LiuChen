using Chenyuan.Extensions;
using Chenyuan.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Chenyuan.Date
{
    /// <summary>
	/// Object context
	/// </summary>
	public abstract class ChenyuanObjectContext : ObjectContextBase
    {
        #region 自动构造过程

        private static IDictionary<Type, Func<string, object>> s_dbContextCreator;
        private static object s_initLocker = new object();
        //private static IDictionary<int, object> _dbContexts;
        static ChenyuanObjectContext()
        {
            s_dbContextCreator = new Dictionary<Type, Func<string, object>>();
            //_dbContexts = new Dictionary<int, object>();
        }

        static Func<string, object> GetCreator(Type type)
        {
            if (s_dbContextCreator.ContainsKey(type))
            {
                return s_dbContextCreator[type];
            }
            lock (s_initLocker)
            {
                if (s_dbContextCreator.ContainsKey(type))
                {
                    return s_dbContextCreator[type];
                }
                var paramExp = Expression.Parameter(typeof(string), "nameOrConnectionString");
                var constructor = type.GetConstructor(new Type[] { typeof(string) });
                if (constructor == null)
                {
                    return null;
                }
                var callExpression = Expression.New(constructor, paramExp);
                var @delegate = Expression.Lambda<Func<string, object>>(callExpression, new ParameterExpression[] { paramExp }).Compile();
                s_dbContextCreator.Add(type, @delegate);
                return @delegate;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObjectContext"></typeparam>
        /// <param name="nameOrConnectionString"></param>
        /// <returns></returns>
        public static TObjectContext Create<TObjectContext>(string nameOrConnectionString)
            where TObjectContext : ChenyuanObjectContext
        {
            return CreateInternal(typeof(TObjectContext), nameOrConnectionString) as TObjectContext;
            ////先从线程中读取可用连接
            //var key = Thread.CurrentThread.ManagedThreadId;
            //TObjectContext context = null;
            //if (_dbContexts.Keys.Contains(key))
            //{
            //    context = _dbContexts[key] as TObjectContext;
            //    if (context == null) _dbContexts.Remove(key);
            //}
            //if (context == null)
            //{
            //    context = CreateInternal(typeof(TObjectContext), nameOrConnectionString) as TObjectContext;
            //    _dbContexts.Add(key, context);
            //}
            //return context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextType"></param>
        /// <param name="nameOrConnectionString"></param>
        /// <returns></returns>
        public static object Create(Type contextType, string nameOrConnectionString)
        {
            Guard.TypeArgumentAssignedTo<ChenyuanObjectContext>(() => contextType);
            return CreateInternal(contextType, nameOrConnectionString);
            ////先从线程中读取可用连接
            //var key = System.Threading.Thread.CurrentThread.ManagedThreadId;
            //if(_dbContexts.Keys.Contains(key))
            //{
            //    return _dbContexts[key];
            //}
            //else
            //{
            //    var context = CreateInternal(contextType, nameOrConnectionString);
            //    _dbContexts.Add(key, context);
            //    return context;
            //}
        }

        private static object CreateInternal(Type contextType, string nameOrConnectionString)
        {
            var func = GetCreator(contextType);
            if (func == null)
            {
                throw new ArgumentOutOfRangeException("contextType", "类型 {0} 必须具备带一个类型为字符串的参数的构造函数。".FormatWith(contextType.FullName));
            }
            return func(nameOrConnectionString);
        }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nameOrConnectionString">连接字符串或连接名称</param>
        public ChenyuanObjectContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public static string ReadConnectionString(string filename)
        {
            string connStr = null;
            if (string.IsNullOrEmpty(connStr))
            {
                string path = "~/App_Data/";
                try
                {
                    path = HttpContext.Current.Server.MapPath(path);
                }
                catch
                {
                    string baseDirectory = AppContext.BaseDirectory;
                    path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
                    path = Path.Combine(baseDirectory, path);
                }
                string filePath = Path.Combine(path, filename);

                if (File.Exists(filePath))
                {
                    string text = File.ReadAllText(filePath);
                    StringReader sr = new StringReader(text);
                    string provider = sr.ReadLine();
                    string connectionString = sr.ReadLine();
                    var arr = connectionString.Split(':');
                    if (arr[0] == "DataConnectionString" && arr.Length == 2)
                    {
                        connStr = arr[1];
                    }
                }
            }

            return connStr;
        }

        /// <summary>
        /// 获取指定模块下的所有映射类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="namespace">类型名称空间</param>
        /// <param name="mustGenericType">是否只针对基类为泛类型过滤，默认为是</param>
        /// <returns></returns>
        protected IEnumerable<Type> GetTypesToRegister(Assembly assembly, string @namespace = null, bool mustGenericType = true)
        {
            var typesToRegister = from t in assembly.GetTypes()
                                  where t.Namespace.HasValue() &&
                                        t.BaseType != null &&
                                        (!mustGenericType || t.BaseType.IsGenericType) &&
                                        (@namespace.IsEmpty() || t.Namespace == @namespace) &&
                                        (t.IsSubOfGenericClass(typeof(EntityTypeConfiguration<>)) || t.IsSubOfGenericClass(typeof(ComplexTypeConfiguration<>))) &&
                                        !t.IsAbstract &&
                                        !t.IsInterface &&
                                        t.IsClass
                                  select t;
            return typesToRegister.ToList();
        }

        /// <summary>
        /// 获取待注册模型映射类型集合
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 默认获取当前实例类型定义模块下的所有实体类型映射类，可在派生类中处理
        /// </remarks>
        protected virtual IEnumerable<Type> GetTypesToRegister()
        {
            Assembly assembly = this.GetType().Assembly;//Assembly.GetExecutingAssembly()
            return GetTypesToRegister(assembly);
        }

        /// <summary>
        /// 附加模型注册处理函数，默认调用
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected virtual void RegisterOtherModel(DbModelBuilder modelBuilder)
        {

        }

        /// <summary>
        /// 自动模型创建处理函数，默认调用
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected virtual void RegisterModel(DbModelBuilder modelBuilder)
        {
            var typesToRegister = this.GetTypesToRegister();

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

        }

        /// <summary>
        /// 模型创建处理函数
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //解决EF动态建库数据库表名变为复数问题  
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            // codehint: sm-edit
            this.RegisterModel(modelBuilder);
            this.RegisterOtherModel(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}
