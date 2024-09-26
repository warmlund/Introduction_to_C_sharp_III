using MediaDTO;
using MediaPlayerDA;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MediaPlayerBL
{
    public class MediaBL : IMediaBL
    {
        private PlaylistManager playlistManager;
        private MediaDA _mediaDA;
        private CancellationTokenSource _tokenSource;
        public event Action<Media> MediaChanged;
        private Media _currentPlayingMedia;
        public int PlaySpeed { get; set; }
        public string CurrentFormat { get; set; }
        public bool IsPlaying { get; set; }

        private int currentIndex = 0;

        public Media CurrentPlayingMedia
        {
            get => _currentPlayingMedia;
            set
            {
                if (_currentPlayingMedia != value)
                {
                    _currentPlayingMedia = value;
                    MediaChanged?.Invoke(value);
                }
            }
        }

        public MediaBL()
        {
            this._mediaDA = new MediaDA();
            playlistManager = new PlaylistManager();
            _currentPlayingMedia = null;
        }
        public List<Media> LoadPlaylist(string filepath)
        {
            playlistManager.LoadPlaylist(_mediaDA, filepath);
            return playlistManager.CurrentPlaylist.MediaFiles;
        }

        public List<Media> LoadMedia(string[] filenames)
        {
            currentIndex = 0;
            return _mediaDA.LoadMedia(filenames);
        }

        public void SavePlaylist(string filepath, List<Media> loadedMedia) => playlistManager.SavePlaylist(_mediaDA, filepath, loadedMedia);

        public void SetPlaylistTitle(string title) => playlistManager.CurrentPlaylist.PlaylistName = title;

        public string GetPlaylistTitle() => playlistManager.CurrentPlaylist.PlaylistName;

        public async Task PlayMediaAsync(List<Media> loadedMedia)
        {
            _tokenSource = new CancellationTokenSource();

            IsPlaying = true;
            try
            {
                for (int i = currentIndex; i < loadedMedia.Count; i++)
                {
                    if (_tokenSource.IsCancellationRequested)
                    {
                        break;
                    }

                    _currentPlayingMedia = loadedMedia[i];
                    CurrentFormat = CurrentPlayingMedia.Format;
                    await Task.Delay(PlaySpeed * 1000, _tokenSource.Token);
                }
            }
            catch (TaskCanceledException)
            {
                IsPlaying = false;
            }
        }

        public void PauseMedia()
        {
            _tokenSource.Cancel();
            IsPlaying = false;
        }

        public void ResetIndex()
        {
            currentIndex = 0;
        }
    }
}
