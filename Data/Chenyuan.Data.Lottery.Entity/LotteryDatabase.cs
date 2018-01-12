using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data.Lottery.Entity
{
    public class LotteryDatabase : BaseDatabase, IDatabase
    {
        public string DefaultDbName => "Chenyuan-Lottery";

        public string WriteDbName => "Chenyuan-Lottery";

        public string ReadDbName => "Chenyuan-Lottery";
        public string ConnString
        {
            get
            {
                if (string.IsNullOrEmpty(this._connString))
                {
                    string filename = $"{DefaultDbName}Settings.txt";
                    base.LoadConnString(filename);
                    if (string.IsNullOrEmpty(this._connString)) { this._connString = "data source=DESKTOP-D51E9P0;initial catalog=Chenyuan-Lottery;persist security info=True;user id=sa;password=l88888888;"; }
                }
                return _connString;
            }
        }

        public string MappingsAssembly => "Chenyuan.Data.Lottery.Mappings";

        public string DataContextName => "Chenyuan.Data.Lottery";
    }
}
