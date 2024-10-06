using Newtonsoft.Json; // Importing the Json.NET library for handling json files

namespace MediaDTO
{
    /// <summary>
    /// A class representing the media file
    /// </summary>
    public class Media
    {
        //Name property
        [JsonProperty("FileName")] //JSON attribute
        public string FileName { get; set; }

        //Filepath property
        [JsonProperty("FilePath")] //JSON attribute
        public string FilePath { get; set; }

        //Format property
        [JsonProperty("Format")] //JSON attribute
        public string Format { get; set; }

    }
}
