using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShopping.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        public int NPages { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; }
        public int Category { get; set; }

        public PaginatedList(IQueryable<T> source, int pageIndex, int pageSize, int nPages, string searhTerm = null, int category = 0)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            NPages = nPages;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);
            SearchTerm = searhTerm;
            Category = category;
            AddRange(source.Skip(PageIndex * PageSize).Take(PageSize));
        }

        public bool HasPreviousPage
        {
            get { return PageIndex > 0; }
        }

        public bool HasNextPage
        {
            get { return PageIndex + 1 < TotalPages; }
        }
    }
}