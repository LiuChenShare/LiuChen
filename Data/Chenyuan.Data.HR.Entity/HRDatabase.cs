using Chenyuan.Date;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data.HR.Entity
{
    public class HRDatabase : BaseDatabase, IDatabase
    {
        public string DefaultDbName => "Chenyuan-hr";

        public string WriteDbName => "Chenyuan-hr";

        public string ReadDbName => "Chenyuan-hr";
        public string ConnString
        {
            get
            {
                if (string.IsNullOrEmpty(this._connString))
                {
                    string filename = $"{DefaultDbName}Settings.txt";
                    base.LoadConnString(filename);
                    if (string.IsNullOrEmpty(this._connString)) { this._connString = "data source=DESKTOP-D51E9P0;initial catalog=Chenyuan-hr;persist security info=True;user id=sa;password=l88888888;"; }
                }
                return _connString;
            }
        }

        public string MappingsAssembly => "Chenyuan.Data.HR.Mappings";

        public string DataContextName => "Chenyuan.Data.HR";
    }
}
