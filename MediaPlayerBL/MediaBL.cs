using MediaDTO;
using MediaPlayerDA;
using System.Collections.Generic;

namespace MediaPlayerBL
{
    public class MediaBL : IMediaBL
    {
        private PlaylistManager playlistManager;
        private MediaManager mediaManager;
        private MediaDA _mediaDA;
        public Media CurrentPlayingMedia { get { return mediaManager.ShownMedia; } set { if (value != null) { mediaManager.ShownMedia = value; } } }

        public MediaBL()
        {
            this._mediaDA = new MediaDA();
            playlistManager = new PlaylistManager();
            mediaManager = new MediaManager();
        }
        public List<Media> LoadPlaylist(string filepath)
        {
            playlistManager.LoadPlaylist(_mediaDA, filepath);
            return playlistManager.CurrentPlaylist.MediaFiles;
        }

        public List<Media> LoadMedia(string[] filenames) => mediaManager.LoadMedia(_mediaDA, filenames);

        public void SavePlaylist(string filepath, List<Media> loadedMedia) => playlistManager.SavePlaylist(_mediaDA, filepath, loadedMedia);

        public void SetInterval(int interval) => mediaManager.SetPlaySpeed(interval);

        public int GetInterval() => mediaManager.PlaySpeed;

        public void SetPlaylistTitle(string title) => playlistManager.CurrentPlaylist.PlaylistName = title;

        public string GetPlaylistTitle() => playlistManager.CurrentPlaylist.PlaylistName;

        public void PlayMedia(List<Media> loadedMedia) => mediaManager.PlayMedia(loadedMedia);

        public void PauseMedia() => mediaManager.PauseMedia();

        public bool GetIsMediaPlaying() => mediaManager.IsPlaying;

        public void SetMediaPlayPause(bool value) => mediaManager.IsPlaying = value; 
    }
}
