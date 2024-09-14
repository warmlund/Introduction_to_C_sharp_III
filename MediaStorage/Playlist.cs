using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaStorage
{
    public class Playlist
    {
        [JsonProperty("Media")]
        public List<Media> MediaFiles { get; set; }

        [JsonProperty("Title")]
        public string PlaylistName { get; set; }
    }
}
