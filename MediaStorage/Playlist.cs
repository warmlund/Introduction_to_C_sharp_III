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
        [Key]
        [JsonProperty("Title")] // JSON attribute
        public string PlaylistName { get; set; } = null!; // Set to non-nullable to ensure consistency

        [JsonProperty("Media")] // JSON attribute
        [InverseProperty("Playlist")]
        public ICollection<Media> MediaFiles { get; set; } = new List<Media>(); // Initialize to avoid null references
    }
}
