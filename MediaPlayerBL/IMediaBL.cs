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
        Task PlayMediaAsync(List<Media> loadedMedia);
        BitmapImage CreateImage(string filepath);
        Uri CreateVideo(string filepath);   
        void SavePlaylist(string filepath, List<Media> loadedMedia);
        void PauseMedia();
        string GetPlaylistTitle();
        void SetPlaylistTitle(string title);
        void ResetIndex();
        bool IsImageFormat(string filepath);
        bool IsVideoFormat(string filepath);

        event Action<Media> MediaChanged;
        event Action<string> FormatChanged;

        int PlaySpeed { get; set; }
        bool IsPlaying { get; set; }
        Media CurrentPlayingMedia { get; set; }
        string CurrentFormat { get; set; }
    }
}
