using MediaDTO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace MediaPlayerDA
{
    public class MediaDA : IMediaDA
    {
        DatabaseManager _databaseManager;
        public MediaDA()
        {
            _databaseManager = new DatabaseManager();
        }
        /// <summary>
        /// Method for loading media from an array of filenames
        /// </summary>
        public ICollection<Media> LoadMedia(string[] filenames)
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
        public bool SavePlaylist(string path, string title, ICollection<Media> currentMedia)
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

        public ICollection<Media> LoadMediaFromDatabase()
        {
            return _databaseManager.LoadMediaFromDb();
        }

        public Playlist LoadPlaylistFromDatabase(string name)
        {
            return _databaseManager.LoadPlaylistFormDb(name);
        }

        public void SaveMediaToDatabase(ICollection<Media> currentMedia, string PlaylistTitle)
        {
            _databaseManager.SaveMediaToDb(currentMedia, PlaylistTitle);
        }

        public void SavePlaylistToDatabase(string title, ICollection<Media> currentMedia)
        {
            _databaseManager.SavePlaylistToDb(title, currentMedia);
        }

        public void RemoveMediaFromDatabase(ICollection<Media> media)
        {
            _databaseManager.RemoveMediaFromDb(media);
        }

        public void RemovePlaylistFromDatabase(string title)
        {
            _databaseManager.RemovePlaylistFromDb(title);
        }

        public void ChangePlaylistTitle(string newName, Playlist playlist)
        {
            _databaseManager.ChangePlaylistTitle(newName, playlist);
        }

        public void CreateNewPlaylist(string name)
        {
            _databaseManager.CreateNewPlaylist(name);
        }

        public bool IsPlaylistInDatabase(string name)
        {
           return _databaseManager.IsPlaylistInDatabase(name);
        }
    }
}
