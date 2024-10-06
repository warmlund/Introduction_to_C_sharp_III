using MediaDTO;
using MediaPlayerDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace MediaPlayerBL
{
    /// <summary>
    /// The class representing the business logic layer
    /// </summary>
    public class MediaBL : IMediaBL
    {

        private PlaylistManager _playlistManager; //instanc for handling playlists
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
        public List<Media> LoadPlaylist(string filepath)
        {
            _playlistManager.LoadPlaylist(_mediaDA, filepath);
            return _playlistManager.CurrentPlaylist.MediaFiles;
        }

        /// <summary>
        /// A method that loads media calling the method from the DA layer
        /// </summary>
        public List<Media> LoadMedia(string[] filenames)
        {
            return _mediaDA.LoadMedia(filenames);
        }

        /// <summary>
        /// A method saving the playlist
        /// </summary>
        public void SavePlaylist(string filepath, List<Media> loadedMedia) => _playlistManager.SavePlaylist(_mediaDA, filepath, loadedMedia);

        /// <summary>
        /// A method retrieving the playlist title
        /// </summary>
        public string GetPlaylistTitle() => _playlistManager.CurrentPlaylist.PlaylistName;

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
            try
            {
                image.BeginInit(); // Begin initialization
                image.UriSource = new Uri(filePath, UriKind.Absolute); //Gets source as uri
                image.CacheOption = BitmapCacheOption.OnLoad; // Cache the image
                image.EndInit(); // Ends initialization
                image.Freeze(); // Freeze the image to make it cross-thread accessible
            }
            catch
            {
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
            
            if(System.IO.File.Exists(filePath))
                path = new Uri(filePath, UriKind.RelativeOrAbsolute);

            return path;    
        }
    }
}
