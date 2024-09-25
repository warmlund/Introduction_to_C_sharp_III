using MediaDTO;
using System.Collections.Generic;

namespace MediaPlayerBL
{
    public interface IMediaBL
    {
        List<Media> LoadMedia(string[] filenames);
        void SetInterval(int interval);
        int GetInterval();
        List<Media> LoadPlaylist(string filepath);
        void SavePlaylist(string filepath, List<Media> loadedMedia);
        void PlayMedia(List<Media> loadedMedia);
        void PauseMedia();
        string GetPlaylistTitle();
        void SetPlaylistTitle(string title);
        bool GetIsMediaPlaying();
        void SetMediaPlayPause(bool value);

        Media CurrentPlayingMedia { get; set; }
    }
}
