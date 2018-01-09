using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.VxIO
{
    public class VxEnviroment : IVxEnviroment
    {
        public VxEnviroment(string root, bool caseSensetive, params char[] separatorChars)
        {
            if (string.IsNullOrWhiteSpace(root))
            {
                throw new ArgumentNullException(nameof(root));
            }
            if (separatorChars.Length == 0)
            {
                throw new ArgumentNullException(nameof(separatorChars));
            }
            this.CaseSensetive = CaseSensetive;
            this.SeparatorChars = separatorChars;
            this.Root = this.VerifyRoot(root);
        }

        protected virtual string VerifyRoot(string root)
        {
            return root;
        }

        public ArraySegment<string> SegPath(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            path = path.Trim();
            if (path == "")
            {
                return new ArraySegment<string>(new string[0]);
            }
            var parts = path.Split(this.SeparatorChars.ToArray());
            List<string> list = new List<string>();
            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part))
                {
                    continue;
                }
                list.Add(part.Trim());
            }
            return new ArraySegment<string>(list.ToArray());
        }

        public string Root
        {
            get;
        }

        public virtual bool CaseSensetive
        {
            get;
        }

        public virtual char SeparatorChar => this.SeparatorChars.First();

        public IEnumerable<char> SeparatorChars
        {
            get;
        }

        public string MapPath(VxProviderInfo provider, string path)
        {
            return $"{provider}{SeparatorChar}{path}";
        }

        string IVxEnviroment.MapPath(IVxProviderInfo provider, string path) => this.MapPath(provider as VxProviderInfo, path);
    }
}
