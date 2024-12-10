using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperUtils
{
    public class PagedList<T> where T : class
    {
        public PagedList(IEnumerable<T> list, int totalRecords, int pageNumber, int pageSize)
        {
            List = list;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = (int)Math.Ceiling(TotalRecords / (double)pageSize);
        }

        public int PageNumber { get; }
        public int PageSize { get; }
        public IEnumerable<T> List { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
