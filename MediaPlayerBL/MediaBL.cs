using MediaDTO;
using MediaPlayerDA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MediaPlayerBL
{
    /// <summary>
    /// The class representing the business logic layer
    /// </summary>
    public class MediaBL : IMediaBL
    {

        private PlaylistManager _playlistManager; //instance for handling playlists
        private MediaDA _mediaDA; // instance for data access layer

        /// <summary>
        /// Constructor for the business layer
        /// </summary>
        public MediaBL()
        {
            _mediaDA = new MediaDA();
            _playlistManager = new PlaylistManager();
        }

        /// <summary>
        /// Loads the playlist from a filepath
        /// and returns a list of media
        /// </summary>
        public ICollection<Media> LoadPlaylist(string filepath, bool loadFromDb)
        {
            _playlistManager.LoadPlaylist(_mediaDA, filepath, loadFromDb);

            return _playlistManager.CurrentPlaylist.MediaFiles;
        }

        public ICollection<Playlist> GetPlaylistFromDb()
        {
           return _playlistManager.GetPlaylistsFromDatabase(_mediaDA);
        }

        /// <summary>
        /// A method that loads media calling the method from the DA layer
        /// </summary>
        public ICollection<Media> LoadMedia(string[] filenames, bool loadFromDb)
        {
            if (loadFromDb)
                return _mediaDA.LoadMediaFromDatabase();

            else
                return _mediaDA.LoadMedia(filenames);
        }

        public void SaveMedia(ICollection<Media> media, string PlaylistTitle)
        {
            _mediaDA.SaveMediaToDatabase(media, PlaylistTitle);
        }

        public void RemoveMedia(ICollection<Media> media)
        {
            _mediaDA.RemoveMediaFromDatabase(media);
        }

        /// <summary>
        /// A method saving the playlist
        /// </summary>
        public void SavePlaylist(string title, string filepath, ICollection<Media> loadedMedia)
        {
            _playlistManager.SavePlaylist(title, _mediaDA, filepath, loadedMedia);
        }

        public void SavePlaylistToDatabase(string title, ICollection<Media> loadedMedia)
        {
            _playlistManager.SavePlaylistToDb(title, _mediaDA, loadedMedia);
        }
        /// <summary>
        /// A method retrieving the playlist title
        /// </summary>
        public string GetPlaylistTitle() => _playlistManager.CurrentPlaylist.PlaylistName;

        public void ChangePlaylistTitle(string newName, Playlist playlist)
        {
            _playlistManager.ChangePlaylistTitle(_mediaDA, newName, playlist);
        }

        public void CreateNewPlaylist(string name)
        {
            _playlistManager.CreateNewPlaylist(_mediaDA, name);
        }

        public void RemovePlaylist(string title)
        {
            _playlistManager.RemoveCurrentPlaylist(_mediaDA, title);
        }

        public bool IsPlaylistInDatabase(string name)
        {
            return _playlistManager.IsPlaylistInDatabase(_mediaDA, name);
        }

        public Playlist GetCurrentPlaylist() => _playlistManager.CurrentPlaylist;

        /// <summary>
        /// Checks if the media is an image
        /// </summary>
        public bool IsImageFormat(string format)
        {
            List<string> imageFormats = new List<string> { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".ico" }; //Creates string list of formats
            return imageFormats.Any(s => s.Equals(format, StringComparison.OrdinalIgnoreCase)); //Checks if the format is in the list 
        }

        /// <summary>
        /// Checks if the media is a video
        /// </summary>
        public bool IsVideoFormat(string format)
        {
            List<string> videoFormats = new List<string> { ".mp4", ".wmv", ".avi", ".mpeg", ".mpg", ".asf" }; //Creates string list of formats
            return videoFormats.Any(s => s.Equals(format, StringComparison.OrdinalIgnoreCase)); //Checks if the format is in the list 
        }

        /// <summary>
        /// Method for creating a bitmapimage of the filepath
        /// </summary>
        public BitmapImage CreateImage(string filePath)
        {
            var image = new BitmapImage();
            bool db = true;
            try
            {
                image.BeginInit(); // Begin initialization

                if (db)
                    image.UriSource = new Uri(filePath, UriKind.Relative);

                else
                    image.UriSource = new Uri(filePath, UriKind.Absolute); //Gets source as uri

                image.CacheOption = BitmapCacheOption.OnLoad; // Cache the image
                image.EndInit(); // Ends initialization
                image.Freeze(); // Freeze the image to make it cross-thread accessible
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                image = null;
            }
            return image;
        }

        /// <summary>
        /// A method creating an URI of a file path for a video
        /// </summary>
        public Uri CreateVideo(string filePath)
        {
            Uri path = null;

            try
            {
                if (!Path.IsPathRooted(filePath))
                {
                    path = new Uri(filePath, UriKind.Relative);
                }

                else
                {
                    path = new Uri(filePath, UriKind.Absolute);
                }
            }

            catch
            {
                path = null;
            }

            return path;
        }
    }
}
