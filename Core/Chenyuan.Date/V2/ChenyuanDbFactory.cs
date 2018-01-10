using Autofac;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Data.Entity;
using Chenyuan.Date.V2;

namespace Chenyuan.Data.V2
{
    public class ChenyuanDbFactory
    {
        public static DataContext GetDataContext(IComponentContext componentContext)
        {
            //Type entityType = typeof(T);
            //var dbType = entityType.Assembly.GetTypes().Where(x => x.GetInterface(typeof(IDatabase).Name) != null).SingleOrDefault();
            //if(dbType == null)
            //{
            //    throw new Exception($"Can't find class inherit of IDatabase in {entityType.Name} Entity Library ");
            //}
            //if(componentContext.IsRegisteredWithKey(dbType, typeof(DataContext)))
            //{
            //    return componentContext.ResolveKeyed(dbType, typeof(DataContext)) as DataContext;
            //}

            return null;
            //var db = Activator.CreateInstance(dbType) as IDatabase;
            //var dbKey = _databasePool.Keys.FirstOrDefault(x => x.ConnString == db.ConnString);
            //if (dbKey == null)
            //{
            //var dataContextType = Assembly.Load(db.DataContextName).GetTypes().Where(x => x.BaseType == typeof(DataContext)).FirstOrDefault(); //Type.GetType(db.DataContextName);
            //var dataContext = Activator.CreateInstance(dataContextType, db.ConnString, db.MappingsAssembly) as DataContext;
            //_databasePool.TryAdd(db, dataContext); //new DataContext(db.ConnString, db.MappingsAssembly)
            //dbKey = db;
            //}
            //return _databasePool[dbKey];
            //var dataContextType = Assembly.Load(db.DataContextName).GetTypes().Where(x => x.BaseType == typeof(DataContext)).FirstOrDefault(); //Type.GetType(db.DataContextName);
            //var dataContext = Activator.CreateInstance(dataContextType, db.ConnString, db.MappingsAssembly) as DataContext;

            //return dataContext;
        }
    }
}
