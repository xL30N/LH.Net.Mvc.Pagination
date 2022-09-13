using System.Collections.Generic;
using System;

namespace LH.Net.Mvc.Pagination
{
    public class PaginatedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalItems { get; private set; }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(TotalItems / (double)PageSize);
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return CurrentPage > 1;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return CurrentPage < TotalPages;
            }
        }

        public int PreviousPage
        {
            get
            {
                return CurrentPage - 1;
            }
        }

        public int NextPage
        {
            get
            {
                return CurrentPage + 1;
            }
        }
        public int StartIndex
        {
            get
            {
                return (CurrentPage - 1) * PageSize;
            }
        }

        public PaginatedList(int totalItems, int currentPage, int pageSize)
        {
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
    }
}