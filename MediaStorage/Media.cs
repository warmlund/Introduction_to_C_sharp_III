using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Importing the Json.NET library for handling json files

namespace MediaDTO
{
    /// <summary>
    /// A class representing the media file
    /// </summary>
    public class Media
    {
        [Key]
        [JsonProperty("MediaId")]
        public int MediaId { get; set; }

        [Required]
        [JsonProperty("FileName")] // JSON attribute
        public string FileName { get; set; } = null!;

        [Required]
        [JsonProperty("FilePath")] // JSON attribute
        public string FilePath { get; set; } = null!;

        [Required]
        [JsonProperty("Format")] // JSON attribute
        public string Format { get; set; } = null!;

        [ForeignKey(nameof(Playlist))] // Reference Playlist entity
        public string? PlaylistName { get; set; } // Nullable foreign key

        public Playlist? Playlist { get; set; } // Nullable navigation property
    }
}
