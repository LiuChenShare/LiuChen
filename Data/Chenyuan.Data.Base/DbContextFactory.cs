using Chenyuan.Data.Base.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data.Base
{
    /// <summary>
    /// Code First 数据迁移
    /// </summary>
    public class DbContextFactory : IDbContextFactory<BaseDataContext>
    {
        private static Dictionary<string, string> connStr = new Dictionary<string, string>
        {
            { "Chenyuan-Base", "data source=DESKTOP-D51E9P0;initial catalog=Chenyuan-Base;persist security info=True;user id=sa;password=l88888888;"},
        };

        private static Dictionary<string, string> mappingAssembly = new Dictionary<string, string>
        {
            {"Chenyuan-Base","Chenyuan.Data.HR.Mappings" },
        };
        public BaseDataContext Create()
        {
            return new BaseDataContext(connStr["Chenyuan-Base"], mappingAssembly["Chenyuan-Base"]);
        }
    }
}
