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
        //Id Property
        [JsonProperty("PlaylistId")]
        [Key]
        public int PlaylistId { get; set; }

        //Media files property
        [JsonProperty("Media")] //JSON attribute
        [InverseProperty("Playlist")]
        public ICollection<Media> MediaFiles { get; set; } = null!;

        //Playlist name property
        [Required]
        [JsonProperty("Title")] //JSON attribute
        public string PlaylistName { get; set; } = null!;
    }
}
