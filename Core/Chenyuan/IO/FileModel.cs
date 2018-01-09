using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.IO
{
    public class FileModel
    {
        public long From { get; set; }
        public long To { get; set; }
        public bool IsPartial { get; set; }
        public long Length { get; set; }
    }
}
