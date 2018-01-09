using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Chenyuan.Components
{
    /// <summary>
    /// Classes implementing this interface provide information about types 
    /// to various services in the Lifenxiang engine.
    /// </summary>
    public interface ITypeFinder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<Assembly> GetAssemblies();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assignTypeFrom"></param>
        /// <param name="ignoreTypes"></param>
        /// <param name="onlyConcreteClasses"></param>
        /// <returns></returns>
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Type> ignoreTypes = null, bool onlyConcreteClasses = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assignTypeFrom"></param>
        /// <param name="assemblies"></param>
        /// <param name="ignoreTypes"></param>
        /// <param name="onlyConcreteClasses"></param>
        /// <returns></returns>
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, IEnumerable<Type> ignoreTypes = null, bool onlyConcreteClasses = true);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ignoreTypes"></param>
        /// <param name="onlyConcreteClasses"></param>
        /// <returns></returns>
        IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Type> ignoreTypes = null, bool onlyConcreteClasses = true);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblies"></param>
        /// <param name="ignoreTypes"></param>
        /// <param name="onlyConcreteClasses"></param>
        /// <returns></returns>
        IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, IEnumerable<Type> ignoreTypes = null, bool onlyConcreteClasses = true);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAssemblyAttribute"></typeparam>
        /// <param name="ignoreTypes"></param>
        /// <param name="onlyConcreteClasses"></param>
        /// <returns></returns>
        IEnumerable<Type> FindClassesOfType<T, TAssemblyAttribute>(IEnumerable<Type> ignoreTypes = null, bool onlyConcreteClasses = true) where TAssemblyAttribute : Attribute;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<Assembly> FindAssembliesWithAttribute<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        IEnumerable<Assembly> FindAssembliesWithAttribute<T>(IEnumerable<Assembly> assemblies);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        IEnumerable<Assembly> FindAssembliesWithAttribute<T>(DirectoryInfo assemblyPath);


    }
}
