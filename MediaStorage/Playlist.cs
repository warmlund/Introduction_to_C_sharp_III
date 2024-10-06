using Newtonsoft.Json;
using System.Collections.Generic;

namespace MediaDTO
{
    /// <summary>
    /// A class representing the playlist object
    /// </summary>
    public class Playlist
    {
        //Media files property
        [JsonProperty("Media")] //JSON attribute
        public List<Media> MediaFiles { get; set; }

        //Playlist name property
        [JsonProperty("Title")] //JSON attribute
        public string PlaylistName { get; set; }
    }
}
