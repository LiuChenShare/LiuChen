using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data.Base.Entity
{
    public class ChenyuanDatabase : BaseDatabase, IDatabase
    {
        public string DefaultDbName => "Chenyuan-Base";

        public string WriteDbName => "Chenyuan-Base";

        public string ReadDbName => "Chenyuan-Base";
        public string ConnString
        {
            get
            {
                if (string.IsNullOrEmpty(this._connString))
                {
                    string filename = $"{DefaultDbName}Settings.txt";
                    base.LoadConnString(filename);
                    if (string.IsNullOrEmpty(this._connString)) { this._connString = "data source=DESKTOP-D51E9P0;initial catalog=Chenyuan-Base;persist security info=True;user id=sa;password=l88888888;"; }
                }
                return _connString;
            }
        }

        public string MappingsAssembly => "Chenyuan.Data.Base.Mappings";

        public string DataContextName => "Chenyuan.Data.Base";
    }
}
