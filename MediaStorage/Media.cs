using Newtonsoft.Json; // Importing the Json.NET library for handling json files

namespace MediaDTO
{
    /// <summary>
    /// A class representing the media file
    /// </summary>
    public class Media
    {
        //Id Property
        [JsonProperty("MediaId")]
        public int MediaId { get; set; } 

        //Name property
        [JsonProperty("FileName")] //JSON attribute
        public string FileName { get; set; }

        //Filepath property
        [JsonProperty("FilePath")] //JSON attribute
        public string FilePath { get; set; }

        //Format property
        [JsonProperty("Format")] //JSON attribute
        public string Format { get; set; }

        //Playlist id. is null if the media is not in a playlist
        [JsonProperty("ReferencingPlaylistId")]
        public int? PlaylistId { get; set; } = null;
    }
}
