﻿using MediaDTO;
using MediaPlayerDA;
using System.Collections.Generic;

namespace MediaPlayerBL
{
    public class MediaBL : IMediaBL
    {
        private PlaylistManager playlistManager;
        private MediaManager mediaManager;
        private MediaDA _mediaDA;

        public MediaBL()
        {
            this._mediaDA = new MediaDA();
            playlistManager = new PlaylistManager();
            mediaManager = new MediaManager();
        }
        public List<Media> LoadPlaylist(string filepath)
        {
            playlistManager.LoadPlaylist(_mediaDA, filepath);
            return playlistManager.Playlist.MediaFiles;
        }

        public List<Media> LoadMedia(string[] filenames) => mediaManager.LoadMedia(_mediaDA, filenames);

        public void SavePlaylist(string filepath, List<Media> loadedMedia) => playlistManager.SavePlaylist(_mediaDA, filepath, loadedMedia);

        public void SetInterval(int interval) => mediaManager.SetPlaySpeed(interval);

        public int GetInterval() => mediaManager.PlaySpeed;

        public void SetPlaylistTitle(string title) => playlistManager.Playlist.PlaylistName = title;

        public string GetPlaylistTitle() => playlistManager.Playlist.PlaylistName;

        public void PlayMedia(List<Media> loadedMedia) => mediaManager.PlayMedia(loadedMedia);

        public void PauseMedia() => mediaManager.PauseMedia();

        public bool GetIsMediaPlaying() => mediaManager.IsPlaying;

        public void SetMediaPlayPause(bool value) => mediaManager.IsPlaying = value; 
    }
}
