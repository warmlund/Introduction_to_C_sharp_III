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
        // Id Property
        [Key]
        [JsonProperty("MediaId")]
        public int MediaId { get; set; }

        // Name property
        [Required]
        [JsonProperty("FileName")] // JSON attribute
        public string FileName { get; set; } = null!;

        // Filepath property
        [Required]
        [JsonProperty("FilePath")] // JSON attribute
        public string FilePath { get; set; } = null!;

        // Format property
        [Required]
        [JsonProperty("Format")] // JSON attribute
        public string Format { get; set; } = null!;

        // Foreign key to Playlist
        [ForeignKey(nameof(Playlist))] // Reference Playlist entity
        public string? PlaylistName { get; set; } // Foreign key, nullable if media is not part of any playlist

        // Navigation property
        public Playlist? Playlist { get; set; } // Navigation property pointing to Playlist
    }
}
