using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations; // Importing the Json.NET library for handling json files

namespace MediaDTO
{
    /// <summary>
    /// A class representing the media file
    /// </summary>
    public class Media
    {
        //Id Property
        [Required]
        [JsonProperty("MediaId")]
        public int MediaId { get; set; }

        //Name property
        [Required]
        [JsonProperty("FileName")] //JSON attribute
        public string FileName { get; set; } = null!;

        //Filepath property
        [Required]
        [JsonProperty("FilePath")] //JSON attribute
        public string FilePath { get; set; } = null!;

        //Format property
        [Required]
        [JsonProperty("Format")] //JSON attribute
        public string Format { get; set; } = null!;

        //Playlist id. is null if the media is not in a playlist
        [JsonProperty("ReferencingPlaylistId")]
        public int? PlaylistId { get; set; }

        public Playlist? Playlist { get; set; }
    }
}
