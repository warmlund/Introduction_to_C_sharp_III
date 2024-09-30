using MediaDTO;
using MediaPlayerDA;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Windows.Media;

namespace MediaPlayerBL
{
    public class MediaBL : IMediaBL
    {
        private PlaylistManager _playlistManager;
        private MediaDA _mediaDA;

        public MediaBL()
        {
            _mediaDA = new MediaDA();
            _playlistManager = new PlaylistManager();
        }
        public List<Media> LoadPlaylist(string filepath)
        {
            _playlistManager.LoadPlaylist(_mediaDA, filepath);
            return _playlistManager.CurrentPlaylist.MediaFiles;
        }

        public List<Media> LoadMedia(string[] filenames)
        {
            return _mediaDA.LoadMedia(filenames);
        }

        public void SavePlaylist(string filepath, List<Media> loadedMedia) => _playlistManager.SavePlaylist(_mediaDA, filepath, loadedMedia);

        public string GetPlaylistTitle() => _playlistManager.CurrentPlaylist.PlaylistName;

        public bool IsImageFormat(string format)
        {
            List<string> imageFormats = new List<string> { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".ico" };
            return imageFormats.Any(s => s.Equals(format, StringComparison.OrdinalIgnoreCase));
        }

        public bool IsVideoFormat(string format)
        {
            List<string> videoFormats = new List<string> { ".mp4", ".wmv", ".avi", ".mpeg", ".mpg", ".asf" };
            return videoFormats.Any(s => s.Equals(format, StringComparison.OrdinalIgnoreCase));
        }

        public BitmapImage CreateImage(string filePath)
        {
            var image = new BitmapImage();
            try
            {
                image.BeginInit();
                image.UriSource = new Uri(filePath, UriKind.Absolute);
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                image.Freeze();
            }
            catch 
            { 
                image = null;
            }
            return image;
        }

        public Uri CreateVideo(string filePath)
        {
            return new Uri(filePath, UriKind.RelativeOrAbsolute);
        }
    }
}
