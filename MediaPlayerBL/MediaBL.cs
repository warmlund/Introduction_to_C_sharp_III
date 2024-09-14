using System.Collections.Generic;
using MediaStorage;
using MediaPlayerDA;

namespace MediaPlayerBL
{
    public class MediaBL : IMediaBL
    {
        private PlaylistManager playlistManager;
        private MediaManager mediaManager;
        private IMediaDA mediaDA;

        public MediaBL(IMediaDA mediaDA)
        {
            this.mediaDA = mediaDA;
            playlistManager = new PlaylistManager();
            mediaManager = new MediaManager();
        }
        public void LoadMedia(IMediaDA mediaDA, string[] filenames)
        {
            mediaManager.LoadMedia(mediaDA,filenames);
        }

        public void LoadPlaylist(IMediaDA mediaDA, string filepath)
        {
            playlistManager.LoadPlaylist(mediaDA,filepath);
            mediaManager.LoadedMedia =playlistManager.Playlist.MediaFiles;
        }

        public bool SavePlaylist(IMediaDA mediaDA, string filepath, Playlist playliste)
        {
            return playlistManager.SavePlaylist(mediaDA,filepath,playliste);
        }

        public void SetInterval(int interval)
        {
            mediaManager.SetPlaySpeed(interval);
        }

        public void ArrangeMedia(List<Media> NewSorting)
        {
            mediaManager.ArrangeMedia(NewSorting);
        }

        public void PlayMedia()
        {
            mediaManager.PlayMedia();
        }

        public void PauseMedia()
        {
            mediaManager.PauseMedia();
        }

    }
}
