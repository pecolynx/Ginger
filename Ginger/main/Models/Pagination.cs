using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Models
{
    internal class Pagination
    {
        public Pagination(int totalCount, int countPerPage)
        {
            this.TotalCount = totalCount;
            this.CountPerPage = countPerPage;
            if (this.TotalCount == 0)
            {
                this.FirstPage = 0;
                this.LastPage = 0;
                this.Visibility = false;
            }
            else if (this.TotalCount % this.CountPerPage == 0)
            {
                this.FirstPage = 1;
                this.LastPage = this.TotalCount / this.CountPerPage;
                this.Visibility = true;
            }
            else
            {
                this.FirstPage = 1;
                this.LastPage = (this.TotalCount / this.CountPerPage) + 1;
                this.Visibility = true;
            }
        }

        public int TotalCount { get; }

        public int CountPerPage { get; }

        public int FirstPage { get; }

        public int LastPage { get; }

        public bool Visibility { get; }

        public List<Page> GetPageList(int pageNumber)
        {
            var list = new List<Page>();
            var beginPage = pageNumber - 3;
            if (beginPage < this.FirstPage)
            {
                beginPage = this.FirstPage;
            }

            var endPage = pageNumber + 3;
            if (endPage > this.LastPage)
            {
                endPage = this.LastPage;
            }

            var prevPage = pageNumber - 1;
            if (prevPage < this.FirstPage)
            {
                prevPage = this.FirstPage;
            }

            var nextPage = pageNumber + 1;
            if (nextPage > this.LastPage)
            {
                nextPage = this.LastPage;
            }

            list.Add(new Page(this.FirstPage, "<<", pageNumber != this.FirstPage));
            list.Add(new Page(prevPage, "<", pageNumber != prevPage));

            for (var i = beginPage; i <= endPage; i++)
            {
                list.Add(new Page(i, i.ToString(), pageNumber != i));
            }

            list.Add(new Page(nextPage, ">", pageNumber != nextPage));
            list.Add(new Page(this.LastPage, ">>", pageNumber != this.LastPage));

            return list;
        }
    }
}
