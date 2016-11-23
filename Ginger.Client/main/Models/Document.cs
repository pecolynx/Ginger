using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Client.Models
{
    [JsonObject("document")]
    public class Document
    {
        public Document()
        {
        }

        public Document(string id)
        {
            this.Id = id;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty("indexName")]
        public string IndexName { get; set; }

        [JsonProperty("mappingName")]
        public string MappingName { get; set; }

        [JsonProperty("documentFieldList")]
        public List<DocumentField> DocumentFieldList { get; set; }
    }
}
