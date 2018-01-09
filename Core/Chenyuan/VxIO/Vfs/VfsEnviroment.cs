using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.VxIO.Vfs
{
    public class VfsEnviroment : VxEnviroment
    {
        public VfsEnviroment(string root)
            : base(root, false, '/', '\\')
        {
        }

        protected override string VerifyRoot(string root)
        {
            if (root.EndsWith(":"))
            {
                root += "\\";
            }
            return root;
        }

    }
}
