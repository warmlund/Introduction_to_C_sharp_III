using MediaDTO;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MediaPlayerDA
{
    public class MediaDA : IMediaDA
    {
        public MediaDA()
        {
            
        }
        /// <summary>
        /// Method for loading media from an array of filenames
        /// </summary>
        public List<Media> LoadMedia(string[] filenames)
        {
            var loadedMedia = new List<Media>(); //creates new list of media

            for (int i = 0; i < filenames.Length; i++) //loops through every string
            {
                //Adds new media to loadedmedia
                loadedMedia.Add(new Media
                {
                    FileName = Path.GetFileName(filenames[i]),
                    FilePath = filenames[i],
                    Format = Path.GetExtension(filenames[i])
                });
            }

            return loadedMedia;
        }


        /// <summary>
        /// Method for loading a playlist
        /// The playlist loaded is in a JSON format
        /// </summary>
        public Playlist LoadPlaylist(string path)
        {
            try
            {
                if (File.Exists(path)) //Checks if file exists
                {
                    string jsonPlaylist = File.ReadAllText(path); //Reads the json file
                    return JsonConvert.DeserializeObject<Playlist>(jsonPlaylist); //returns the deserialized json file as a playlist
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// A method for saving a playlist
        /// From a playlist object to a JSON file
        /// </summary>
        public bool SavePlaylist(string path, string title, List<Media> currentMedia)
        {
            var playlist = new Playlist(); //creates a playlist 
            playlist.PlaylistName = title; //sets the title
            playlist.MediaFiles = currentMedia; //sets the media 

            try
            {
                string jsonPlaylist = JsonConvert.SerializeObject(playlist, Formatting.Indented); //converts to json 
                File.WriteAllText(path, jsonPlaylist); //write to file
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
