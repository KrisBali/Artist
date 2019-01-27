using System;
using System.Collections.Generic;
using System.Linq;


namespace ArtistBusinessLayer
{

    /// <summary>
    /// Generic list to allow paging of a source element.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagingListClass<T> : List<T>
    {
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int TotalNumSearchResults { get; private set; }
        public int TotalPages { get; private set; }

        public PagingListClass(IQueryable<T> source, int pageIndex, int pageSize)
        {
            PageNumber = pageIndex;
            this.PageSize = pageSize;
            TotalNumSearchResults = source.Count();
            TotalPages = (int)Math.Ceiling(TotalNumSearchResults / (double)this.PageSize);
                       
            if (TotalPages != 0 && PageNumber >= TotalPages) PageNumber = TotalPages;

            // amount to skip.
            int SkipAmount = pageSize * (PageNumber - 1);

            this.AddRange(source.Skip(SkipAmount).Take(pageSize));
        }

    }
}