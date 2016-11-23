using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Client.Models
{
    [JsonObject("document")]
    public class DocumentField
    {
        [JsonProperty("fieldName")]
        public string FieldName { get; set; }

        [JsonProperty("fielPath")]
        public string FieldPath { get; set; }

        [JsonProperty("fieldValueList")]
        public List<string> FieldValueList { get; set; }

        [JsonProperty("fieldType")]
        public int FieldType { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
