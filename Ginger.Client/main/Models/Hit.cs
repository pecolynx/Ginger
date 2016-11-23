using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Client.Models
{
    [JsonObject("hit")]
    public class Hit
    {
        [JsonProperty("document")]
        public DocumentFile Document { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("highlight")]
        public string Highlight { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }
    }
}
