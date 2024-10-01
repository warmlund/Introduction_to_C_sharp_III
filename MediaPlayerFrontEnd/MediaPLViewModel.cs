using MediaDTO;
using MediaPlayerBL;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MediaPlayerPL
{
    public class MediaPLViewModel : NotifyPropertyChanged
    {
        #region instance variables
        private int _interval;
        private double _videoProgress;
        private double _duration;
        private int _currentProgress;
        private double _progressValue;
        private string _playlistTitle;
        private string[] _selectedFiles;
        private string _currentFormat;
        private IMediaBL _mediaBl;
        private ObservableCollection<Media> _currentLoadedMedia;
        private CancellationTokenSource _tokenSource;
        private TaskCompletionSource<bool> _mediaOpenedTcs;
        private Media _currentPlayingMedia;
        private bool _isPlaying = false;
        private bool _isVideo = false;
        private bool _isImage = false;
        private bool _isIndexChanged = false;
        private BitmapImage _image;
        private Uri _video;
        public EventHandler PlayRequested;
        public EventHandler PauseRequested;
        public EventHandler StopRequested;
        #endregion

        #region commands
        public AsyncCommand Play { get; private set; }
        public Command LoadPlaylist { get; private set; }
        public Command SavePlaylist { get; private set; }
        public Command LoadMedia { get; private set; }
        public CommandWithParameter<Media> MoveMediaUp { get; private set; }
        public CommandWithParameter<Media> MoveMediaDown { get; private set; }
        #endregion

        #region Properties
        public ObservableCollection<Media> CurrentLoadedMedia { get { return _currentLoadedMedia; } set { if (_currentLoadedMedia != value) { _currentLoadedMedia = value; OnPropertyChanged(nameof(CurrentLoadedMedia)); } } }
        public int Interval { get { return _interval; } set { if (_interval != value) { _interval = value; OnPropertyChanged(nameof(Interval)); } } }
        public double VideoDuration { get { return _duration; } set { if (_duration != value) { _duration = value; OnPropertyChanged(nameof(VideoDuration)); } } }
        public double VideoProgress { get { return _videoProgress; } set { if (_videoProgress != value) { _videoProgress = value; OnPropertyChanged(nameof(VideoProgress));} } }
        public double ProgressValue { get { return _progressValue; } set { if (_progressValue != value) { _progressValue = value; OnPropertyChanged(nameof(ProgressValue)); } } }
        public string PlaylistTitle { get { return _playlistTitle; } set { if (_playlistTitle != value) { _playlistTitle = value; OnPropertyChanged(nameof(PlaylistTitle)); } } }
        public string[] SelectedFiles { get { return _selectedFiles; } set { if (_selectedFiles != value) { _selectedFiles = value; OnPropertyChanged(nameof(SelectedFiles)); } } }
        public bool IsPlaying { get { return _isPlaying; } set { if (_isPlaying != value) { _isPlaying = value; OnPropertyChanged(nameof(IsPlaying)); SavePlaylist.RaiseCanExecuteChanged(); LoadMedia.RaiseCanExecuteChanged(); LoadPlaylist.RaiseCanExecuteChanged(); } } }
        public bool IsImage { get { return _isImage; } set { if (_isImage != value) { _isImage = value; OnPropertyChanged(nameof(IsImage)); } } }
        public bool IsVideo { get { return _isVideo; } set { if (_isVideo != value) { _isVideo = value; OnPropertyChanged(nameof(IsVideo)); } } }
        public bool IsIndexChanged { get { return _isIndexChanged; } set { if (_isIndexChanged != value) { _isIndexChanged = value; OnPropertyChanged(nameof(IsIndexChanged)); } } }
        public Media CurrentPlayingMedia { get => _currentPlayingMedia; set { if (_currentPlayingMedia != value) { _currentPlayingMedia = value; OnPropertyChanged(nameof(CurrentPlayingMedia)); CheckFormatAndSetCurrentMedia(); } } }
        public string CurrentFormat { get => _currentFormat; set { if (_currentFormat != value) { _currentFormat = value; OnPropertyChanged(nameof(CurrentFormat)); } } }
        public BitmapImage CurrentImage { get => _image; set { if (_image != value) { _image = value; OnPropertyChanged(nameof(CurrentImage)); } } }
        public Uri CurrentVideo { get => _video; set { if (_video != value) { _video = value; OnPropertyChanged(nameof(CurrentVideo)); } } }
        public TaskCompletionSource<bool> TaskComplete { get => _mediaOpenedTcs; set { if (_mediaOpenedTcs != value) { _mediaOpenedTcs = value; OnPropertyChanged(nameof(TaskComplete)); } } }
        #endregion

        public MediaPLViewModel(IMediaBL mediaBL)
        {
            _mediaBl = mediaBL;
            _interval = 5;
            _currentProgress = 0;
            _currentLoadedMedia = new ObservableCollection<Media>();
            Play = new AsyncCommand(TogglePlayPause, CanPlayMedia);
            LoadPlaylist = new Command(LoadExistingPlaylist, CanLoadOrSave);
            SavePlaylist = new Command(SaveNewPlaylist, CanSaveNewPlaylist);
            LoadMedia = new Command(LoadNewMedia, CanLoadOrSave);
            MoveMediaUp = new CommandWithParameter<Media>(MoveUp, CanMoveUp);
            MoveMediaDown = new CommandWithParameter<Media>(MoveDown, CanMoveDown);

            CurrentLoadedMedia.CollectionChanged += OnCollectionChanged;
        }

        #region booleans
        private bool CanMoveDown(Media media)
        {
            if (CurrentLoadedMedia.IndexOf(media) != CurrentLoadedMedia.IndexOf(CurrentLoadedMedia.Last()) && IsPlaying == false)
                return true;
            return false;
        }
        private bool CanMoveUp(Media media)
        {
            if(CurrentLoadedMedia.IndexOf(media) > 0 && IsPlaying == false) 
                return true;
            return false;
        }
        private bool CanPlayMedia() => CurrentLoadedMedia.Count > 0;
        private bool CanSaveNewPlaylist()
        {
            if (_currentLoadedMedia.Count > 0 && IsPlaying == false)
                return true;

            return false;
        }
        private bool CanLoadOrSave()
        {
            if(IsPlaying == false) 
                return true;
            return false;
        }

        #endregion

        private async Task TogglePlayPause()
        {
            if (IsPlaying)
            {
                _tokenSource = new CancellationTokenSource();

                if (this.PlayRequested != null)
                {
                    this.PlayRequested(this, EventArgs.Empty);
                }
                await PlayMediaAsync(CurrentLoadedMedia.ToList());
            }

            else
            {
                if (this.PauseRequested != null)
                {
                    this.PauseRequested(this, EventArgs.Empty);
                }

                IsPlaying = false;
                _tokenSource?.Cancel();
            }
        }
        public async Task PlayMediaAsync(List<Media> loadedMedia)
        {
            try
            {
                for (int i = CurrentLoadedMedia.IndexOf(CurrentPlayingMedia); i < loadedMedia.Count; i++)
                {
                    if (_tokenSource.Token.IsCancellationRequested)
                    {
                        ResetIndexWhenPaused(i);
                        break;
                    }

                    CurrentPlayingMedia = loadedMedia[i];
                    CurrentFormat = CurrentPlayingMedia.Format;

                    // If the video was paused, use the existing VideoProgress value, else start from 0
                    if (CurrentPlayingMedia.Format == "video" && VideoProgress == 0)
                    {
                        ProgressValue = 0;
                    }

                    // Start playing the video
                    if (_mediaBl.IsVideoFormat(CurrentPlayingMedia.Format))
                    {
                        // Ensure the video is opened only once, avoiding re-awaiting the TaskCompletionSource
                        if (!_mediaOpenedTcs?.Task.IsCompleted ?? true)
                        {
                            _mediaOpenedTcs = new TaskCompletionSource<bool>();
                            await _mediaOpenedTcs.Task; // Wait for the video to be ready
                        }

                        // Resume from where the video left off
                        for (int j = (int)VideoProgress; j < VideoDuration; j++)
                        {
                            if (_tokenSource.Token.IsCancellationRequested)
                            {
                                VideoProgress = j; // Save the current progress when paused
                                ResetIndexWhenPaused(i);
                                return; // Exit both loops
                            }

                            // Update progress bar value based on the video's duration
                            ProgressValue = (j + 1) * (100.0 / VideoDuration);

                            // Handle cancellation token within Task.Delay
                            await Task.Delay(1000, _tokenSource.Token);
                        }

                        // After video finishes, reset VideoProgress
                        VideoProgress = 0;
                    }

                    // Handle image formats if necessary
                    else if (_mediaBl.IsImageFormat(CurrentPlayingMedia.Format))
                    {
                        for (int j = _currentProgress; j < Interval; j++)
                        {
                            if (_tokenSource.Token.IsCancellationRequested)
                            {
                                _currentProgress = j; // Save progress for image playback
                                ResetIndexWhenPaused(i);
                                return; // Exit both loops
                            }

                            ProgressValue = (j + 1) * (100.0 / Interval);
                            await Task.Delay(1000, _tokenSource.Token);
                        }

                        _currentProgress = 0; // Reset progress after image finishes
                    }

                    // If it's the last media, reset the player
                    if (i == CurrentLoadedMedia.Count - 1)
                    {
                        ResetPlayer();
                    }
                }
            }
            catch (TaskCanceledException)
            {
                IsPlaying = false;
            }
        }
        private void ResetIndexWhenPaused(int i)
        {
            if (i == 0)
                CurrentPlayingMedia = CurrentLoadedMedia.First();
            else
                CurrentPlayingMedia = CurrentLoadedMedia[i - 1];
        }
        private void ResetPlayer()
        {
            ProgressValue = 0;
            CurrentPlayingMedia = CurrentLoadedMedia.First();
            _currentProgress = 0;
            IsPlaying = false;
        }
        private void LoadExistingPlaylist()
        {
            var openManager = new OpenManager("Load Playlist");
            if (openManager.ShowDialog())
            {
                CurrentLoadedMedia.Clear();
                foreach (Media media in _mediaBl.LoadPlaylist(openManager.FilePath))
                {
                    CurrentLoadedMedia.Add(media);
                }

                PlaylistTitle = _mediaBl.GetPlaylistTitle();
                SetUpFirstLoadedMedia();
            }

            else
            {
                openManager.AlertUser();
            }
        }
        private void SaveNewPlaylist()
        {
            var saveManager = new SaveManager();
            if (saveManager.ShowDialog())
                _mediaBl.SavePlaylist(saveManager.FilePath, CurrentLoadedMedia.ToList());

            else
            {
                saveManager.AlertUser();
            }
        }
        private void LoadNewMedia()
        {
            var openManager = new OpenManager("Load Media");
            if (openManager.ShowDialog())
            {
                _selectedFiles = openManager.SelectedFiles;
                foreach (Media m in _mediaBl.LoadMedia(_selectedFiles))
                {
                    CurrentLoadedMedia.Add(m);
                }
                SetUpFirstLoadedMedia();
            }

            else
            {
                openManager.AlertUser();
            }
        }
        private void MoveUp(Media media)
        {
            int index = CurrentLoadedMedia.IndexOf(media);
            if (index > 0)
                SwapMedia(index, index - 1);
        }
        private void MoveDown(Media media)
        {
            int index = CurrentLoadedMedia.IndexOf(media);
            if (index < CurrentLoadedMedia.Count - 1)
                SwapMedia(index, index + 1);
        }
        private void SwapMedia(int oldIndex, int newIndex)
        {
            var tempMedia = CurrentLoadedMedia[oldIndex];
            CurrentLoadedMedia[oldIndex] = CurrentLoadedMedia[newIndex];
            CurrentLoadedMedia[newIndex] = tempMedia;
            CurrentPlayingMedia = CurrentLoadedMedia[0];
            //OnPropertyChanged(nameof(CurrentLoadedMedia));
            OnPropertyChanged(nameof(CurrentPlayingMedia));
        }
        private void SetUpFirstLoadedMedia()
        {
            CurrentPlayingMedia = CurrentLoadedMedia[0];
            CurrentFormat = CurrentPlayingMedia.Format;
            CheckFormatAndSetCurrentMedia();
        }
        private void CheckFormatAndSetCurrentMedia()
        {
            if (CurrentPlayingMedia != null)
            {
                if (_mediaBl.IsVideoFormat(CurrentPlayingMedia.Format))
                {
                    IsVideo = true;
                    IsImage = false;
                    OnPropertyChanged(nameof(IsVideo));
                    OnPropertyChanged(nameof(IsImage));
                    CurrentVideo = _mediaBl.CreateVideo(CurrentPlayingMedia.FilePath);
                }

                else if (_mediaBl.IsImageFormat(CurrentPlayingMedia.Format))
                {
                    IsVideo = false;
                    IsImage = true;
                    OnPropertyChanged(nameof(IsVideo));
                    OnPropertyChanged(nameof(IsImage));
                    CurrentImage = _mediaBl.CreateImage(CurrentPlayingMedia.FilePath);
                }
                else
                {
                    IsVideo = false;
                    IsImage = false;
                }
            }
        }
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CurrentLoadedMedia));
            Play.RaiseCanExecuteChanged();
            SavePlaylist.RaiseCanExecuteChanged();
        }
    }
}
