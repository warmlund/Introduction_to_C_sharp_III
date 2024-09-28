using MediaDTO;
using MediaPlayerDA;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static MediaPlayerBL.MediaBL;

namespace MediaPlayerBL
{
    public interface IMediaBL
    {
        List<Media> LoadMedia(string[] filenames);
        List<Media> LoadPlaylist(string filepath);
        BitmapImage CreateImage(string filepath);
        Uri CreateVideo(string filepath);   
        void SavePlaylist(string filepath, List<Media> loadedMedia);
        string GetPlaylistTitle();
        bool IsImageFormat(string filepath);
        bool IsVideoFormat(string filepath);
    }
}
