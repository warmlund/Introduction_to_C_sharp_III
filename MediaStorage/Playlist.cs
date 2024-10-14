using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaDTO
{
    /// <summary>
    /// A class representing the playlist object
    /// </summary>
    public class Playlist
    {
        //Playlist name property
        [Key]
        [JsonProperty("Title")] //JSON attribute
        public string PlaylistName { get; set; }

        //Media files property used for loading a playlist as a json file and not from database
        [JsonProperty("Media")] //JSON attribute
        [InverseProperty("Playlist")]
        public ICollection<Media> MediaFiles { get; set; } = null!;
    }
}
