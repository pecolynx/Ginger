using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Common.Models
{
    public class ModelId
    {
        public ModelId()
        {
        }

        public ModelId(string value)
        {
            this.Value = value;
        }

        public string Value { get; set; }
    }
}
