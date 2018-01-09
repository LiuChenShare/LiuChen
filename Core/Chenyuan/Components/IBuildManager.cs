using System;
using System.Collections;
using System.IO;

namespace Chenyuan.Components
{
    public interface IBuildManager
	{
		bool FileExists(string virtualPath);
		Type GetCompiledType(string virtualPath);
		ICollection GetReferencedAssemblies();
		Stream ReadCachedFile(string fileName);
		Stream CreateCachedFile(string fileName);
	}
}
