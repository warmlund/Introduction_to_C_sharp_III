using MediaDTO;
using MediaPlayerDA;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static MediaPlayerBL.MediaBL;

namespace MediaPlayerBL
{
    public interface IMediaBL
    {
        List<Media> LoadMedia(string[] filenames);
        List<Media> LoadPlaylist(string filepath);
        void SavePlaylist(string filepath, List<Media> loadedMedia);
        Task PlayMediaAsync(List<Media> loadedMedia);
        void PauseMedia();
        string GetPlaylistTitle();
        void SetPlaylistTitle(string title);
       
        void ResetIndex();

        event Action<Media> MediaChanged;

        int PlaySpeed { get; set; }
        bool IsPlaying { get; set; }
        Media CurrentPlayingMedia { get; set; }
    }
}
