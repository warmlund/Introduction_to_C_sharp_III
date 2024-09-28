using MediaDTO;
using MediaPlayerDA;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MediaPlayerBL
{
    public class MediaBL : IMediaBL
    {
        private PlaylistManager _playlistManager;
        private MediaDA _mediaDA;
        private CancellationTokenSource _tokenSource;
        public event Action<Media> MediaChanged;
        public event Action<string> FormatChanged;
        private Media _currentPlayingMedia;
        private string _currentFormat;

        public int PlaySpeed { get; set; }
        public bool IsPlaying { get; set; }
        private int currentIndex = 0;
        public string CurrentFormat { get => _currentFormat; set { if (_currentFormat != value) { _currentFormat = value; FormatChanged?.Invoke(value); }; } }
        public Media CurrentPlayingMedia { get => _currentPlayingMedia; set { if (_currentPlayingMedia != value) { _currentPlayingMedia = value; MediaChanged?.Invoke(value); } } }

        public MediaBL()
        {
            this._mediaDA = new MediaDA();
            _playlistManager = new PlaylistManager();
            _currentPlayingMedia = null;
        }
        public List<Media> LoadPlaylist(string filepath)
        {
            _playlistManager.LoadPlaylist(_mediaDA, filepath);
            return _playlistManager.CurrentPlaylist.MediaFiles;
        }

        public List<Media> LoadMedia(string[] filenames)
        {
            currentIndex = 1;
            return _mediaDA.LoadMedia(filenames);
        }

        public void SavePlaylist(string filepath, List<Media> loadedMedia) => _playlistManager.SavePlaylist(_mediaDA, filepath, loadedMedia);

        public void SetPlaylistTitle(string title) => _playlistManager.CurrentPlaylist.PlaylistName = title;

        public string GetPlaylistTitle() => _playlistManager.CurrentPlaylist.PlaylistName;

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
                    _currentFormat = CurrentPlayingMedia.Format;

                    if (MediaFormatManager.IsVideoFormat(CurrentPlayingMedia.Format))
                    {
                        int duration = await MediaFormatManager.GetVideoDurationAsync(_currentPlayingMedia.FilePath);

                        if (duration <= 0)
                        {
                            duration = PlaySpeed;  
                        }

                        await Task.Delay(duration * 1000, _tokenSource.Token);
                    }
                    else
                    {
                        await Task.Delay(PlaySpeed * 1000, _tokenSource.Token);
                    }
                }
            }
            catch (TaskCanceledException)
            {
                _currentPlayingMedia = null;
                IsPlaying = false;

            }
            finally
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

        public bool IsImageFormat(string filepath)
        {
            if(MediaFormatManager.IsImageFormat(filepath))
                return true;
            return false;
        }

        public bool IsVideoFormat(string filepath)
        {
            if( MediaFormatManager.IsVideoFormat(filepath))
                return true;
            return false;
        }

        public BitmapImage CreateImage(string filePath)
        {
            return MediaFormatManager.CreateBitmap(filePath);
        }

        public Uri CreateVideo(string filePath)
        {
            return MediaFormatManager.CreateVideoUri(filePath);
        }
    }
}
