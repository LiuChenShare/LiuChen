using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data.HR
{
    /// <summary>
    /// Code First 数据迁移
    /// </summary>
    public class DbContextFactory : IDbContextFactory<HRDataContext>
    {
        private static Dictionary<string, string> connStr = new Dictionary<string, string>
        {
            { "Chenyuan-hr", "data source=DESKTOP-D51E9P0;initial catalog=Chenyuan-hr;persist security info=True;user id=sa;password=l88888888;"},
        };

        private static Dictionary<string, string> mappingAssembly = new Dictionary<string, string>
        {
            {"Chenyuan-hr","Chenyuan.Data.HR.Mappings" },
        };
        public HRDataContext Create()
        {
            return new HRDataContext(connStr["Chenyuan-hr"], mappingAssembly["Chenyuan-hr"]);
        }
    }
}
