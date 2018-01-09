using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class DictionaryConvertException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="problems"></param>
        public DictionaryConvertException(ICollection<ConvertProblem> problems)
            : this(CreateMessage(problems), problems)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="problems"></param>
        public DictionaryConvertException(string message, ICollection<ConvertProblem> problems)
            : base(message)
        {
            Problems = problems;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public DictionaryConvertException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<ConvertProblem> Problems
        {
            get; private set;
        }

        private static string CreateMessage(ICollection<ConvertProblem> problems)
        {
            var counter = 0;
            var builder = new StringBuilder();
            builder.Append("Could not convert all input values into their expected types:");
            builder.Append(Environment.NewLine);
            foreach (var prob in problems)
            {
                builder.AppendFormat("-----Problem[{0}]---------------------", counter++);
                builder.Append(Environment.NewLine);
                builder.Append(prob);
                builder.Append(Environment.NewLine);
            }
            return builder.ToString();
        }
    }
}
