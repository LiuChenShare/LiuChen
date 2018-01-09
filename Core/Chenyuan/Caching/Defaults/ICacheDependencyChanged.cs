using System;

namespace Chenyuan.Caching.Defaults
{
    internal interface ICacheDependencyChanged
    {
        void DependencyChanged(object sender, EventArgs e);
    }
}
