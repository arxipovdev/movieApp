using System;
using System.Collections.Generic;

namespace Web.ViewModels
{
    public class MovieIndexViewModel
    {
        public int NumberPage { get; set; }
        public int TotalPage { get; set; }
        public int Count { get; set; }
        public IEnumerable<MovieViewModel> Data { get; set; }

        public bool IsPreviousPage => NumberPage > 1;
        public bool IsNextPage => NumberPage < TotalPage;
        public MovieIndexViewModel(int numberPage, int sizePage, int count, IEnumerable<MovieViewModel> data)
        {
            NumberPage = numberPage;
            TotalPage = (int)Math.Ceiling(count / (double) sizePage);
            Count = count;
            Data = data;
        }
    }
}