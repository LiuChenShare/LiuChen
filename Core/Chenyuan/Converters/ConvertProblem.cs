using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ConvertProblem
    {
        /// <summary>
        /// 
        /// </summary>
        public object Item
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public PropertyInfo Property
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public object AttemptedValue
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Exception Exception
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return
                $@"Item type:     {((Item != null) ? Item.GetType().FullName : "(null)")}
                Property:        {Property.Name}
                Property Type:   {Property.PropertyType}
                Attempted Value: {AttemptedValue}
                Exception:
                {Exception}.";
        }
    }
}
