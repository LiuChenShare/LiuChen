using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data
{
    public interface IPagedList<T>
    {
        /// <summary>
        /// The 0-based current page index
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// The number of items in each page.
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// The total number of items.
        /// </summary>
        int TotalCount { get; set; }

        /// <summary>
        /// The total number of pages.
        /// </summary>
        int TotalPages { get; }

        List<T> Data { get; set; }
    }
}
