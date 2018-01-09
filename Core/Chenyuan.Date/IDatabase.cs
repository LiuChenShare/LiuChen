using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Date
{
    /// <summary>
    /// data base stander interface
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// default data base name
        /// </summary>
        string DefaultDbName { get; }
        /// <summary>
        /// write data base name
        /// </summary>
        string WriteDbName { get; }
        /// <summary>
        /// read data base name
        /// </summary>
        string ReadDbName { get; }

        string ConnString { get; }

        string MappingsAssembly { get; }

        string DataContextName { get; }
    }
}
