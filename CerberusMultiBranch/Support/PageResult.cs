using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Support
{
    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public int PageSize { get; set; }

        public int RowCount { get; set; }

        public int PagesCount { get; set; }

        public int CurrentMin { get; set; }

        public int CurrentMax { get; set; }

        public int FirstRowOnPage
        {

            get { return (CurrentPage - 1) * PageSize + 1; }
        }

        public int LastRowOnPage
        {
            get { return Math.Min(CurrentPage * PageSize, RowCount); }
        }

        public string NextClass { get { return this.CurrentPage == PageCount ? "disabled" : string.Empty; } }

        public string PrevClass { get { return  this.CurrentPage == Cons.One ? "disabled" : string.Empty; ; } }
    }

    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IList<T> Results { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }

}