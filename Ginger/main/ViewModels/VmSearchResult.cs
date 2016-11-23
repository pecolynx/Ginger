using Ginger.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.ViewModels
{
    internal class VmSearchResult
    {
        public VmSearchResult(SearchResult searchResult)
        {
            this.HitList = searchResult.HitList.Select(x => new VmHit(x)).ToList();
            this.TotalCount = searchResult.TotalCount;
        }

        public List<VmHit> HitList { get; set; }

        public int TotalCount { get; set; }
    }
}
