using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Chenyuan.Utilities.Threading;

namespace Chenyuan.Assemblies
{
    /// <summary>
    /// 
    /// </summary>
    public class AssemblyInitor
    {
        internal const string c_defaultAssemblySkipLoadingPattern = @"^System|^mscorlib|^Microsoft|^CppCodeProvider|^VJSharpCodeProvider|^WebDev|^Castle|^Iesi|^log4net|^Autofac|^AutoMapper|^dotless|^EntityFramework|^EPPlus|^Fasterflect|^FiftyOne|^nunit|^TestDriven|^MbUnit|^Rhino|^QuickGraph|^TestFu|^Telerik|^Antlr3|^Recaptcha|^FluentValidation|^ImageResizer|^itextsharp|^MiniProfiler|^Newtonsoft|^Pandora|^WebGrease|^DotNetOpenAuth|^Facebook|^LinqToTwitter|^PerceptiveMCAPI|^CookComputing|^GCheckout|^Mono\.Math|^Org\.Mentalis|^App_Web";

        internal const string c_defaultAssemblyRestrictToLoadingPattern = ".*";

        internal static AssemblyInitor Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public AssemblyInitor()
        {
            Instance = this;
        }

        /// <summary>
        /// 
        /// </summary>
        public string AssemblySkipLoadingPattern
        {
            get;
            set;
        } = c_defaultAssemblySkipLoadingPattern;

        /// <summary>
        /// 
        /// </summary>
        public string AssemblyRestrictToLoadingPattern
        {
            get;
            set;
        } = c_defaultAssemblyRestrictToLoadingPattern;

        private static readonly ReaderWriterLockSlim s_Locker = new ReaderWriterLockSlim();
        /// <summary>
        /// Initialize
        /// </summary>
        public void AppInitialize()
        {
            using (s_Locker.GetWriteLock())
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => Matches(a.FullName)).ToArray();
                //AppDomain.CurrentDomain.AssemblyLoad += (s, a) => {
                //    InitAssembly(a.LoadedAssembly)};
                foreach (Assembly assembly in assemblies)
                {
                    InitAssembly(assembly, _attributes);
                }
                this.AssemblyLoaded(true);
            }
        }

        private List<PreApplicationStartMethodAttribute> _attributes = new List<PreApplicationStartMethodAttribute>();

        /// <summary>Check if a dll is one of the shipped dlls that we know don't need to be investigated.</summary>
        /// <param name="assemblyFullName">The name of the assembly to check.</param>
        /// <returns>True if the assembly should be loaded into Lifenxiang.</returns>
        public virtual bool Matches(string assemblyFullName)
        {
            return !Matches(assemblyFullName, AssemblySkipLoadingPattern)
                   && Matches(assemblyFullName, AssemblyRestrictToLoadingPattern);
        }

        /// <summary>Check if a dll is one of the shipped dlls that we know don't need to be investigated.</summary>
        /// <param name="assemblyFullName">The assembly name to match.</param>
        /// <param name="pattern">The regular expression pattern to match against the assembly name.</param>
        /// <returns>True if the pattern matches the assembly name.</returns>
        protected virtual bool Matches(string assemblyFullName, string pattern)
        {
            return Regex.IsMatch(assemblyFullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        internal void AssemblyLoaded(bool beforeEngineContext)
        {
            foreach (var item in _attributes.Where(x => x.BeforeEngineContext == beforeEngineContext))
            {
                item.Method.Invoke(item, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="list"></param>
        protected virtual void InitAssembly(Assembly assembly, List<PreApplicationStartMethodAttribute> list)
        {
            var attributes = assembly.GetCustomAttributes(typeof(PreApplicationStartMethodAttribute), true).Cast<PreApplicationStartMethodAttribute>().ToArray();
            list.AddRange(attributes);
        }
    }
}
