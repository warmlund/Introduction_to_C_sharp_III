using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MediaStorage
{
    public class Media
    {
        [JsonProperty("FileName")]
        public string FileName { get; set; }

        [JsonProperty("FilePath")]
        public string FilePath {  get; set; }

        [JsonProperty("Format")]
        public string Format { get; set; }
    }
}
