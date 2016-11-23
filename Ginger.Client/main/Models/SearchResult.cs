using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Client.Models
{
    [JsonObject("search_result")]
    public class SearchResult
    {
        [JsonProperty("hitList")]
        public List<Hit> HitList { get; set; }

        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
    }
}
