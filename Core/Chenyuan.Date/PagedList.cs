using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Date
{
    public class PagedList<T> : IPagedList<T>
    {

        public PagedList()
        {
            Data = new List<T>();
            PageIndex = 1;
            PageSize = 20;
        }

        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            Init(source.Skip((pageIndex - 1) * pageSize).Take(pageSize), pageIndex, pageSize, source.Count());
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IList<T> source, int pageIndex, int pageSize)
        {
            // codehint: sm-edit
            if (source == null) { throw new ArgumentNullException("source"); }
            Init(source.Skip((pageIndex - 1) * pageSize).Take(pageSize), pageIndex, pageSize, source.Count);
        }

        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            Init(source.Skip((pageIndex - 1) * pageSize).Take(pageSize), pageIndex, pageSize, source.Count());
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totaChenyuanount">Total count</param>
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totaChenyuanount)
        {
            // codehint: sm-edit
            if (source == null) { throw new ArgumentNullException("source"); }
            Init(source, pageIndex, pageSize, totaChenyuanount);
        }

        // codehint: sm-add
        private void Init(IEnumerable<T> source, int pageIndex, int pageSize, int totaChenyuanount)
        {
            if (source == null) { throw new ArgumentNullException("source"); }

            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotaChenyuanount = totaChenyuanount;

            this.Data = new List<T>();
            this.Data.AddRange(source);
        }

        #region IPageable Members

        // codehint: sm-add/edit
        /// <summary>
        /// 
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int TotaChenyuanount
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int TotalPages
        {
            get
            {
                var total = this.TotaChenyuanount / this.PageSize;

                if (this.TotaChenyuanount % this.PageSize > 0)
                    total++;

                return total;
            }
        }


        #endregion

        public List<T> Data { get; set; }

    }
}
