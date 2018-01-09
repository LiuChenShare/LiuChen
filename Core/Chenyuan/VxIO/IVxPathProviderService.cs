using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.VxIO
{
    public interface IVxPathProviderService
    {
        IVxProviderInfo Resolve(string root, string name = null);

        IVxProviderInfo Resolve<TProvider>(string root, string name = null)
            where TProvider : IVxProviderInfo;
    }
}
