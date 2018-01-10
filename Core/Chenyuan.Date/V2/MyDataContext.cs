using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Extensions;

namespace Chenyuan.Data.V2
{
    public class MyDataContext : DbContext
    {
        /// <summary>
        /// database entity mapping assembly
        /// </summary>
        private readonly string _mappingAssembly;
        public MyDataContext(IDatabase database) : base(database.ConnString)
        {
            _mappingAssembly = database.MappingsAssembly;
        }

        public MyDataContext(string nameOrConnectionString, string mappingAssembly) : base(nameOrConnectionString)
        {
            _mappingAssembly = mappingAssembly;
        }

        /// <summary>
        /// struct func
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            this.RegisterModel(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 实体创建处理函数
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected virtual void RegisterModel(DbModelBuilder modelBuilder)
        {
            IEnumerable<Type> typesToRegister = this.GetTypesToRegister();
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
        }

        /// <summary>
        /// 获取当前database下实体mapping,注册模型
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<Type> GetTypesToRegister()
        {
            var assembly = Assembly.Load(_mappingAssembly);
            var typesToRegister = from t in assembly.GetTypes()
                                  where !t.IsAbstract
                                  && !t.IsInterface
                                  && t.IsClass
                                  select t;
            return typesToRegister.ToList();
        }
        
    }
}
